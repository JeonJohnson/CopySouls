using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Melee1Atk : Golem_SubState
{
	public Sub_Melee1Atk(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 1;
		atkRangeType = eGolemAtkRangeType.CloseAtk;
	}

	public override void EnterState()
	{
		base.EnterState();

		golem.status.curStamina -= stateCost;

		golem.animCtrl.SetTrigger("tAtk1");
		int iRand = Random.Range(1, 6);
		animName = $"Attack_{iRand}";
		golem.animCtrl.SetInteger("iAtk1_Num", iRand);

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

