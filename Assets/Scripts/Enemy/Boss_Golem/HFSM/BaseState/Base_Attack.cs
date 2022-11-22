using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eGolemAttackState
{
	Think,
	Melee,
	Forward,
	Range,
	End
}

public class Base_Attack : Golem_BaseState
{
	public Base_Attack(HFSMCtrl script, string name) : base(script, name)
	{
	}
	public override void InitBaseState()
	{
		base.InitBaseState();

		subStates = new Golem_SubState[(int)eGolemAttackState.End];
		subStates[(int)eGolemAttackState.Think] = new Sub_Think(this,"Think");
		subStates[(int)eGolemAttackState.Melee] = new Sub_MeleeAtk(this, "Melee");
		subStates[(int)eGolemAttackState.Forward] = new Sub_ForwardAtk(this, "Forward");
		subStates[(int)eGolemAttackState.Range] = new Sub_RangeAtk(this, "Range");

		referSubState = subStates[(int)eGolemAttackState.Think];
		nextSubState = subStates[(int)eGolemAttackState.Think];
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
