using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_TurnAtk : cGolemState
{
	string animName;
	public Golem_TurnAtk(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.CloseAtk;
	}
	public void Attack()
	{
		golem.status.curStamina -= stateCost;

		
		switch (golem.targetWhichSide)
		{
			case eSideDirection.Left:
				{
					animName = "Attack_Left";
					golem.animCtrl.SetTrigger("tAtk1");
					golem.animCtrl.SetInteger("iRotDir", -1);
				}
				break;
			case eSideDirection.Right:
				{
					animName = "Attack_Right";
					golem.animCtrl.SetTrigger("tAtk1");
					golem.animCtrl.SetInteger("iRotDir", 1);
				}
				break;
			default:
				break;
		}

	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		Attack();
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
