using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;



public class Spirit : Enemy
{
    public eSpiritState curState_e;
    public bool isMoving;
    

    public override void InitializeState()
	{
        fsm = new cState[(int)Enums.eSpiritState.End];

        //limitPatrolRange = new Vector3(transform.position.x + status.patrolRange, transform.position.y + status.patrolRange);

        fsm[(int)Enums.eSpiritState.Idle] = new Spirit_Idle();
        fsm[(int)Enums.eSpiritState.Patrol] = new Spirit_Patrol();
        fsm[(int)Enums.eSpiritState.Trace] = new Spirit_Trace();
        fsm[(int)Enums.eSpiritState.Atk] = new Spirit_Atk();

        SetState((int)Enums.eSpiritState.Idle);
	}

	protected override void Awake()
    {
        base.Awake();

        this.rd = GetComponent<Rigidbody>();
        this.animCtrl = GetComponent<Animator>();
        this.navAgent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        curState_e = GetCurState<Enums.eSpiritState>();
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if(isMoving && targetObj == null)
        {
            //����Ÿ����ġ�� ���� ���� Ÿ���������� �����̴� ���ε� �̹� ���� �ߴٸ�
            if (transform.position == curTargetPos) navAgent.isStopped = true;
            else if (transform.position != curTargetPos) navAgent.isStopped = false;
        }

        isMoving = !navAgent.isStopped;

        if (isMoving) navAgent.SetDestination(curTargetPos);

        //GameObject obj = UnitManager.Instance.playerObj;
    }


    public Vector3 ThinkRandomTargetPos(int minDis, int maxDis)
    {
        Vector3 TargetPos;

        int distanceX = Random.Range(minDis, maxDis + 1);
        int distanceZ = Random.Range(minDis, maxDis + 1);
        int randX = Random.Range(-1, 2) * distanceX;
        int randZ = Random.Range(-1, 2) * distanceZ;
        if (randX == 0 && randZ == 0) ThinkRandomTargetPos(minDis, maxDis);
        TargetPos = new Vector3(transform.position.x + randX, 0, transform.position.z + randZ);
        if (TargetPos == preTargetPos) ThinkRandomTargetPos(minDis, maxDis);
        return TargetPos;
    }

    public IEnumerator AutoMove(float moveDulationTime, float restartTime)
    {
        if (targetObj == null)
        {
            preTargetPos = curTargetPos;
            curTargetPos = ThinkRandomTargetPos(6, 10);
            navAgent.isStopped = false;
            yield return new WaitForSeconds(moveDulationTime);
            navAgent.isStopped = true;
            yield return new WaitForSeconds(restartTime);
            CoroutineHelper.Instance.StartCoroutine(AutoMove(moveDulationTime, restartTime));
        }
    }

    public IEnumerator DelayNavOn()
    {
        if(isMoving) navAgent.isStopped = true;
        animCtrl.SetTrigger("isEquipt");
        animCtrl.SetBool("isTrace", true);
        yield return new WaitForSeconds(1.2f);
        targetObj = player;
        navAgent.isStopped = false;

    }


}
