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
        me.curTargetPos = new Vector3(me.transform.position.x, 0f, me.transform.position.z);
        me.animCtrl.SetBool("isIdle", true);
    }

    public override void UpdateState()
    {
        curTime += Time.deltaTime;
        
        if (curTime >= patrolWaitTime)
        {
            curTime = 0;
            if (me.targetObj == null)
            {
                me.SetState((int)Enums.eSpiritState.Patrol);
            }
        }


        if(me.targetObj != null)
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
