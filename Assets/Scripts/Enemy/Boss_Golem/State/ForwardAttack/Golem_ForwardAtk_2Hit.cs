using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_ForwardAtk_2Hit : cGolemState
{
	public Golem_ForwardAtk_2Hit(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.MiddleAtk;
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
