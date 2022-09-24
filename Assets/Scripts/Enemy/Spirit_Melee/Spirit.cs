using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Spirit : Enemy
{
	public override void InitializeState()
	{
		this.rd = GetComponent<Rigidbody>();
		this.animCtrl = GetComponent<Animator>();
		this.navAgent = GetComponent<NavMeshAgent>();

		//limitPatrolRange = new Vector3(transform.position.x + status.patrolRange, transform.position.y + status.patrolRange);

		fsm[(int)Enums.eEnmeyState.Idle] = new Spirit_Idle();
		fsm[(int)Enums.eEnmeyState.Patrol] = new Spirit_Patrol();
		fsm[(int)Enums.eEnmeyState.Atk] = new Spirit_Atk();

		SetState(Enums.eEnmeyState.Idle);
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
        if (/*((StateCtrl_Spirit)fsmCtrl).*/curState_e == Enums.eEnmeyState.Patrol) navAgent.speed = status.moveSpd;
        else if (/*((StateCtrl_Spirit)fsmCtrl).*/curState_e == Enums.eEnmeyState.Atk) navAgent.speed = status.runSpd;
        //추격 대상이 있으면 추격
        if (curTargetPos != null && navAgent.isStopped == false)
        {
            navAgent.SetDestination(curTargetPos);
        }
        
        //GameObject obj = UnitManager.Instance.playerObj;
    }

}
