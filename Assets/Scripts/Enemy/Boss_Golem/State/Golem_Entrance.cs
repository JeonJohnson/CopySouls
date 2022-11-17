using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Entrance : cState
{
	Golem golem = null;
	int stateCost;

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (!golem)
		{
			golem = script as Golem;
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
