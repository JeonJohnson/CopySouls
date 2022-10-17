using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;



public class Spirit : Enemy
{
    public Vector3 responPos;

    public eSpiritState curState_e;
    public float WaitTimer;     //test
    public float PatrolTimer;   //test
    public bool Arrival;        //test
    public bool complete_Equipt;
    public bool complete_Unequipt;
    public bool complete_Atk;
    public bool isEquipt;

    public override void InitializeState()
	{
        fsm = new cState[(int)Enums.eSpiritState.End];

        //limitPatrolRange = new Vector3(transform.position.x + status.patrolRange, transform.position.y + status.patrolRange);

        fsm[(int)Enums.eSpiritState.Idle] = new Spirit_Idle();
        fsm[(int)Enums.eSpiritState.Patrol] = new Spirit_Patrol();
        fsm[(int)Enums.eSpiritState.Equipt] = new Spirit_Equipt();
        fsm[(int)Enums.eSpiritState.Unequipt] = new Spirit_Unequipt();
        fsm[(int)Enums.eSpiritState.Trace] = new Spirit_Trace();
        fsm[(int)Enums.eSpiritState.Atk] = new Spirit_Atk();

        SetState((int)Enums.eSpiritState.Idle);
	}

	protected override void Awake()
    {
        base.Awake();
        responPos = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        //if(Input.GetKey("a"))
        //{
        //    status.curHp--;
        //}

        curState_e = GetCurState<Enums.eSpiritState>();

        if (targetObj != null)
        {
            curTargetPos = targetObj.transform.position;
            distToTarget = Vector3.Distance(targetObj.transform.position, transform.position);
        }

        if(isEquipt)
        {
            //weapon.gameObject.SetActive(true);
            //GameObject.Find("Sword").SetActive(false);
        }
        else
        {
            //weapon.gameObject.SetActive(false);
            //GameObject.Find("Sword").SetActive(true);
        }

    }

    //애니메이션 이벤트==================================

    public void Spirit_Melee_CompleteEquiptment()
    {
        complete_Equipt = true;
    }
    public void Spirit_Melee_CompleteUnequiptment()
    {
        complete_Unequipt = true;
    }
    public void Spirit_Melee_CompleAtk()
    {
        complete_Atk = true;
    }

    private void OnDrawGizmosSelected()
    {
        //순찰범위
        //Color temp_White = Color.white;
        //temp_White.a = 0.4f;
        //Gizmos.color = temp_White;
        //Gizmos.DrawSphere(responPos, status.patrolRange);

        //타겟위치
        Color temp_Blue = Color.blue;
        temp_Blue.a = 0.4f;
        Gizmos.color = temp_Blue;
        Gizmos.DrawSphere(curTargetPos + Vector3.up * 0.5f, 0.5f);
    }
}
