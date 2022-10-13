using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_LookAround: cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }


		me.ResetAllAnimTrigger(Defines.ArcherAnimTriggerStr);

		me.animCtrl.SetTrigger("tLookAround");
	}
	public override void UpdateState()
	{
		if (archer.CheckTargetInFovAndRange())
		{
			//me.animCtrl.SetTrigger("tAttack");
			if (me.distToTarget <= me.status.atkRange)
			{ archer.RandomAttack(); }
			else 
			{
				me.SetState((int)Enums.eArcherState.Walk_Careful);
			}
		}

		if (Funcs.IsAnimationCompletelyFinish(me.animCtrl,"Archer_LookAround"))
		{
			//archer.UnequippedBow();
			me.SetState((int)Enums.eArcherState.Bow_Unequip);
		}
	}

	public override void ExitState()
	{
		me.isAlert = false;
	}

}
