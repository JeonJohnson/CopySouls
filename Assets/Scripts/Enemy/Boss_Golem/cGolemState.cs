using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cGolemState : cState
{
	public cGolemState(int cost)
	{
		stateCost = cost;
	}

	public Golem golem = null;
	public Golem_ActionTable table = null;
	public int stateCost;
	public eGolemStateAtkType atkType;

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);
		if (!golem)
		{
			golem = script as Golem;
			table = golem.actTable;
		}

	}

	public override void UpdateState()
	{
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
		golem.decisionTime = Random.Range(1f, 2f);
	}
}
