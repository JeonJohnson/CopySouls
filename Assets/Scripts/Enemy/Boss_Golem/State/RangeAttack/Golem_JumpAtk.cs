using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_JumpAtk : cGolemState
{
	public Golem_JumpAtk(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.RangeAtk;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);
	}

	public override void UpdateState()
	{
		base.UpdateState();
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
		base.ExitState();
	}
}
