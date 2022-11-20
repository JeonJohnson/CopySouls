using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_ForwardAtk_1Hit : cGolemState
{

	public Golem_ForwardAtk_1Hit(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.MiddleAtk;
	}
	string animName;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		golem.animCtrl.applyRootMotion = true;

		golem.status.curStamina -= stateCost;
		int rand = Random.Range(1, 4);
		animName = $"ForAtk_{rand}";

		golem.animCtrl.SetTrigger("tForAtk1");
		golem.animCtrl.SetInteger("iForAtk1", rand);
	}

	public override void UpdateState()
	{
		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, animName))
		{
			golem.SetState((int)eGolemState.Think);
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
		golem.animCtrl.applyRootMotion = false;
	}
}
