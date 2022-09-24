using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Trace : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isRun", true);
        me.navAgent.isStopped = false;
    }

    public override void UpdateState()
    {
        me.curTargetPos = me.player.transform.position;
        me.transform.LookAt(me.curTargetPos);

        if (me.distToPlayer <= me.status.atkRange)
        {
            me.SetState(Enums.eEnmeyState.Atk);
        }
        if (me.distToPlayer > me.status.ricognitionRange)
        {
            me.SetState(Enums.eEnmeyState.Patrol);
        }
    }

    public override void ExitState()
    {
        me.animCtrl.SetBool("isRun", false);
        me.navAgent.isStopped = true;
    }
}
