using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Patrol -> Idle
//Patrol -> Equipt

public class Spirit_Patrol : cState
{
    public float curTime;
    public Vector3 targetPos;
    public bool isArrival = false;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Spirit_StartPatrol();
    }

    public override void UpdateState()
    {
        ((Spirit)me).Arrival = isArrival;

        Debug.Log(Vector3.Distance(me.transform.position, targetPos));

        if (me.isAlert) me.SetState((int)Enums.eSpiritState.Equipt);
        else
        {
            if (Vector3.Distance(me.transform.position, targetPos) < 1f)
            {
                isArrival = true;
                me.animCtrl.SetBool("isWalk", false);
            }
            else isArrival = false;
        }
        if(isArrival) me.SetState((int)Enums.eSpiritState.Idle);

    }

    public override void ExitState()
    {
        Spirit_StopPatrol();
        Spirit_StopPatrolAnimation();
    }





    public void MoveToNextWayPoint()
    {
        int ranIndex = Random.Range(0, ((Spirit)me).PatrolPos.Length);
        bool isfalse = false;
        if (ranIndex < 0 || ranIndex == ((Spirit)me).curPatrol_Index || ranIndex > ((Spirit)me).PatrolPos.Length) isfalse = true;

        if(!isfalse)
        {
            ((Spirit)me).prePatrol_Index = ((Spirit)me).curPatrol_Index;
            ((Spirit)me).curPatrol_Index = ranIndex;

            targetPos = ((Spirit)me).PatrolPos[ranIndex].position;
            return;
        }
        else MoveToNextWayPoint();
    }
    
    public void Spirit_StartPatrol()
    {
        if (!isArrival)
        {
            MoveToNextWayPoint();
            me.navAgent.speed = me.status.moveSpd;
            me.MoveOrder(targetPos);
            me.animCtrl.SetBool("isWalk", true);
            me.animCtrl.SetBool("isPatrol", true);
        }
    }

    public void Spirit_StopPatrol()
    {
        isArrival = false;
        me.animCtrl.SetBool("isPatrol", false);
        me.animCtrl.SetBool("isWalk", false);
        me.MoveStop();
    }

    public void Spirit_PlayPatrolAnimation()
    {
        
    }
    public void Spirit_StopPatrolAnimation()
    {
        me.animCtrl.SetBool("isWalk", false);
    }

}
