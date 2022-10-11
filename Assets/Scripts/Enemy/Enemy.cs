using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Enums;
using Structs;


//<필요 함수>
//각도 , 데미지
//적이 데미지 입는 함수
//앞잡 뒤잡 시 몬스터가 그 방향으로 회전
//

public abstract class Enemy : MonoBehaviour
{
    public Structs.EnemyStatus status;

    
    ////Target
    //public GameObject player;

    public GameObject targetObj;
    public float distToTarget;
    public Vector3 dirToTarget; //정규화된 값임 (normalize된거)
    public Vector3 preTargetPos;
    public Vector3 curTargetPos;
    public float CoolTime;
    ////Target



    //// Events
    //public delegate void Al
    public delegate void AlertEventHandler();
    public AlertEventHandler alertStartEvent;
    public AlertEventHandler alertEndEvent;

    public delegate void CombatEventandler();
	public CombatEventandler combatStartEvent;
    public CombatEventandler combatEndEvent;
    //// Events

    public FovStruct fovStruct;
    public bool isAlert = false;
    public bool isCombat = false;

    public GameObject weapon;
    public List<Vector3> patrolPosList;

    ////FSM
    public cState[] fsm;
    public cState preState = null;
    public int preState_i = -1;
    //public eEnmeyState preState_e = eEnmeyState.End;
    public cState curState = null;
    public int curState_i = -1;
    //public eEnmeyState curState_e = eEnmeyState.End;
    ////FSM

    ////default Components
    public Animator animCtrl;
    public Collider col;
    public Rigidbody rd;
    public NavMeshAgent navAgent;
    ////default Components


    //public void UpdateStatus()
    //{ //스테이터스 수치들 각종 컴포넌트에 연동 되도록.

    //    //네브 요원
    //    navAgent.speed = status.moveSpd;
        
    
    //}


    public void CalcAboutTarget()
    {
        //22 10 02 fin, 설명해주기
        if (targetObj == null)
        { return; }


        distToTarget = Vector3.Distance(transform.position, targetObj.transform.position);
        dirToTarget = (targetObj.transform.position - transform.position).normalized;

        preTargetPos = curTargetPos;
        curTargetPos = targetObj.transform.position;
	}

