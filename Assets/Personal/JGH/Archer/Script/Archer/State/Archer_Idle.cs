using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Idle : cState
{

	Archer archer = null;

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }


		me.ResetAllAnimTrigger(Defines.ArcherAnimTriggerStr);

	}

	public override void UpdateState()
	{


	}

	public override void LateUpdateState()
	{


	}

	public override void ExitState()
	{
	}


}
