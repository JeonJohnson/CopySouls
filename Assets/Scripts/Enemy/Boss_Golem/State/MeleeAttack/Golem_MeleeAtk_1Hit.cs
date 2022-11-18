using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_MeleeAtk_1Hit : cState
{
	Golem golem = null;
	Golem_ActionTable table = null;
	int stateCost;

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
	}
}
