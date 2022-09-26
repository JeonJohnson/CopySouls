using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Trace : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Spirit_StartTraceAnimation();
        //me.navAgent.isStopped = false;
    }

    public override void UpdateState()
    {
        me.curTargetPos = me.player.transform.position;

        me.transform.LookAt(me.curTargetPos);

        if (me.distToPlayer <= me.status.atkRange)
        {
            me.SetState((int)Enums.eSpiritState.Atk);
        }
        if (me.distToPlayer > me.status.ricognitionRange)
        {
            me.SetState((int)Enums.eSpiritState.Patrol);
        }
    }

    public override void ExitState()
    {
        Spirit_StopTraceAnimation();
        //me.navAgent.isStopped = true;
    }

    public void Spirit_StartTraceAnimation()
    {
        if (((Spirit)me).isMoving) me.animCtrl.SetBool("isRun", true);
        else me.animCtrl.SetBool("isRun", false);
    }

    public void Spirit_StopTraceAnimation()
    {
        me.animCtrl.SetBool("isRun", false);
    }

}
