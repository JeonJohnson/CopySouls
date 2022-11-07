using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Attack_Rapid : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		me.isCombat = true;

		me.animCtrl.SetTrigger("tAttack");
	}
	public override void UpdateState()
	{

	}

	public override void ExitState()
	{
	}

}
