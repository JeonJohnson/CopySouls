using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_RockThrowAtk : Golem_SubState
{
	public Sub_RockThrowAtk(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 6;
		atkRangeType = eGolemAtkRangeType.RangeAtk;
	}

	public override void EnterState()
	{
		base.EnterState();

		golem.Golem_TrailOnOff(true);

		golem.status.curStamina -= stateCost;

		golem.animCtrl.SetTrigger("tThrow");
	}
	public override void UpdateState()
	{
		base.UpdateState();

		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, "Throw"))
		{
			hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Move));
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
		golem.Golem_TrailOnOff(false);
	}
}

