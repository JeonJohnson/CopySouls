using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;

public abstract class Enemy : MonoBehaviour
{
    public GameObject player;
    public float distToPlayer;
    public Vector3 preTargetPos;
    public Vector3 curTargetPos;

    public Structs.EnemyStatus status;

    public Animator animCtrl;
    public Rigidbody rd;
    public NavMeshAgent navAgent;

    //FSM
    public cState[] fsm;
    public cState preState = null;
    public eEnmeyState preState_e = eEnmeyState.End;
    public cState curState = null;
    public eEnmeyState curState_e = eEnmeyState.End;


    public abstract void InitializeState();
    //추상함수임으로 Enemy클래스 상속받는 진짜 적 스크립트에서 설정해주셈.


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

    public void ChangeTargetPos()
    {
        StartCoroutine(Think());
    }
    public void StopChangeTargetPos()
    {
        StopAllCoroutines();
        //StopCoroutine(Think());
    }
    public Vector3 ThinkTargetPos()
    {
        int randX = Random.Range(-1, 2) * 10;
        int randZ = Random.Range(-1, 2) * 10;
        curTargetPos = new Vector3(transform.position.x + randX, 0, transform.position.z + randZ);
        if (curTargetPos == preTargetPos) ThinkTargetPos();
        return curTargetPos;
    }
    public IEnumerator Think()
    {
        yield return new WaitForSeconds(2.0f);
        navAgent.isStopped = false;
        animCtrl.SetBool("isWalk", true);
        preTargetPos = curTargetPos;
        curTargetPos = ThinkTargetPos();
        yield return new WaitForSeconds(5.0f);
        navAgent.isStopped = true;
        animCtrl.SetBool("isWalk", false);
        curTargetPos = transform.position;
        StopCoroutine(Think());
        StartCoroutine(Think());
    }


    public void SetState(eEnmeyState state)
    {
        if (state == curState_e || fsm[(int)state] == null)
        {
            return;
        }

        if (curState_e != eEnmeyState.End)
        { curState.ExitState(); }
           
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
