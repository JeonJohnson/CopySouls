//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;

//public abstract class StateController
//{
//	public StateController(Enemy enemy)
//	{
//		me = enemy;
//	}

//	public Enemy me;

//	public cState[] fsm;
//	public cState preState = null;
//	public cState curState = null;
//	public T preState_e;
//	public T curState_e;
//	//각자 필요한 enum State 선언해서 쓰기 

//	public abstract void InitializeState();

//	public void SetState(cState state)
//	{
//		int index = System.Array.IndexOf(fsm, state);

//		if (index == -1 || curState == state)
//		{//넣은 state가 null이거나 없는 경우 
//			return;
//		}

//		if (curState != null)
//		{
//			curState.ExitState();
//		}

//		cState nextState = fsm[index];


//		preState = curState;
//		preState_e = curState_e;

//		curState_e = (T)(object)index;
//		curState = state;

//		curState.EnterState(me);
//	}

//	public void SetState(T state)
//	{
//		if ((int)(object)state == (int)(object)curState_e || fsm[(int)(object)state] == null)
//		{
//			return;
//		}


		
//		if ((int)(object)curState_e != (int)(object)Enum.GetValues(typeof(T)).Cast<T>().Last())
//		{ curState.ExitState(); }

//		preState = curState;
//		preState_e = curState_e;

//		curState_e = state;
//		curState = fsm[(int)(object)state];

//		curState.EnterState(me);
//	}

	


//}
