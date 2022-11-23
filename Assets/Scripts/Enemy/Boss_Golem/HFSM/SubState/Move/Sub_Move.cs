using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Move : Golem_SubState
{
	public Sub_Move(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 0;
	}

	public float CalcSpd()
	{ 
		float ratio = (golem.distToTarget - golem.status.atkRange) / (golem.rangeAtkRange - golem.status.atkRange);
		return Mathf.Lerp(golem.status.moveSpd, golem.status.runSpd, ratio);
	}

	public override void EnterState()
	{
		base.EnterState();

		golem.animCtrl.applyRootMotion = false;
		golem.navAgent.isStopped = false;

		golem.animCtrl.SetTrigger("tMove");
	}

	public override void UpdateState()
	{
		base.UpdateState();

		golem.navAgent.SetDestination(golem.targetObj.transform.position);
		float curSpd = CalcSpd();
		golem.navAgent.speed = curSpd;
		golem.animCtrl.SetFloat("fMoveSpd", curSpd);

		if (golem.distToTarget <= golem.status.atkRange)
		{
			if (golem.angleToTarget >= 45f)
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

		golem.navAgent.isStopped = true;
	}
}
