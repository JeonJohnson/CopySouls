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
	}
	public override void EnterBaseState()
	{
		base.EnterBaseState();
	}

	public override void UpdateBaseState()
	{
		base.UpdateBaseState();
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
