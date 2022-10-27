using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;

public class Spirit : Enemy
{
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
    public bool isEquipt;
    public bool nav;

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
    }

    protected override void Update()
    {
        base.Update();
        nav = !navAgent.isStopped;

        //if(Input.GetKey("a"))
        //{
        //    status.curHp--;
        //}

        curState_e = GetCurState<Enums.eSpiritState>();

        if (isAlert)
        {
            if (curState_e != Enums.eSpiritState.Idle || curState_e != Enums.eSpiritState.Patrol)
            {
                //GetComponent<FieldOfView>().viewAngle = 360f;
            }
        }
        else
        {
            if (curState_e == Enums.eSpiritState.Idle || curState_e == Enums.eSpiritState.Patrol)
            {
                //GetComponent<FieldOfView>().viewAngle = initFOVAngle;
            }
        }

        if (isEquipt)
        {
            //weapon.gameObject.SetActive(true);
            //GameObject.Find("Sword").SetActive(false);
        }
        else
        {
            //weapon.gameObject.SetActive(false);
            //GameObject.Find("Sword").SetActive(true);
        }
    }

    //=============================================================================
    //move
    //=============================================================================

    //=============================================================================
    //애니메이션 이벤트

    public IEnumerator Spirit_Step()
    {
        yield return new WaitForSeconds(0.1f);
        MoveStop();
    }

    public void Spirit_StepWait()
    {
        if(!stepWait) stepWait = true;
    }
    public void Spirit_StepStart()
    {
        if (stepWait) stepWait = false;
    }



    public void Spirit_Melee_CompleteEquiptment()
    {
        complete_Equipt = true;
    }
    public void Spirit_Melee_CompleteUnequiptment()
    {
        complete_Unequipt = true;
    }
    public void Spirit_Melee_CompleAtk()
    {
        complete_Atk = true;
    }
    public void Spirit_AttReturn()
    {
        if(!complete_AttReturn) complete_AttReturn = true;
    }

    //=============================================================================

}



