//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Enums;

//public class StateCtrl_Archer : StateController<eArcherState>
//{
//	public StateCtrl_Archer(Enemy enemy) : base(enemy)
//	{
//		InitializeState();
//	}
//	//c# 상속에서 생성자 순서
//	//1.부모 2.자식
	
//	//public eArcherState preState_e;
//	//public eArcherState curState_e;

//	public override void InitializeState()
//	{
//		fsm = new cState[(int)eArcherState.End];
//	}

//	//public void SetState(eArcherState state)
//	//{
//	//	if (state == curState_e || fsm[(int)state] == null)
//	//	{
//	//		return;
//	//	}

//	//	if (curState_e != eArcherState.End)
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

//	//	curState_e = (eArcherState)index;
//	//	curState = state;

//	//	curState.EnterState(me);
//	//}

	


//}
