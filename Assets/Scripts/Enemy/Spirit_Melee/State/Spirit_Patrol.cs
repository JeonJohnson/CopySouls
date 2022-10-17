using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

       

        if (isArrival)
        {
            me.SetState((int)Enums.eSpiritState.Idle);
        }
     //   else
     //   {
     //       //221002 20:23 player -> targetObj
     //       if (me.distToTarget <= me.status.ricognitionRange)
     //       {
     //           me.SetState((int)Enums.eSpiritState.Equipt);
     //       }
     //   }

        
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
        Vector3 TargetPos;

        int distanceX = Random.Range(minDis, maxDis + 1);
        int distanceZ = Random.Range(minDis, maxDis + 1);
        int randX = Random.Range(-1, 2) * distanceX;
        int randZ = Random.Range(-1, 2) * distanceZ;
        if (randX == 0 && randZ == 0) SetTargetPos(minDis, maxDis);
        TargetPos = new Vector3(me.transform.position.x + randX, 0, me.transform.position.z + randZ);
        if (TargetPos == me.preTargetPos) SetTargetPos(minDis, maxDis);

        if (TargetPos != null)
        {
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
        me.curTargetPos = me.transform.position;
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
