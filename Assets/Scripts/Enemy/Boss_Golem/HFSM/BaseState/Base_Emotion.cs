using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGolemEmotionState
{
	Entrance,
	Win,
	End,
}

public class Base_Emotion : Golem_BaseState
{
	public Base_Emotion(HFSMCtrl script, string name) : base(script, name)
	{

	}

	public override void EnterBaseState()
	{
		base.EnterBaseState();
	}

	public override void InitBaseState()
	{
		base.InitBaseState();

		subStates = new Golem_SubState[(int)eGolemEmotionState.End];
		subStates[(int)eGolemEmotionState.Entrance] = new Sub_Entrance(this, "Entrance");
		subStates[(int)eGolemEmotionState.Win] = new Sub_Win(this, "Win");

		nextSubState = subStates[(int)eGolemEmotionState.Entrance];
	}

	public override void UpdateBaseState()
	{
		base.UpdateBaseState();
	}
	public override void LateUpdateBaseState()
	{
		base.LateUpdateBaseState();
	}
	public override void FixedUpdateBaseState()
	{
		base.FixedUpdateBaseState();
	}


	public override void ExitBaseState()
	{
		base.ExitBaseState();
	}

}
