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



		

		if (archer.isEquip)
		{
			me.animCtrl.SetTrigger("tIdle"); 
		}
		else 
		{ 
			me.animCtrl.SetTrigger("tIdle_Unequip"); 
		}

		
	}

	public override void UpdateState()
	{
		//archer.EquipWeapon();
	}

	public override void ExitState()
	{
	}


}
