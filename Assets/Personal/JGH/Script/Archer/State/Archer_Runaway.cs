using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Runaway : cState
{
	Archer archer = null;

	Vector3 runDestPos;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		runDestPos = me.targetObj.transform.position + (me.targetObj.transform.forward * me.status.atkRange);

		me.navAgent.speed = me.status.runSpd;
		me.MoveOrder(runDestPos);
		me.animCtrl.SetTrigger("tRun");
	}
	public override void UpdateState()
	{

		float dist = Vector3.Distance(me.transform.position, runDestPos);

		if (dist < 2f)
		{
			//애니메이션 재생 멈추고 돌아본 뒤 공격상태로
			
			me.MoveStop();
			archer.transform.rotation = archer.LookAtSlow(me.transform, me.targetObj.transform, me.status.lookAtSpd);
		}
			//if (archer.ActingLegWhileTurn(me.targetObj) <= 0.1f
			//	|| archer.ActingLegWhileTurn(me.targetObj) >= -0.1f)
			//{
			//	//me.SetState((int)Enums.eArcherState.Attack_Aiming);
			//}
			//
		

		//if (!archer.CheckTargetIsHidingInFov(me.targetObj))
		//{
		//	me.SetState((int)Enums.eArcherState.LookAround);
		//}
	}

	public override void ExitState()
	{
		me.navAgent.speed = me.status.moveSpd;
	}

}
