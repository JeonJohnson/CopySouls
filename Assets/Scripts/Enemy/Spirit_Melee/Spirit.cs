using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Spirit : Enemy
{
	public override void InitializeState()
	{
        fsm = new cState[(int)Enums.eSpiritState.End];

        //limitPatrolRange = new Vector3(transform.position.x + status.patrolRange, transform.position.y + status.patrolRange);

        fsm[(int)Enums.eSpiritState.Idle] = new Spirit_Idle();
        fsm[(int)Enums.eSpiritState.Patrol] = new Spirit_Patrol();
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

        distToPlayer = Vector3.Distance(player.transform.position,transform.position);
        if (GetCurState<Enums.eSpiritState>() == Enums.eSpiritState.Patrol) navAgent.speed = status.moveSpd;
        else if (GetCurState<Enums.eSpiritState>() == Enums.eSpiritState.Atk) navAgent.speed = status.runSpd;
        if (curTargetPos != null) navAgent.SetDestination(curTargetPos);
        
        //GameObject obj = UnitManager.Instance.playerObj;
    }

}
