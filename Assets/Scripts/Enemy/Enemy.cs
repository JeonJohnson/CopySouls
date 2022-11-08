using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

using Enums;
using Structs;


//<필요 함수>
//각도 , 데미지
//적이 데미지 입는 함수
//앞잡 뒤잡 시 몬스터가 그 방향으로 회전
//
//잡기시 몬스터 데미지 입는함수
// 적 스크립트에서 적이 움직이지 못하게 하고 맞는 애니메이션을 재생시키는 함수 재생
// ex ) HoldAttackHit(Vector3 Playerpos, Vector3 rotation)

public enum eCombatState
{
    Idle, //암것도 아님
    Alert, //경계 상태
    Combat, //전투 모드
    End
}

public enum eEquipState
{
    None, //무기 안낌
    Equip, //무기 낌
    End
}

public abstract class Enemy : MonoBehaviour
{
    public Structs.EnemyStatus status;
    

    public GameObject targetObj;
    public Player targetScript;
    public float distToTarget;
    public Vector3 dirToTarget; //정규화된 값임 (normalize된거)

    public eCombatState combatState = eCombatState.Idle;
    public eEquipState weaponEquipState = eEquipState.None;
    //public bool isAlert = false;
    //public bool isCombat = false;

    public Weapon weapon;

    public Transform[] PatrolPos;
    //public List<Vector3> patrolPosList; //에너미에서 Transform으로 합치기

    ////FSM
    public cState[] fsm;
    public cState preState = null;
    public int preState_i = -1;
    public cState curState = null;
    public int curState_i = -1;
    ////FSM

    ////default Components
    public Animator animCtrl;
    public Collider col;
    public Rigidbody rd;
    public NavMeshAgent navAgent;
    ////default Components




    public void ResetAllAnimTrigger(string[] triggerStrArr)
    {
        for (int i = 0; i < triggerStrArr.Length; ++i)
        {
            animCtrl.ResetTrigger(triggerStrArr[i]);
        }
    }

    public void CalcAboutTarget()
    {
        //22 10 02 fin, 설명해주기
        if (targetObj == null)
        { return; }


        distToTarget = Vector3.Distance(transform.position, targetObj.transform.position);
        dirToTarget = (targetObj.transform.position - transform.position).normalized;
    }

	public Quaternion LookAtSlow(Transform me, Transform target, float spd)
	{
        Vector3 tempDir = dirToTarget;
        tempDir.y = 0;

        Quaternion angle = Quaternion.LookRotation(tempDir);

        return Quaternion.Lerp(me.rotation, angle, Time.deltaTime * spd);
    }

    public Quaternion LookAtSlow(Transform me, Vector3 targetPos, float spd)
    {
        Vector3 tempDir = (targetPos - me.position).normalized;
        tempDir.y = 0;

        Quaternion angle = Quaternion.LookRotation(tempDir);

        return Quaternion.Lerp(me.rotation, angle, Time.deltaTime * spd);
    }

    #region LookAt_Animation Bone
    //LateUpdate에서 써야함!!!
    public void LookAtSpecificBone(HumanBodyBones boneName, Transform targetTr, Vector3 offsetEulerRotate)
    {
        Transform boneTr = animCtrl.GetBoneTransform(boneName);
        boneTr.LookAt(targetTr);
        boneTr.rotation = boneTr.rotation * Quaternion.Euler(offsetEulerRotate);
    }

    public void LookAtSpecificBone(Transform boneTr, Transform targetTr, eGizmoDirection boneDir)
    {
        Vector3 lookDir = (targetTr.position - boneTr.position).normalized;

        switch (boneDir)
        {
            case eGizmoDirection.Foward:
                {
                    boneTr.forward = lookDir;
                }
                break;
            case eGizmoDirection.Back:
                {
                    boneTr.forward = -lookDir;
                }
                break;
            case eGizmoDirection.Right:
                {
                    boneTr.right = lookDir;
                }
                break;
            case eGizmoDirection.Left:
                {
                    boneTr.right = -lookDir;
                }
                break;
            case eGizmoDirection.Up:
                {
                    boneTr.up = lookDir;
                }
                break;
            case eGizmoDirection.Down:
                {
                    boneTr.up = -lookDir;
                }
                break;

            default:
                {
                    Debug.Log("Enemy bone LookAt Error");
                }
                break;
        }
    }

