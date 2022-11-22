using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eGolemDamagedState
{
	Hit,
	Death,
	End
}
public class Base_Damaged : Golem_BaseState
{
	public Base_Damaged(HFSMCtrl script, string name) : base(script, name)
	{
	}
	public override void InitBaseState()
	{
		base.InitBaseState();

		subStates = new Golem_SubState[(int)eGolemDamagedState.End];

		subStates[(int)eGolemDamagedState.Hit] = new Sub_Hit(this, "Hit");
		subStates[(int)eGolemDamagedState.Death] = new Sub_Hit(this, "Death");
		

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
