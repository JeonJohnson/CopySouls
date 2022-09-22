using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;

public abstract class Enemy : MonoBehaviour
{

    public GameObject player;
    public float distToPlayer;

    public Structs.EnemyStatus status;

    public Animator animCtrl;
    public Rigidbody rd;
    public NavMeshAgent navAgent;

    //FSM
    public cState[] fsm;
    public cState preState;
    public eEnmeyState preState_e;
    public cState curState;
    public eEnmeyState curState_e;


    public abstract void InitializeState();

    public void SetState(cState state)
    { 
    
    
    }

    public void SetState(eEnmeyState state)
    {
        if (state == curState_e)
        {
            return;
        }

        curState.ExitState();
           
        preState = curState;
        preState_e = curState_e;

        curState_e = state;
        curState = fsm[(int)state];

        curState.EnterState(this);
    }


    protected virtual void Awake()
    {
        fsm = new cState[(int)eEnmeyState.End];

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
    }
}
