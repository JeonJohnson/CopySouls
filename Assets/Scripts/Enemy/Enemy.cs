﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Enums;
using Structs;


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
    public bool isCombat;
    public GameObject weapon;

    public List<Vector3> patrolPosList;


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


    public void UpdateStatus()
    { //스테이터스 수치들 각종 컴포넌트에 연동 되도록.

        //네브 요원
        navAgent.speed = status.moveSpd;
        
    
    }



	public void MoveOrder(Vector3 dest)
	{//네비 에이전트 움직이는거 편하게
        if (navAgent == null)
        {
            return;
        }

        navAgent.isStopped = true;
        navAgent.destination = dest;
        navAgent.isStopped = false;
	}


	//// Funcs
	public abstract void InitializeState();

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

    public virtual void Hit(DamagedStruct dmgStruct)
    {
        status.curHp -= (int)dmgStruct.dmg;

        //if (dmgStruct.isBackstab)
        //{
        //    transform.forward = player.transform.forward;
        //}
    }
    //public abstract void Death();

    protected virtual void Awake()
    {
        animCtrl = GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();


        //for Test
        patrolPosList = new List<Vector3>();
        patrolPosList.Add(new Vector3());
        patrolPosList.Add(new Vector3(10,0,10));
        patrolPosList.Add(new Vector3(5,0,-10));
        //for Test


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
        if (player != null)
        { distToPlayer = Vector3.Distance(transform.position, player.transform.position); }
            
        curState.UpdateState();



        CoolTime += Time.deltaTime;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        Structs.DamagedStruct dmg = new Structs.DamagedStruct(10, false);

    //        Enemy script = other.GetComponent<Enemy>();
    //        script.Hit(dmg);
    //    }

    //}


    //private void OnDrawGizmos()
    //{

    //    ////인식범위
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, status.ricognitionRange);
    //    ////인식범위

    //    ////시야각
    //    //프로스텀으로 보여주기

    //    ////시야각

    //    ////공격 사정거리
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, status.atkRange);
    //    ////공격 사정거리


    //    ////패트롤 예상 이동 궤적
    //    Gizmos.color = Color.blue;

    //    for (int i = 0; i < patrolPosList.Count; ++i)
    //    {
    //        if (i == (patrolPosList.Count - 1))
    //        {
    //            Gizmos.DrawLine(patrolPosList[i], patrolPosList[0]);
    //        }
    //        else
    //        {
    //            Gizmos.DrawLine(patrolPosList[i], patrolPosList[i + 1]);
    //        }
    //    }
    //}

    private void OnDrawGizmosSelected()
	{

        ////인식범위
        Color temp = Color.yellow;
        temp.a = 0.4f;
        //Gizmos.color = Color.yellow;
        Gizmos.color = temp;
        Gizmos.DrawSphere(transform.position, status.ricognitionRange);
        ////인식범위

        ////시야각
        //프로스텀으로 보여주기

        ////시야각

        ////공격 사정거리
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.atkRange);
		////공격 사정거리


		////패트롤 예상 이동 궤적
		Gizmos.color = Color.blue;

		for (int i = 0; i < patrolPosList.Count; ++i)
		{
            if (i == (patrolPosList.Count - 1))
            {
                Gizmos.DrawLine(patrolPosList[i], patrolPosList[0]);
            }
            else
            {
                Gizmos.DrawLine(patrolPosList[i], patrolPosList[i + 1]);
			}
		}
		////패트롤 예상 이동 궤적
	}
}
