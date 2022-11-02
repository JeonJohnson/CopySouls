using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;

public class Spirit : Enemy
{
    public int preHp;
    public GameObject remainderWeapon;
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

        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    status.curHp--;
        //}

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
    public void Spirit_AttReturn() { if (!complete_AttReturn) complete_AttReturn = true; }
    public void Spirit_Damaged() {
        complete_Damaged = true;
        Debug.Log("ok");
    }
    public void Spirit_Atting()
    {
        if(curState_e == Enums.eSpiritState.Atk)
        {
            if (!atting) atting = true;
            else atting = false;
        }
    }

    //=============================================================================

}



