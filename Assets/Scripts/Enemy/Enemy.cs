using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;


//<필요 함수>
//각도 , 데미지
//적이 데미지 입는 함수
//앞잡 뒤잡 시 몬스터가 그 방향으로 회전
//

public abstract class Enemy : MonoBehaviour
{
    public Structs.EnemyStatus status;

    ////Target
    public GameObject player;
    public float distToPlayer;
    public Vector3 preTargetPos;
    public Vector3 curTargetPos;
    public GameObject targetObj;
    public float CoolTime;
    ////Target

    ////
    public GameObject weapon;


    ////FSM
    public cState[] fsm;
    public cState preState = null;
    public int preState_i = -1;
    //public eEnmeyState preState_e = eEnmeyState.End;
    public cState curState = null;
    public int curState_i = -1;
    //public eEnmeyState curState_e = eEnmeyState.End;
    ////FSM

    ////default Components
    public Animator animCtrl;
    public Collider col;
    public Rigidbody rd;
    public NavMeshAgent navAgent;
    ////default Components

    


    //// Funcs
    public abstract void InitializeState();

	//public StateController fsmCtrl;

	//public void SetState(eEnmeyState state)
	//{
	//    if (state == curState_e || fsm[(int)state] == null)
	//    {
	//        return;
	//    }
	//
	//    if (curState_e != eEnmeyState.End)
	//    { curState.ExitState(); }
	//
	//    preState = curState;
	//    preState_e = curState_e;
	//
	//    curState_e = state;
	//    curState = fsm[(int)state];
	//
	//    curState.EnterState(this);
	//}


	//public cState[] fsm;
	//public cState preState = null;
	//public eEnmeyState preState_e = eEnmeyState.End;
	//public cState curState = null;
	//public eEnmeyState curState_e = eEnmeyState.End;


	//public abstract void InitializeState();
	////추상함수임으로 Enemy클래스 상속받는 진짜 적 스크립트에서 설정해주셈.

	//public void SetState(eEnmeyState state)
	//{
	//    if (state == curState_e || fsm[(int)state] == null)
	//    {
	//        return;
	//    }

	//    if (curState_e != eEnmeyState.End)
	//    { curState.ExitState(); }

	//    preState = curState;
	//    preState_e = curState_e;

	//    curState_e = state;
	//    curState = fsm[(int)state];

	//    curState.EnterState(this);
	//}



	//public void SetState(cState state)
	//{
	//    if (state == curState)
	//    {
	//        return;
	//    }
	//    curState.ExitState();
	//    preState = curState;
	//    preState_e = curState_e;
	//    curState_e = state;
	//    curState = state;
	//    curState.EnterState(this);
	//}

    

	public T GetCurState<T>() where T : Enum
    {
        int index = System.Array.IndexOf(fsm, curState);

        if (curState == null)
        {
            Debug.Log("현재 state가 null입니다!!\nAt GetCurState Funcs");
        }

        return (T)(object)index;
    }


    public void SetState(cState state)
    {
        
        int index = System.Array.IndexOf(fsm, state);

        if (index == -1 || curState == state)
        {//넣은 state가 null이거나 없는 경우 
            return;
        }

        if (curState != null)
        {
            curState.ExitState();
        }

        //cState nextState = fsm[index];

        int curIndex = System.Array.IndexOf(fsm, curState);

        preState = curState;
        preState_i = curIndex;

        curState = state;
        curState_i = index;

        curState.EnterState(this);
    }


    public void SetState(int state)
    {
        if (fsm[state] == null)
        {
            return;
        }

        if (curState != null)
        {
            curState.ExitState();
        }

        cState nextState = fsm[state];
        int curIndex = System.Array.IndexOf(fsm, curState);

        preState = curState;
        preState_i = curIndex;

        curState = nextState;
        curState_i = state;

        curState.EnterState(this);
    }

    public void stop()
    {
        StopAllCoroutines();
    }

    protected virtual void Awake()
    {
        animCtrl = GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();

        //Enemy상속 받은 객체 각자 스크립트에서 설정해주기
        InitializeState();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        curState.UpdateState();

        CoolTime += Time.deltaTime;
    }
}
