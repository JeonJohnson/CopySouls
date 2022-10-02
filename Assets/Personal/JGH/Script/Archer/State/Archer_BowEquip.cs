using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_BowEquip : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		me.animCtrl.SetTrigger("tEquip");
		me.weapon.SetActive(true);
		archer.isEquip = true;
		
	}

	public override void UpdateState()
	{
		if (Funcs.IsAnimationAlmostFinish(me.animCtrl, "Archer_Equip"))
		{
			me.SetState((int)Enums.eArcherState.Idle);
		}
	}

	public override void ExitState()
	{
	}

}
