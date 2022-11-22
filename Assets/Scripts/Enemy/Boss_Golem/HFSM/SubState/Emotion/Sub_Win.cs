using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Win : Golem_SubState
{
	public Sub_Win(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 0;
		//atkRangeType = eGolemAtkRangeType.None;
	}

	public override void EnterState()
	{
		base.EnterState();
	}
	public override void UpdateState()
	{
		base.UpdateState();
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
	}

}
