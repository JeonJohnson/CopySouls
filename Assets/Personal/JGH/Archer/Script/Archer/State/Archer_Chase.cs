using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Chase : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);
		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }
	}



	public override void UpdateState()
	{
		throw new System.NotImplementedException();
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
		throw new System.NotImplementedException();
	}
}
