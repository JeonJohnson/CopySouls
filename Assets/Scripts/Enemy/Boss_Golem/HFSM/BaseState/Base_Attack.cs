using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eGolemAttackState
{
	Think,

	Melee1Hit,
	Melee2Hit,
	Melee3Hit,

	Forward1Hit,
	Forward2Hit,
	Forward3Hit,

	RockThrow,

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

		subStates[(int)eGolemAttackState.Melee1Hit] = new Sub_Melee1Atk(this, "Melee1");
		subStates[(int)eGolemAttackState.Melee2Hit] = new Sub_Melee2Atk(this, "Melee2");
		subStates[(int)eGolemAttackState.Melee3Hit] = new Sub_Melee3Atk(this, "Melee3");

		subStates[(int)eGolemAttackState.Forward1Hit] = new Sub_Forward1Atk(this, "Forward1");
		subStates[(int)eGolemAttackState.Forward2Hit] = new Sub_Forward2Atk(this, "Forward2");
		subStates[(int)eGolemAttackState.Forward3Hit] = new Sub_Forward3Atk(this, "Forward3");

		subStates[(int)eGolemAttackState.RockThrow] = new Sub_RockThrowAtk(this, "ThrowRock");

		referSubState = subStates[(int)eGolemAttackState.Think];
		//nextSubState = subStates[(int)eGolemAttackState.Think];
	}

	public override void EnterBaseState()
	{
		base.EnterBaseState();

		golem.animCtrl.ResetTrigger("tIdle");
	}

	public override void UpdateBaseState()
	{


		base.UpdateBaseState();
		//if (golem.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		//{
		//	hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Move));
		//}

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
