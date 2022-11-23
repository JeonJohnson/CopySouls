using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Idle : Golem_SubState
{
	public Sub_Idle(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 0;
	}

	public override void EnterState()
	{
		base.EnterState();


		golem.animCtrl.SetTrigger("tIdle");
	}

	public override void UpdateState()
	{
		base.UpdateState();

		if (golem.distToTarget > golem.status.atkRange + 1f)
		{
			baseState.SetSubState(baseState.GetSubState((int)eGolemMoveState.Move));
		}
		else if (golem.angleToTarget >= 45f)
		{
			baseState.SetSubState(baseState.GetSubState((int)eGolemMoveState.Turn));
		}
	}

	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
	}

	public override void ExitState()
	{
		base.ExitState();
	}
}
