using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;
public class Archer_Idle : cState
{

	Archer archer = null;

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }


		me.ResetAllAnimTrigger(Defines.ArcherAnimTriggerStr);

		me.animCtrl.SetTrigger("tIdle");
	}

	public override void UpdateState()
	{
		

		if (archer.CheckTargetInFov() == true)
		{
			archer.SetState((int)eArcherState.Bow_Equip);
		}


	}

	public override void LateUpdateState()
	{


	}

	public override void ExitState()
	{
	}


}
