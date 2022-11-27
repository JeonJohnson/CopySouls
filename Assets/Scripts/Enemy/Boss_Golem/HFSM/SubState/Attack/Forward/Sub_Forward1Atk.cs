using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Forward1Atk : Golem_SubState
{
	public Sub_Forward1Atk(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 2;
		atkRangeType = eGolemAtkRangeType.MiddleAtk;
	}

	public override void EnterState()
	{
		base.EnterState();
		golem.Golem_TrailOnOff(true);

		golem.animCtrl.applyRootMotion = true;

		golem.status.curStamina -= stateCost;
		int rand = Random.Range(1, 4);
		animName = $"ForAtk_{rand}";

		golem.animCtrl.SetTrigger("tForAtk1");
		golem.animCtrl.SetInteger("iForAtk1", rand);

		if (rand == 1)
		{ table.SetAtkType((int)Enums.eAttackType.Week); }
		else
		{ table.SetAtkType((int)Enums.eAttackType.Strong); }

		
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
		golem.Golem_TrailOnOff(true);
	}
}