using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Runaway : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		me.animCtrl.SetTrigger("tRun");
	}
	public override void UpdateState()
	{
		if (!archer.CheckTargetInFov())
		{
			me.SetState((int)Enums.eArcherState.LookAround);
		}
	}

	public override void ExitState()
	{
	}

}
