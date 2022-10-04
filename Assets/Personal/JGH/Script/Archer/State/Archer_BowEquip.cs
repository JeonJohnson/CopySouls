using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

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

		me.transform.rotation = me.LookAtSlow(me.transform, me.targetObj.transform, me.status.lookAtSpd);


		if (Funcs.IsAnimationAlmostFinish(me.animCtrl, "Archer_Equip", 0.7f))
		{
			if (me.isAlert)
			{
				if (me.distToTarget > me.status.atkRange)
				{
					me.SetState((int)eArcherState.Walk_Careful);
				}
			}
			else
			{
				me.SetState((int)Enums.eArcherState.Idle);
			}
			

		}
	}

	public override void ExitState()
	{
	}

}
