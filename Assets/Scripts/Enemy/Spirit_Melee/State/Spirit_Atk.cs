using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Atk : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.curTargetPos = me.player.transform.position;
        me.transform.LookAt(me.curTargetPos);
        me.animCtrl.SetBool("isRun", true);
    }
    public override void UpdateState()
    {
        if(me.distToPlayer <= me.status.atkRange)
        {
            me.navAgent.isStopped = true;
            me.animCtrl.SetBool("isRun", false);
            me.animCtrl.SetTrigger("isAtk1");
        }
    }

    public override void ExitState()
    {

    }
}
