using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_TurnAtk : cGolemState
{
	public Golem_TurnAtk(int cost) : base(cost)
	{
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
