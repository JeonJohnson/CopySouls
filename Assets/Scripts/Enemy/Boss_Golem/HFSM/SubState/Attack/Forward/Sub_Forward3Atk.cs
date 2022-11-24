using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Forward3Atk : Golem_SubState
{
	public Sub_Forward3Atk(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 4;
		atkRangeType = eGolemAtkRangeType.MiddleAtk;
	}

	public override void EnterState()
	{
		base.EnterState();


		golem.animCtrl.applyRootMotion = true;

		golem.status.curStamina -= stateCost;

		golem.animCtrl.SetTrigger("tForAtk3");
		animName = "ForAtk3";

		table.SetAtkType((int)Enums.eAttackType.Strong);
	}
	public override void UpdateState()
	{
		base.UpdateState();

		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, animName))
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
	}
}