using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Melee3Atk : Golem_SubState
{
	public Sub_Melee3Atk(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 3;
		atkRangeType = eGolemAtkRangeType.CloseAtk;
	}

	public override void EnterState()
	{
		base.EnterState();
		golem.Golem_TrailOnOff(true);

		golem.status.curStamina -= stateCost;
		golem.animCtrl.SetTrigger("tAtk3");

		table.SetAtkType((int)Enums.eAttackType.Week);
		//3타는 애니메이션에서 AtkType 정해줌.
	}
	public override void UpdateState()
	{
		base.UpdateState();
		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, "3Attack"))
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

