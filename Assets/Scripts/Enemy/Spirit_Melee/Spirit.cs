using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;
using Structs;

public class Spirit : Enemy
{
    public Vector3 respawnPos;

    public Transform targetHeadPos;
    public Transform headPos;
    public Transform head;
    public GameObject model;
    //public GameObject ragdollModel;
    public GameObject remainderWeapon;
    public Spirit_Weapon weapon;
    public LayerMask player_Hitbox;
    public int preHp;
    public int prePatrol_Index;
    public eSpiritState curState_e;
    public float distToRespawnPos;

    public bool isGetUp;
    public float HoldCoolTime = 1.5f;
    public float Timer;

    public float initFOVAngle;
    public int curPatrol_Index;
    public bool Arrival;
    public bool complete_Equipt;
    public bool complete_Unequipt;
    public bool complete_Atk;
    public bool complete_AttReturn;
    public bool complete_Damaged;
    public bool complete_Groggy;
    public bool isEquipt;
    public bool atting;
    public bool existRemainderWeapon;
    public bool preChangeWeaponPos;
    public bool doubleAttCheck;
    public bool isReturn;
    public bool stepWait;
   
    public override void InitializeState()
	{
        fsm = new cState[(int)Enums.eSpiritState.End];

        fsm[(int)Enums.eSpiritState.Idle] = new Spirit_Idle();
        fsm[(int)Enums.eSpiritState.Patrol] = new Spirit_Patrol();
        fsm[(int)Enums.eSpiritState.Equipt] = new Spirit_Equipt();
        fsm[(int)Enums.eSpiritState.Unequipt] = new Spirit_Unequipt();
        fsm[(int)Enums.eSpiritState.Trace] = new Spirit_Trace();
        fsm[(int)Enums.eSpiritState.Atk] = new Spirit_Atk();
        fsm[(int)Enums.eSpiritState.Damaged] = new Spirit_Damaged();
        fsm[(int)Enums.eSpiritState.Groggy] = new Spirit_Groggy();
        fsm[(int)Enums.eSpiritState.Hold] = new Spirit_Hold();
        fsm[(int)Enums.eSpiritState.Death] = new Spirit_Death();
        fsm[(int)Enums.eSpiritState.Return] = new Spirit_Return();

        SetState((int)Enums.eSpiritState.Idle);
	}
	protected override void Awake()
    {
        base.Awake();
        respawnPos = transform.localPosition;
        initFOVAngle = GetComponent<FieldOfView>().viewAngle;

        weapon = GetComponentInChildren<Spirit_Weapon>();
        if (weapon != null)
        { weapon.owner = gameObject; }
    }

    protected override void Start()
    {
        base.Start();

        //targetObj = GameObject.Find("Player");
        //if (targetObj != null) targetScript = targetObj.GetComponent<Player>();

        targetObj = UnitManager.Instance.GetPlayerObj;
        if (targetObj != null) targetScript = UnitManager.Instance.GetPlayerScript;

        player_Hitbox = 1 << LayerMask.NameToLayer("Player_Hitbox");
        head = animCtrl.GetBoneTransform(HumanBodyBones.Head);
    }

    protected override void Update()
    {
        base.Update();

        distToRespawnPos = Vector3.Distance(respawnPos, transform.position);
        if (distToRespawnPos > status.moveMileage) isReturn = true;
        curState_e = GetCurState<Enums.eSpiritState>();

        //=======================================
        //TestCtrl

        //-->Hit
        if (Input.GetKeyDown(KeyCode.C))
        {
            preHp = status.curHp;
            status.curHp--;
            HitCount++;
        }

        //->Groggy
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (curState_e == eSpiritState.Atk)
            {
                status.isGroggy = true;
            }
        }
        //=======================================


