using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Idle -> Patrol
//Idle -> Equipt

public class Spirit_Idle : cState
{
    public float curTime = 0;
    public float patrolWaitTime = 2;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.MoveStop();
        me.animCtrl.SetBool("isIdle", true);
    }

    public override void UpdateState()
    {
        if (me.isAlert) me.SetState((int)Enums.eSpiritState.Equipt);
        curTime += Time.deltaTime;
        if (curTime >= patrolWaitTime)
        {
            curTime = 0;
            me.SetState((int)Enums.eSpiritState.Patrol);
        }
    }

    public override void ExitState()
    {
        me.animCtrl.SetBool("isIdle", false);
    }
}
