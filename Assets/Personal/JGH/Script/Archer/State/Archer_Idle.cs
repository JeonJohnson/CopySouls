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

		if (archer.isEquip)
		{
			me.animCtrl.SetTrigger("tIdle"); 
		}
		else 
		{ 
			me.animCtrl.SetTrigger("tIdle_Unequip"); 
		}

		me.isAlert = false;
		
	}

	public override void UpdateState()
	{
		//archer.EquipWeapon();

		me.CheckTargetInFovAndRange();
	}

	public override void LateUpdateState()
	{


	}

	public override void ExitState()
	{
	}


}
