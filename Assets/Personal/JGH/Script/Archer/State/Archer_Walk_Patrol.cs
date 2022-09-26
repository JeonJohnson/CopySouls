using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Walk_Patrol : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		me.animCtrl.SetTrigger("tWalk");
	}

	public override void UpdateState()
	{
	}

	public override void ExitState()
	{
	}

}
