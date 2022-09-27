using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;

public class Spirit : Enemy
{
    public eSpiritState curState_e;
    public bool isMoving;
    public bool TargetOn;
    public bool RandomOn;

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
        isMoving = !navAgent.isStopped;
        ChangeSpeed();
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        //아직 타겟지점으로 움직이는 중인데 이미 도착 했다면
        if (targetObj == null && curTargetPos != null && !navAgent.isStopped && transform.position == curTargetPos) navAgent.isStopped = true;
        if (targetObj != null && curTargetPos == null && !navAgent.isStopped && transform.position == targetObj.transform.position) navAgent.isStopped = true;

        if (isMoving)
        {
            
            if (targetObj != null && curTargetPos == null)
            {
                navAgent.SetDestination(targetObj.transform.position);

                TargetOn = true;
                RandomOn = false;

            }
            else if (targetObj == null && curTargetPos != null)
            {
                navAgent.SetDestination(curTargetPos);

                TargetOn = false;
                RandomOn = true;
            }
        }

        //GameObject obj = UnitManager.Instance.playerObj;
    }

    public void ChangeSpeed()
    {
        if (GetCurState<Enums.eSpiritState>() == Enums.eSpiritState.Patrol) navAgent.speed = status.moveSpd;
        else if (GetCurState<Enums.eSpiritState>() == Enums.eSpiritState.Atk) navAgent.speed = status.runSpd;
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
            navAgent.isStopped = false;
            preTargetPos = curTargetPos;
            curTargetPos = ThinkRandomTargetPos(6, 10);
            yield return new WaitForSeconds(moveDulationTime);
            navAgent.isStopped = true;
            yield return new WaitForSeconds(restartTime);
            CoroutineHelper.Instance.StartCoroutine(AutoMove(moveDulationTime, restartTime));
        }
    }

    public IEnumerator EquiptAndTargetSelction()
    {
        animCtrl.SetTrigger("isEquipt");
        yield return new WaitForSeconds(1.2f);
        navAgent.isStopped = false;
        targetObj = player;
    }
}
