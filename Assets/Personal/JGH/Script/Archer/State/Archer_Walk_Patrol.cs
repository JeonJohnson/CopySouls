using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Archer_Walk_Patrol : cState
{
	Archer archer = null;


	public bool isRevert = false;
	public int destIndex = 0;

	public Vector3 prePos = new Vector3(float.MinValue, float.MinValue, float.MinValue);

	public Vector3 curDest;
	//public int nextDestIndex;
	//public Vector3 curDest;
	//public Vector3 nextDest;

	public void SetStartDest()
	{
		//전투나 다른 위치에 갔따가 다시 패트롤로 들어올 때.

		//Vector3 curPos = me.transform.position;

		//var distances = from dest in me.patrolPosList.ToArray()
		//		   select Vector3.Distance(dest, curPos);

		//int minDistIndex = Array.IndexOf(distances.ToArray(), distances.ToArray().Min());

		//destIndex = minDistIndex;

		if (isRevert)
		{
			curDest = prePos;
		}
		else 
		{
			curDest = me.patrolPosList[destIndex];
		}

		me.MoveOrder(curDest);
	}

	public void CheckNextDest(Vector3 destPos)
	{
		float dist = Vector3.Distance(me.transform.position, destPos);

		if (dist <= 1.0f)
		{
			if (isRevert)
			{
				isRevert = false;
			}
			else
			{
				++destIndex;

				if (destIndex >= me.patrolPosList.Count)
				{
					destIndex = 0;
				}

				curDest = me.patrolPosList[destIndex];
			}
			me.MoveOrder(curDest);
		}
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }


		me.navAgent.speed = me.status.moveSpd;
		//CalcNearDestIndex();
		SetStartDest();

		me.animCtrl.SetTrigger("tWalk");

	}

	public override void UpdateState()
	{
		CheckNextDest(curDest);

		//archer.EquipWeapon();
		me.CheckTargetInFov();

	}

	public override void ExitState()
	{
		prePos = me.transform.position;
		//preDestIndex = destIndex;

		me.MoveStop();
	}

}
