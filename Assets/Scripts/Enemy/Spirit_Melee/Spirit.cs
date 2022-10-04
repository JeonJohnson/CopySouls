using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;



public class Spirit : Enemy
{
    public eSpiritState curState_e;
    public float WaitTimer;     //test
    public float PatrolTimer;   //test
    public bool Arrival;        //test
    public bool complete_Equipt;
    public bool complete_Unequipt;
    public bool complete_Atk;

    public override void InitializeState()
	{
        fsm = new cState[(int)Enums.eSpiritState.End];

        //limitPatrolRange = new Vector3(transform.position.x + status.patrolRange, transform.position.y + status.patrolRange);

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

        this.rd = GetComponent<Rigidbody>();
        this.animCtrl = GetComponent<Animator>();
        this.navAgent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        //if(Input.GetKey("a"))
        //{
        //    status.curHp--;
        //}

        curState_e = GetCurState<Enums.eSpiritState>();
        //221002 20:23 player -> targetObj
        distToTarget = Vector3.Distance(targetObj.transform.position, transform.position);

        if (!navAgent.isStopped) navAgent.SetDestination(curTargetPos);
    }

    //애니메이션 이벤트==================================

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
}
