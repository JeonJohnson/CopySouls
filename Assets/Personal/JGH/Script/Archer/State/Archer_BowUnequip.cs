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
		
	}
	public override void UpdateState()
	{
		if (Funcs.IsAnimationAlmostFinish(me.animCtrl, "Archer_Unequip"))
		{
			if (archer.isEquip)
			{
				archer.isEquip = false;
			}

			me.SetState((int)archer.defaultPattern);
		}
	}

	public override void ExitState()
	{
	}

}
