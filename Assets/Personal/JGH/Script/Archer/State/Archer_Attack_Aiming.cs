using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Attack_Aiming : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		me.animCtrl.SetTrigger("tAttack");
	}

	public override void UpdateState()
	{

		archer.bowString.transform.position = archer.rightHand.transform.position;
		if (Funcs.IsAnimationCompletelyFinish(me.animCtrl, "Archer_Atk_Shot"))
		{
			me.SetState(0);
		}
	}
	public override void ExitState()
	{
		archer.bowString.transform.localPosition = archer.bowStringOriginPos;
	}

}
