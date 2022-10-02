using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Idle : cState
{
    public float curTime = 0;
    public float patrolWaitTime = 2;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isIdle", true);
    }

    public override void UpdateState()
    {
        curTime += Time.deltaTime;
        
        if (curTime >= patrolWaitTime)
        {
            curTime = 0;
            me.SetState((int)Enums.eSpiritState.Patrol);
        }
        //221002 20:23 player -> targetObj
        if (me.distToTarget <= me.status.ricognitionRange)
        {
            me.SetState((int)Enums.eSpiritState.Equipt);
        }

        //test
        ((Spirit)me).WaitTimer = curTime;

    }

    public override void ExitState()
    {
        me.animCtrl.SetBool("isIdle", false);
    }
}