    public void LookAtSpecificBone(Transform boneTr, Transform targetTr, eGizmoDirection boneDir, Vector3 offsetEulerRotate)
    {

        Vector3 lookDir = (targetTr.position - boneTr.position).normalized;

        switch (boneDir)
		{
			case eGizmoDirection.Foward:
                {
                    boneTr.forward = lookDir;
                }
				break;
			case eGizmoDirection.Back:
                {
                    boneTr.forward = -lookDir;
                }
				break;
			case eGizmoDirection.Right:
                {
                    boneTr.right = lookDir;
                }
				break;
			case eGizmoDirection.Left:
                {
                    boneTr.right = -lookDir;
                }
				break;
			case eGizmoDirection.Up:
                {
                    boneTr.up = lookDir;
                }
				break;
			case eGizmoDirection.Down:
                {
                    boneTr.up = -lookDir;
                }
				break;

            default:
                {
                    Debug.Log("Enemy bone LookAt Error");
                }
				break;
		}

		boneTr.rotation = boneTr.rotation * Quaternion.Euler(offsetEulerRotate);
    }
    #endregion

    public void SetDestination(Vector3 dest)
    {
        if (navAgent == null || navAgent.isStopped) return;
        navAgent.destination = dest;
    }

    public void MoveOrder(Vector3 dest)
    {//네비 에이전트 움직이는거 편하게
        if (navAgent == null)
        {
            return;
        }

        navAgent.isStopped = true;
        navAgent.destination = dest;
        navAgent.isStopped = false;
    }

    public void MoveStop()
    {
        if (navAgent == null)
        {
            return;
        }

        navAgent.isStopped = true;
        navAgent.SetDestination(transform.position);
    }

 


    public abstract void InitializeState();

	public T GetCurState<T>() where T : Enum
    {
        int index = System.Array.IndexOf(fsm, curState);

        if (curState == null)
        {
            Debug.Log("현재 state가 null입니다!!\nAt GetCurState Funcs");
        }

        return (T)(object)index;
    }


    public void SetState(cState state)
    {
        
        int index = System.Array.IndexOf(fsm, state);

        if (index == -1 || curState == state)
        {//넣은 state가 null이거나 없는 경우 
            return;
        }

        if (curState != null)
        {
            curState.ExitState();
        }

        //cState nextState = fsm[index];

        int curIndex = System.Array.IndexOf(fsm, curState);

        preState = curState;
        preState_i = curIndex;

        curState = state;
        curState_i = index;

        curState.EnterState(this);
    }


    public void SetState(int state)
    {
        if (fsm[state] == null)
        {
            return;
        }

        if (curState == fsm[state])
        {
            return;
        }

        if (curState != null)
        {
            curState.ExitState();
        }

        cState nextState = fsm[state];
        int curIndex = System.Array.IndexOf(fsm, curState);

        preState = curState;
        preState_i = curIndex;

        curState = nextState;
        curState_i = state;

        curState.EnterState(this);
    }

    public void RestartCurState()
    {
        curState.ExitState();

        preState = curState;
        
        int curIndex = System.Array.IndexOf(fsm, curState);
        preState_i = curIndex;

        curState.EnterState(this);
    }

    public void stop()
    {
        StopAllCoroutines();
    }

    protected virtual void Awake()
    {
        animCtrl = GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();

        //for Test
        //patrolPosList = new List<Vector3>();
        //patrolPosList.Add(new Vector3());
        //patrolPosList.Add(new Vector3(10,0,10));
        //patrolPosList.Add(new Vector3(5,0,-10));
        //for Test

        //Enemy상속 받은 객체 각자 스크립트에서 설정해주기
        InitializeState();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
        if (weapon != null)
        { weapon.owner = gameObject; }
    }