        if (status.curHp <= 0 && curState_e != eSpiritState.Hold)
        {
            status.isDead = true;
            SetState((int)Enums.eSpiritState.Death);
        }
        else if(status.curHp > 0 && !status.isDead)
        {
            if (status.isBackHold || status.isFrontHold )
            {
                //잡기
                if (status.isBackHold && status.isFrontHold)
                {
                    Debug.Log("뒤잡앞잡 동시 발동 : 판정 error");
                }
            }
            //else if (!status.isBackHold && !status.isFrontHold)
            //{
            //    if (HitCount > 0)
            //    {
            //        if (curState_e != eSpiritState.Damaged)
            //        {
            //            SetState((int)Enums.eSpiritState.Damaged);
            //        }
            //    }
            //}

            //if (HitCount > 0)
            //{
            //    if (!status.isBackHold && !status.isFrontHold)
            //    {
            //        if (curState_e != eSpiritState.Damaged)
            //        {
            //            SetState((int)Enums.eSpiritState.Damaged);
            //        }
            //    }
            //}
        }

        //모종의 이유로 무기해제시
        if (curState_e == eSpiritState.Atk || curState_e == eSpiritState.Trace)
        {
            if (weaponEquipState == eEquipState.UnEquip) SetState((int)Enums.eSpiritState.Idle);
        }
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.tag == "ReturnBoundary" && curState_e != eSpiritState.Return && !status.isDead)
        {
            isReturn = true;
        }
    }


    public override void Hit(DamagedStruct dmgStruct)
    {
        base.Hit(dmgStruct);
        if (!status.isBackHold && !status.isFrontHold) HitCount++;
        else
        {
            if(HitCount > 0) HitCount = 0;
        }
        //if (!dmgStruct.isBackstab && !dmgStruct.isRiposte)
        //{
        //   HitCount++;
        //}
    }



    //=============================================================================
    // 렉돌교체함수
    //=============================================================================
    public void ChangeToRagDoll()
    {
        if(model.activeSelf)
        {
            CopyCharacterTransfoemRoRagdoll(model.transform, ragdoll.transform);
            model.SetActive(false);
            //GameObject ragdollObj = ragdoll.gameObject;
            ragdoll.gameObject.SetActive(true);
        }
    }

    private void CopyCharacterTransfoemRoRagdoll(Transform origin, Transform ragdoll)
    {
        for(int i = 0; i < origin.childCount; i++)
        {
            if(origin.childCount != 0)
            {
                CopyCharacterTransfoemRoRagdoll(origin.GetChild(i), ragdoll.GetChild(i));
            }
            ragdoll.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            ragdoll.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }
    }

    //상체 본만 회전하는 함수
    public void boneRotation(HumanBodyBones boneName, Transform targetTr, Vector3 offsetEulerRotate)
    {
        Transform boneTr = animCtrl.GetBoneTransform(boneName);
        boneTr.LookAt(targetTr);
        boneTr.rotation = boneTr.rotation * Quaternion.Euler(offsetEulerRotate);
    }

    //=============================================================================

    //=============================================================================
    //instance
    //=============================================================================
    public void CreateRemainderWeapon(Transform trans)
    {
        if (!existRemainderWeapon)
        {
            existRemainderWeapon = true;
            GameObject obj = Instantiate(remainderWeapon);
            obj.transform.position = trans.position;
            obj.transform.rotation = trans.rotation;
        }
        else return;
    }

    //=============================================================================
    //애니메이션 이벤트

    public void Spirit_StepWait() { if (!stepWait) stepWait = true; }
    public void Spirit_StepStart() { if (stepWait) stepWait = false; }
    public void Spirit_Melee_CompleteEquiptment() { complete_Equipt = true; }
    public void Spirit_Melee_CompleteUnequiptment() { complete_Unequipt = true; }
    public void Spirit_Melee_CompleAtk() { complete_Atk = true; }
    public void Spirit_Melee_DoubleAttCheck() { doubleAttCheck = true; }

    public void Spirit_Damaged() { complete_Damaged = true; }
    public void Spirit_Groggy() { complete_Groggy = true; }
    public void Spirit_Atting()
    {
        if(curState_e == Enums.eSpiritState.Atk)
        {
            if (!atting)
            {
                atting = true;
            }
            else if(atting)
            {
                atting = false;
            }
        }
    }
    public bool isCurrentAnimationOver(Animator animator,float time)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > time;
    }

    //=============================================================================
}



