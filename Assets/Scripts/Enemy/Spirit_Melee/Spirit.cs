using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;

//스턴 상태 만들어야함

// 연속공격? 데미지 모션 -> 랜덤
//    public float curActAtkValue = 1.0f;
// 강공 -> 데미지 처리 ()
// 
// 

public class Spirit : Enemy
{
    public Collider dashCol;
    public float dashTime = 4f;
    public int preHp;
    public GameObject remainderWeapon;
    public Material Material_Disable;
    public Material Material_Standard;
    public float initFOVAngle;

    public eSpiritState curState_e;
    public bool Arrival;

    public Transform[] PatrolPos;
    public int curPatrol_Index;
    public int prePatrol_Index;

    public bool complete_Equipt;
    public bool complete_Unequipt;
    public bool complete_Atk;
    public bool complete_AttReturn;
    public bool complete_Damaged;
    public bool isEquipt;
    public bool atting;
    public bool existRemainder;
    //public bool complete_Combo1;
    public bool transWeaponPos;

    //step
    public bool stepWait;
   
    public override void InitializeState()
	{
        fsm = new cState[(int)Enums.eSpiritState.End];

        fsm[(int)Enums.eSpiritState.Idle] = new Spirit_Idle();
        fsm[(int)Enums.eSpiritState.Patrol] = new Spirit_Patrol();
        fsm[(int)Enums.eSpiritState.Equipt] = new Spirit_Equipt();
        fsm[(int)Enums.eSpiritState.Unequipt] = new Spirit_Unequipt();
        fsm[(int)Enums.eSpiritState.Trace] = new Spirit_Trace();
        fsm[(int)Enums.eSpiritState.Dash] = new Spirit_Dash();
        fsm[(int)Enums.eSpiritState.Atk] = new Spirit_Atk();
        fsm[(int)Enums.eSpiritState.Damaged] = new Spirit_Damaged();
        fsm[(int)Enums.eSpiritState.Death] = new Spirit_Death();

        SetState((int)Enums.eSpiritState.Idle);
	}

	protected override void Awake()
    {
        base.Awake();
        initFOVAngle = GetComponent<FieldOfView>().viewAngle;
    }

    protected override void Start()
    {
        base.Start();
        targetObj = GameObject.Find("Player");
        weapon.Type = eWeaponType.Melee;
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.C))
        {
            status.curHp--;
        }

        if (status.curHp <= 0)
        {
            isDead = true;
            SetState((int)Enums.eSpiritState.Death);
            return;
        }
        else if(status.curHp > 0 && !isDead)
        {
            if (preHp > status.curHp && curState_e != Enums.eSpiritState.Damaged)
            {
                SetState((int)Enums.eSpiritState.Damaged);
            }
        }
        curState_e = GetCurState<Enums.eSpiritState>();

        if(preHp != status.curHp) preHp = status.curHp;
    }
    //=============================================================================
    // MaterialChange함수
    //=============================================================================
    public void Material_Change(Material material)
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material = material;
        //GetComponentInChildren<SkinnedMeshRenderer>().materials[0] = material;
    }

    //=============================================================================


    //=============================================================================
    // think함수
    //=============================================================================

    public void Think_Trace_Dash()
    {
        int index = Random.Range(1, 11);
        if (status.curHp < status.maxHp * 0.3f)
        {
            //30%
            if (index < 7) SetState((int)Enums.eSpiritState.Trace);
            else SetState((int)Enums.eSpiritState.Dash);
        }
        else
        {
            //10%
            if (index < 9) SetState((int)Enums.eSpiritState.Trace);
            else SetState((int)Enums.eSpiritState.Dash);
        }
    }

    //Dash_Test용
    public void Think_Trace_Dash(bool value)
    {
        if(value) SetState((int)Enums.eSpiritState.Dash);
    }
    //=============================================================================


    //=============================================================================
    //instance
    //=============================================================================

    public void CreateRemainderWeapon(Transform trans)
    {
        if (!existRemainder)
        {
            existRemainder = true;
            GameObject obj = Instantiate(remainderWeapon);
            obj.transform.position = trans.position;
            obj.transform.rotation = trans.rotation;
        }
        else return;
    }

    //=============================================================================
    //애니메이션 이벤트

    public IEnumerator Spirit_Step()
    {
        yield return new WaitForSeconds(0.1f);
        MoveStop();
    }

    public void Spirit_StepWait() { if (!stepWait) stepWait = true; }
    public void Spirit_StepStart() { if (stepWait) stepWait = false; }
    public void Spirit_Melee_CompleteEquiptment() { complete_Equipt = true; }
    public void Spirit_Melee_CompleteUnequiptment() { complete_Unequipt = true; }
    public void Spirit_Melee_CompleAtk() { complete_Atk = true; }
    //public void Spirit_AttReturn() { if (!complete_AttReturn) complete_AttReturn = true; }
    //public void Spirit_Melee_CompleCombo1() { complete_Combo1 = true; }
    public void Spirit_Damaged() { complete_Damaged = true; }
    public void Spirit_Atting()
    {
        if(curState_e == Enums.eSpiritState.Atk)
        {
            if (!atting) atting = true;
            else atting = false;
        }
    }
    public void Spirit_WeaponTransPos()
    {
        if (curState_e == Enums.eSpiritState.Atk)
        {
            if (!transWeaponPos) transWeaponPos = true;
            else transWeaponPos = false;
        }
    }


    //=============================================================================

}



