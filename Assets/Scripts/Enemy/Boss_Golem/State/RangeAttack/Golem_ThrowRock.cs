using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_ThrowRock : cGolemState
{
	public Golem_ThrowRock(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.RangeAtk;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		golem.status.curStamina -= stateCost;

		golem.animCtrl.SetTrigger("tThrow");
	}

	public override void UpdateState()
	{
		base.UpdateState();

		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, "Throw"))
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
		base.ExitState();
	}
}
