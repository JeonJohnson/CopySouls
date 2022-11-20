using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_MeleeAtk_1Hit : cGolemState
{
	string animName;

	public Golem_MeleeAtk_1Hit(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.CloseAtk;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		golem.animCtrl.SetTrigger("tAtk1");
		int iRand = Random.Range(1, 6);
		animName = $"Attack_{iRand}";
		golem.animCtrl.SetInteger("iAtk1_Num", iRand);

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
	}
}
