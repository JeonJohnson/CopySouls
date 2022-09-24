//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//using Enums;

//public class StateCtrl_Spirit : StateController
//{
//	public StateCtrl_Spirit(Enemy enemy) : base(enemy)
//	{
//		InitializeState();
//	}

//	//public eEnmeyState preState_e;
//	//public eEnmeyState curState_e;


//	public override void InitializeState()
//	{
//		fsm = new cState[(int)eEnmeyState.End];

//		fsm[(int)Enums.eEnmeyState.Idle] = new Spirit_Idle();
//		fsm[(int)Enums.eEnmeyState.Patrol] = new Spirit_Patrol();
//		fsm[(int)Enums.eEnmeyState.Atk] = new Spirit_Atk();

//		SetState(Enums.eEnmeyState.Idle);
//	}

//	//public void SetState(eEnmeyState state)
//	//{
//	//	if (state == curState_e || fsm[(int)state] == null)
//	//	{
//	//		return;
//	//	}

//	//	if (curState_e != eEnmeyState.End)
//	//	{ curState.ExitState(); }

//	//	preState = curState;
//	//	preState_e = curState_e;

//	//	curState_e = state;
//	//	curState = fsm[(int)state];

//	//	curState.EnterState(me);
//	//}

//	//public override void SetState(cState state)
//	//{
//	//	int index = System.Array.IndexOf(fsm, state);

//	//	if (index == -1 || curState == state)
//	//	{//넣은 state가 null이거나 없는 경우 
//	//		return;
//	//	}

//	//	if (curState != null)
//	//	{
//	//		curState.ExitState();
//	//	}

//	//	cState nextState = fsm[index];


//	//	preState = curState;
//	//	preState_e = curState_e;

//	//	curState_e = (eEnmeyState)index;
//	//	curState = state;

//	//	curState.EnterState(me);
//	//}
//}
