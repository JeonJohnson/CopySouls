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


		me.animCtrl.SetTrigger("tLookAround");
	}
	public override void UpdateState()
	{
		if (archer.CheckTargetInFov())
		{
			//me.animCtrl.SetTrigger("tAttack");
			archer.RandomAttack();
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