	public Quaternion LookAtSlow(Transform me, Transform target, float spd)
	{
        Vector3 tempDir = dirToTarget;
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

    //public void MoveOrder(Vector3 dir)
    //{
    //    if (navAgent == null)
    //    {
    //        return;
    //    }

    //    navAgent.isStopped = true;
    //    navAgent.Move(dir);
    //    navAgent.isStopped = false;
    //}

    public void MoveStop()
    {
        if (navAgent == null)
        {
            return;
        }

        navAgent.isStopped = true;
    }


	public void CalcFovDir(float degreeAngle)
	{
        //22 10 02 fin, 설명해주기

        //시야각도를 이용해서 ㄹㅇ 시야각 구하기

        //f=Forward
        // A   f    B
        // \   |   /
        //  \  |  /
        //   \ | /
        //-----0-------

        //forward와 A사이의 각도 @1
        //=> @1 = Dot(f,A) * aCos;

        //forword와 B 사이의 각도 @2
        //=> @2 = Dot(f,B) * aCos;

        //판별기준은 몬스터와 0의 각도를 구한다음
        //그게 fov/2 보다 작으면 시야각 내에 있는거.

        //여기서는 A와 B의 Direction 구하는거
        fovStruct.fovAngle = degreeAngle;
        fovStruct.LeftDir = Funcs.DegreeAngle2Dir(transform.eulerAngles.y - (status.fovAngle * 0.5f));
        fovStruct.LookDir = Funcs.DegreeAngle2Dir(transform.eulerAngles.y);
        fovStruct.RightDir = Funcs.DegreeAngle2Dir(transform.eulerAngles.y + (status.fovAngle * 0.5f));
    }


    public bool CheckTargetInFov()
    {
        //22 10 02 fin, 설명해주기

        Collider[] hitObjs = Physics.OverlapSphere(transform.position, status.ricognitionRange);

        if (hitObjs.Length == 0)
        {
            //if (isCombat)
            //{
            //    combatEndEvent();
            //}

            return false;
        }

        foreach (Collider col in hitObjs)
        {
            if (col.gameObject != targetObj)
            {

                continue;
            }

           // Vector3 dir = (targetObj.transform.position - transform.position).normalized;

            float angleToTarget = Mathf.Acos(Vector3.Dot(fovStruct.LookDir, dirToTarget)) * Mathf.Rad2Deg;
            //내적해주고 나온 라디안 각도를 역코사인걸어주고 오일러각도로 변환.
            
            int layerMask = (1 << LayerMask.NameToLayer("Environment")) | (1<< LayerMask.NameToLayer("Enemy"));
            
            if (angleToTarget <= (fovStruct.fovAngle * 0.5f)
                && !Physics.Raycast(transform.position, dirToTarget, status.ricognitionRange, layerMask))
            {
                if (!isAlert)
                {
                    isAlert = true;
                    alertStartEvent();
                }

                return true;
            }
        }

        //if (isCombat)
        //{
        //    combatEndEvent();
        //}

        return false;
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
        if (fsm[state] == null
            || curState == fsm[state])
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

    //public IEnumerator DelaySetState(float delayTime, eArcherState state)
    //{
    //    float time = 0f;
    //    while (time <= delayTime)
    //    {
    //        time += Time.deltaTime;

    //        yield return null;
    //    }


    //}


    public void stop()
    {
        StopAllCoroutines();
    }

    public virtual void Hit(DamagedStruct dmgStruct)
    {
        status.curHp -= (int)dmgStruct.dmg;

        //if (dmgStruct.isBackstab)
        //{
        //    transform.forward = player.transform.forward;
        //}
    }
    //public abstract void Death();

    protected virtual void Awake()
    {
        animCtrl = GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();


        //for Test
        patrolPosList = new List<Vector3>();
        patrolPosList.Add(new Vector3());
        patrolPosList.Add(new Vector3(10,0,10));
        patrolPosList.Add(new Vector3(5,0,-10));
        //for Test


        //Enemy상속 받은 객체 각자 스크립트에서 설정해주기
        InitializeState();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CalcAboutTarget();

        CalcFovDir(status.fovAngle);
        isAlert = CheckTargetInFov();

        curState.UpdateState();

        CoolTime += Time.deltaTime;
    }

    protected virtual void LateUpdate()
    {
        curState.LateUpdateState();
    
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        Structs.DamagedStruct dmg = new Structs.DamagedStruct(10, false);

    //        Enemy script = other.GetComponent<Enemy>();
    //        script.Hit(dmg);
    //    }

    //}


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
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, targetObj.transform.position);
        ////Dir to Target



        ////인식범위
        Color temp = Color.yellow;
        temp.a = 0.4f;
        //Gizmos.color = Color.yellow;
        Gizmos.color = temp;
        Gizmos.DrawSphere(transform.position, status.ricognitionRange);
        ////인식범위

        ////시야각
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, fovStruct.LeftDir *status.ricognitionRange);
        Gizmos.DrawRay(transform.position, fovStruct.RightDir * status.ricognitionRange);
        if (isCombat)
        {
            Gizmos.DrawRay(transform.position, dirToTarget*distToTarget);
        }
        ////시야각

        ////공격 사정거리
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.atkRange);
		////공격 사정거리


		////패트롤 예상 이동 궤적
		Gizmos.color = Color.blue;

		for (int i = 0; i < patrolPosList.Count; ++i)
		{
            if (i == (patrolPosList.Count - 1))
            {
                Gizmos.DrawLine(patrolPosList[i], patrolPosList[0]);
            }
            else
            {
                Gizmos.DrawLine(patrolPosList[i], patrolPosList[i + 1]);
			}
		}
		////패트롤 예상 이동 궤적
	}
}
