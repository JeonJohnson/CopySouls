using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Idle : cGolemState
{
	public Golem_Idle(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.None;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);


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