    public void GetPlayerState()
    {



    }



    // Update is called once per frame
    protected virtual void Update()
    {
        GetPlayerState();
        
        CalcAboutTarget();

        
        //isAlert = CheckTargetInFov();

        curState.UpdateState();

    }

    protected virtual void FixedUpdate()
    {
        curState.FixedUpdateState();
    }

    protected virtual void LateUpdate()
    {
        curState.LateUpdateState();
    }


    //=============================================================
    //Damaged함수 --> 조정필요할듯 앞잡뒤잡에 관한...

    //용석 : 트리거를 통한 enemy의 curHP 차감
    public void Damaged(int dmg)
    {
        //status.curHp -= dmg;
    }

    //근희
    public virtual void Hit(DamagedStruct dmgStruct)
    {
        //status.curHp -= (int)dmgStruct.dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!status.isDead)
        {
            if(other.gameObject.GetComponent<Weapon>() != null)
            {
                if (other.gameObject.GetComponent<Weapon>().owner == targetObj)
                {
                    Damaged(other.gameObject.GetComponent<Weapon>().Dmg);
                }
            }
        }

        //if (other.CompareTag("Weapon"))
        //{
        //    Structs.DamagedStruct dmg = new Structs.DamagedStruct();
        //
        //    dmg.dmg = 10f;
        //
        //    Hit(dmg);
        //    //Debug.Log("한대맞음 으엑");
        //    //Enemy script = other.GetComponent<Enemy>();
        //
        //}
    }

    //=============================================================





    //private void OnDrawGizmos()
    //{

    //    ////인식범위
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, status.ricognitionRange);
    //    ////인식범위

    //    ////시야각
    //    //프로스텀으로 보여주기

    //    ////시야각

    //    ////공격 사정거리
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, status.atkRange);
    //    ////공격 사정거리


    //    ////패트롤 예상 이동 궤적
    //    Gizmos.color = Color.blue;

    //    for (int i = 0; i < patrolPosList.Count; ++i)
    //    {
    //        if (i == (patrolPosList.Count - 1))
    //        {
    //            Gizmos.DrawLine(patrolPosList[i], patrolPosList[0]);
    //        }
    //        else
    //        {
    //            Gizmos.DrawLine(patrolPosList[i], patrolPosList[i + 1]);
    //        }
    //    }
    //}


    private void OnDrawGizmosSelected()
	{
        ////Dir to Target
        if(targetObj != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, targetObj.transform.position);
        }
        ////Dir to Target
        Color temp = Color.yellow;
        temp.a = 0.4f;
        Gizmos.color = Color.yellow;
        Gizmos.color = temp;
        Gizmos.DrawSphere(transform.position, status.ricognitionRange);
        ////인식범위

        //Test SpinAtt
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, Destination);

        //정면 
        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position,transform.forward * 1000f);

        ////시야각
        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(transform.position, fovStruct.LeftDir * status.ricognitionRange);
        //Gizmos.DrawRay(transform.position, fovStruct.RightDir * status.ricognitionRange);
        //if (isCombat)
        //{
        //    Gizmos.DrawRay(transform.position, dirToTarget*distToTarget);
        //}

        ////공격 사정거리
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.atkRange);
		////공격 사정거리


		////패트롤 예상 이동 궤적
		//Gizmos.color = Color.blue;
        //
		//for (int i = 0; i < patrolPosList.Count; ++i)
		//{
        //    if (i == (patrolPosList.Count - 1))
        //    {
        //        Gizmos.DrawLine(patrolPosList[i], patrolPosList[0]);
        //    }
        //    else
        //    {
        //        Gizmos.DrawLine(patrolPosList[i], patrolPosList[i + 1]);
		//	}
		//}
		////패트롤 예상 이동 궤적
	}
}

