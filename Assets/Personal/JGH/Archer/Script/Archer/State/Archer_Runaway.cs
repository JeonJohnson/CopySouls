using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Runaway : cState
{
	Archer archer = null;

	Vector3 runStartPos;
	Vector3 runDestPos;


	public IEnumerator ArriveToDest()
	{//목적지 도착하고나서 할 것덜
	 //1. 아이들로 돌아가기
	 //2. 원래 있던 곳 바라보기
		while (true)
		{
			archer.transform.rotation = archer.LookAtSlow(me.transform, runStartPos, me.status.lookAtSpd);
			
			int dot = archer.ActingLegWhileTurn(runStartPos);

			if (Input.GetKeyDown(KeyCode.Space)
				|| (dot == 0))
			{
				break;
			}

			yield return null;
		}

		
	}


	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		runStartPos = me.transform.position;
		runDestPos = me.transform.position + (me.transform.forward * me.status.atkRange);

		me.navAgent.speed = me.status.runSpd;
		me.MoveOrder(runDestPos);
		me.animCtrl.SetTrigger("tRun");
	}
	public override void UpdateState()
	{
		float dist = Vector3.Distance(me.transform.position, runDestPos);

		if (dist < 1f)
		{
			//애니메이션 재생 멈추고 돌아본 뒤 공격상태로
			me.MoveStop();
			me.SetState((int)Enums.eArcherState.Idle);
			CoroutineHelper.Instance.StartCoroutine(ArriveToDest());
			//me.animCtrl.SetTrigger("tIlde");
			//archer.transform.rotation = archer.LookAtSlow(me.transform, me.targetObj.transform, me.status.lookAtSpd);
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
