using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Turn: cGolemState
{
	public string animName;
	public Golem_Turn(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.None;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		golem.animCtrl.applyRootMotion = true;
		

		switch (golem.targetWhichSide)
		{
			case eSideDirection.Left:
				{
					animName = "Turn_Left";
					golem.animCtrl.SetTrigger("tRotate");
					golem.animCtrl.SetInteger("iRotDir", -1);
				}
				break;
			case eSideDirection.Right:
				{
					animName = "Turn_Right";
					golem.animCtrl.SetTrigger("tRotate");
					golem.animCtrl.SetInteger("iRotDir", 1);
				}
				break;
			default:
				break;
		}
	}

	public override void UpdateState()
	{
		//table.FillStamina();

		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, animName))
		{
			if (golem.distToTarget > golem.status.atkRange)
			{
				golem.SetState((int)eGolemState.Walk);
			}
			else
			{
				golem.SetState((int)eGolemState.Idle);
			}
			
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
		golem.animCtrl.SetInteger("tRotDir", 0);
		golem.animCtrl.applyRootMotion = false;
	}
}

