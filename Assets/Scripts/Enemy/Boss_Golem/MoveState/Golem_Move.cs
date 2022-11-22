using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Move : cGolemState
{
	//public delegate void ConditionFunc();

	public float curSpd;

	public Golem_Move(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.None;
	}

	public void CalcSpd()
	{
		float ratio = 0f;
		ratio = (golem.distToTarget - golem.status.atkRange) / (golem.rangeAtkRange - golem.status.atkRange);
		curSpd = Mathf.Lerp(golem.status.moveSpd, golem.status.runSpd, ratio);
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		//if (!golem.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
		//{
		golem.navAgent.isStopped = false;
		golem.animCtrl.SetTrigger("tMove");
		//}
		//golem.navAgent.SetDestination(golem.targetObj.transform.position);
	}

	public override void UpdateState()
	{
		if (table.CheckNoThinkLongTime())
		{
			return;
		}

		table.CheckNextStateCondition();

		CalcSpd();

		golem.navAgent.SetDestination(golem.targetObj.transform.position);
		golem.navAgent.speed = curSpd;
		golem.animCtrl.SetFloat("fMoveSpd", curSpd);

		if (table.nextState == null)
		{
			if (golem.distToTarget <= golem.status.atkRange)
			{
				if (golem.angleToTarget >= 45f)
				{ golem.SetState((int)eGolemState.Turn); }
				else { golem.SetState((int)eGolemState.Idle); }
			}
		}
		else
		{ 
			
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
		golem.navAgent.isStopped = false;
	}
}