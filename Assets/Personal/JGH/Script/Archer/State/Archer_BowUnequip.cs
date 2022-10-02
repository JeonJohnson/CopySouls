using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_BowUnequip : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }
		
		me.animCtrl.SetTrigger("tUnequip");

		me.weapon.SetActive(false);
		archer.isEquip = false;
	}
	public override void UpdateState()
	{
		if (Funcs.IsAnimationAlmostFinish(me.animCtrl, "Archer_Unequip"))
		{
			me.SetState((int)Enums.eArcherState.Idle);
		}

	}

	public override void ExitState()
	{
	}

}
