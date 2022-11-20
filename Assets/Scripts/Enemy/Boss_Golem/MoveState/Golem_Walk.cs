using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Walk : cGolemState
{
	public Golem_Walk(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.None;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		//if (!golem.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
		//{
			golem.animCtrl.SetTrigger("tMove");
		//}
		//golem.navAgent.SetDestination(golem.targetObj.transform.position);
	}

	public override void UpdateState()
	{
		//table.FillStamina();

		golem.navAgent.SetDestination(golem.targetObj.transform.position);

		if (golem.distToTarget <= golem.status.atkRange)
		{
			if (golem.angleToTarget >= 45f)
			{ golem.SetState((int)eGolemState.Turn); }
			else { golem.SetState((int)eGolemState.Idle); }
		}
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
	}
	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void ExitState()
	{
	}
}