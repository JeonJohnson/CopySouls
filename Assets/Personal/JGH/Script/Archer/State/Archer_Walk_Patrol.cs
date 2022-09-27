using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Archer_Walk_Patrol : cState
{
	Archer archer = null;

	public int destIndex = 0;
	//public int nextDestIndex;
	//public Vector3 curDest;
	//public Vector3 nextDest;

	public void CalcNearDestIndex()
	{
		//전투나 다른 위치에 갔따가 다시 패트롤로 들어올 때.

		Vector3 curPos = me.transform.position;

		//List<int> distance = new List<int>();

		var distances = from dest in me.patrolPosList.ToArray()
				   select Vector3.Distance(dest, curPos);

		int minDistIndex = Array.IndexOf(distances.ToArray(), distances.ToArray().Min());

		//nextDestIndex = minDistIndex;
		destIndex = minDistIndex;

		me.MoveOrder(me.patrolPosList[destIndex]);
	}

	public void NextDestCheck()
	{
		float dist = Vector3.Distance(me.transform.position, me.patrolPosList[destIndex]);

		if (dist <= 0.5f)
		{
			++destIndex;

			if (destIndex >= me.patrolPosList.Count)
			{
				destIndex = 0;
			}

			me.MoveOrder(me.patrolPosList[destIndex]);
		}
	}

	

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		me.navAgent.speed = me.status.moveSpd;

		me.animCtrl.SetTrigger("tWalk");
	}

	public override void UpdateState()
	{
		NextDestCheck();
	}

	public override void ExitState()
	{

	}

}
