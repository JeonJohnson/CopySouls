using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Turn : Golem_SubState
{
	public Sub_Turn(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 0;
	}


	public override void EnterState()
	{
		base.EnterState();

		golem.navAgent.updateRotation = false;

		//golem.animCtrl.applyRootMotion = true;

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
		base.UpdateState();

		table.LookAtBody(2f);

		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, animName, 1f))
		{
			if (golem.distToTarget > golem.status.atkRange + 1f)
			{
				baseState.SetSubState(baseState.GetSubState((int)eGolemMoveState.Move));
			}
			else if (golem.angleToTarget >= 45f)
			{
				baseState.SetSubState(baseState.GetSubState((int)eGolemMoveState.Turn));
			}
			else
			{	
				baseState.SetSubState(baseState.GetSubState((int)eGolemMoveState.Idle));
			}
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
		golem.animCtrl.ResetTrigger("tRotate");
		golem.animCtrl.SetInteger("iRotDir", 0);
		//golem.animCtrl.applyRootMotion = false;
		golem.navAgent.updateRotation = true;
	}
}
