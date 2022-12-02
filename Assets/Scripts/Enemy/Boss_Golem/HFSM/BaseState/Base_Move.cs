using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGolemMoveState
{
	Idle,
	Move,
	Turn,
	End
}

public class Base_Move : Golem_BaseState
{
	

	public Base_Move(HFSMCtrl script, string name) : base(script, name)
	{

	}

	public override void InitBaseState()
	{
		base.InitBaseState();

		subStates = new Golem_SubState[(int)eGolemMoveState.End];
		subStates[(int)eGolemMoveState.Idle] = new Sub_Idle(this, "Idle");
		subStates[(int)eGolemMoveState.Move] = new Sub_Move(this, "Move");
		subStates[(int)eGolemMoveState.Turn] = new Sub_Turn(this, "Turn");

		//referSubState = subStates[(int)eGolemMoveState.Idle];
		referSubState = subStates[(int)eGolemMoveState.Move];
	}
	public override void EnterBaseState()
	{
		if (golem.distToTarget <= golem.status.atkRange)
		{
			if (golem.angleToTarget >= 45f)
			{
				nextSubState = GetSubState((int)eGolemMoveState.Turn);
			}
			else
			{
				nextSubState = GetSubState((int)eGolemMoveState.Idle);
			}
		}
		else
		{
			nextSubState = GetSubState((int)eGolemMoveState.Move);
		}

		base.EnterBaseState();

		if (hfsmCtrl.baseStates[(int)eGolemBaseState.Attack].nextSubState != null)
		{

		}
		else
		{
			hfsmCtrl.thinkTime = Random.Range(hfsmCtrl.thinkMinTime, hfsmCtrl.thinkMaxTime);
		}
		
	}

	public override void UpdateBaseState()
	{
		base.UpdateBaseState();

		if (hfsmCtrl.baseStates[(int)eGolemBaseState.Attack].nextSubState != null)
		{//Attack 예약 된게 있으니까 해당 거리 조건 맞으면 바로 가기!
			
			eGolemAtkRangeType tempType = hfsmCtrl.baseStates[(int)eGolemBaseState.Attack].nextSubState.atkRangeType;

			switch (tempType)
			{
				case eGolemAtkRangeType.CloseAtk:
					{
						if (golem.distToTarget <= golem.status.atkRange)
						{
							hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Attack));
						}
					}
					break;
				case eGolemAtkRangeType.MiddleAtk:
					{
						if (golem.distToTarget <= golem.rangeAtkRange)
						{
							hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Attack));
						}
					}
					break;
				default:
					break;
			}

		}
		else
		{//Attack에 예약된거 없으면 시간 ㄱㄱㄱ
			hfsmCtrl.curThinkTime+= Time.deltaTime;
			if (hfsmCtrl.curThinkTime >= hfsmCtrl.thinkTime)
			{
				hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Attack));
				hfsmCtrl.curThinkTime = 0f;
			}
		}
	}

	public override void FixedUpdateBaseState()
	{
		base.FixedUpdateBaseState();
	}

	public override void LateUpdateBaseState()
	{
		base.LateUpdateBaseState();
	}

	public override void ExitBaseState()
	{
		base.ExitBaseState();
	}
}
