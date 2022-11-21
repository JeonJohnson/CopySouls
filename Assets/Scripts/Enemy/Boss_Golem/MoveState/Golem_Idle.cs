using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Idle : cGolemState
{
	public Golem_Idle(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.None;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		golem.animCtrl.SetTrigger("tIdle");
	}

	public override void UpdateState()
	{
		//table.FillStamina();

		if (golem.distToTarget > golem.status.atkRange)
		{
			golem.SetState((int)eGolemState.Walk);
		}
		else if (golem.angleToTarget >= 45f)
		{
			golem.SetState((int)eGolemState.Turn);
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