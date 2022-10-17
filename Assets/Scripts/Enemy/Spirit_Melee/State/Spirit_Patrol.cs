using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Patrol -> Idle
//Patrol -> Equipt

public class Spirit_Patrol : cState
{
    public float curTime;
    public float restartPatrolTime = 5;
    public Vector3 TargetPos;
    public bool isArrival;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isWalk", true);
        Spirit_StartPatrol();
    }

    public override void UpdateState()
    {
        curTime += Time.deltaTime;

        if (curTime < restartPatrolTime)
        {
            if(me.transform.position == TargetPos)
            {
                curTime = 0;
                isArrival = true;
            }
        }
        else if(curTime >= restartPatrolTime)
        {
            curTime = 0;
            isArrival = true;
        }

        //test
        ((Spirit)me).PatrolTimer = curTime;

        Spirit_PlayPatrolAnimation();

        //test
        ((Spirit)me).Arrival = isArrival;

        if (((Spirit)me).targetObj == null)
        {
            if (isArrival) me.SetState((int)Enums.eSpiritState.Idle);
        }
        else
        {
            me.SetState((int)Enums.eSpiritState.Equipt);
        }
    }

    public override void ExitState()
    {
        Spirit_StopPatrol();
        Spirit_StopPatrolAnimation();
        //test
        ((Spirit)me).Arrival = isArrival;
    }










    public Vector3 SetTargetPos(int minDis, int maxDis)
    {
        if (maxDis <= 0 || minDis <= 0)
        {
            Debug.Log("잘못된 입력");
            return Vector3.zero;
        }
        //maxRange
        float maxX = ((Spirit)me).responPos.x + me.status.patrolRange;
        float maxZ = ((Spirit)me).responPos.z + me.status.patrolRange;

        //순찰범위를 통한 범위비율
        //float distanceRatio = maxDis / me.status.patrolRange;

        Vector3 TargetPos;

        int distanceX = Random.Range(minDis, maxDis + 1);
        int distanceZ = Random.Range(minDis, maxDis + 1);
        float randX = Random.Range(-1, 2) * distanceX;
        float randZ = Random.Range(-1, 2) * distanceZ;
        if ((randX == 0 && randZ == 0) || randX > maxX || randZ > maxZ) SetTargetPos(minDis, maxDis);
        TargetPos = new Vector3(me.transform.position.x + randX, 0, me.transform.position.z + randZ);
        //똑같은 위치 값을 뽑거나 //지금 내 위치와 너무 가까우면
        if ((TargetPos == me.preTargetPos) || ((Vector3.Distance(me.transform.position, TargetPos)) < 3f)) SetTargetPos(minDis, maxDis);

        if (TargetPos != null)
        {
            me.preTargetPos = me.curTargetPos;
            me.curTargetPos = TargetPos;
        }

        return TargetPos;
    }

    public void Spirit_StartPatrol()
    {
        me.navAgent.speed = me.status.moveSpd;
        me.MoveOrder(SetTargetPos(6, 10));
        me.animCtrl.SetBool("isPatrol", true);
    }

    public void Spirit_StopPatrol()
    {
        isArrival = false;
        me.animCtrl.SetBool("isPatrol", false);
        //me.curTargetPos = me.transform.position;
        me.MoveStop();
    }

    public void Spirit_PlayPatrolAnimation()
    {
        if (!isArrival) me.animCtrl.SetBool("isWalk", true);
        else me.animCtrl.SetBool("isWalk", false);
    }
    public void Spirit_StopPatrolAnimation()
    {
        me.animCtrl.SetBool("isWalk", false);
    }

}
