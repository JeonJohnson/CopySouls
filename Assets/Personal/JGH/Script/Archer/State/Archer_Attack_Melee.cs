using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Attack_Melee : cState
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
	
	}
	public override void ExitState()
	{
	}
}
