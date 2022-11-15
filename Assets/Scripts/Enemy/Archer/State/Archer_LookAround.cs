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


		Debug.Log("Enter LookAround State");

		//me.ResetAllAnimTrigger(Defines.ArcherAnimTriggerStr);

		me.animCtrl.SetTrigger("tLookAround");
	}
	public override void UpdateState()
	{
		//if (!archer.CheckTargetIsHiding())
		//{
		//	Debug.Log("시야에 들어옴");
		//	archer.SetState((int)archer.actTable.RandomAttackState());
		//	return;
		//}

		if (archer.CheckTargetInFov())
		{
			Debug.Log("LookAround에서 시야에 들어옴");
			archer.SetState((int)archer.actTable.RandomAttackState());
			return;
		}
		else 
		{

			if (Funcs.IsAnimationCompletelyFinish(me.animCtrl, "Archer_LookAround"))
			{
				//archer.UnequippedBow();
				me.SetState((int)Enums.eArcherState.Return);
			}
		}

		//if (archer.CheckTargetInFovAndRange())
		//{
		//	//me.animCtrl.SetTrigger("tAttack");
		//	if (me.distToTarget <= me.status.atkRange)
		//	{ archer.RandomAttack(); }
		//	else 
		//	{
		//		me.SetState((int)Enums.eArcherState.Walk_Careful);
		//	}
		//}

	}

	public override void ExitState()
	{
		//me.isAlert = false;
		
	}

}
