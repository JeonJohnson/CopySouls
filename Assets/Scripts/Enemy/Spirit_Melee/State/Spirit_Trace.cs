using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Trace : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isRun", true);
    }

    public override void UpdateState()
    {
        if (me.distToPlayer <= me.status.atkRange)
        {
            me.navAgent.isStopped = true;
            me.animCtrl.SetBool("isRun", false);
            if (me.CoolTime > me.status.slashCoolTime)
            {
                me.SetState(Enums.eEnmeyState.Atk);
                
                me.CoolTime = 0;
            }
        }
    }

    public override void ExitState()
    {

    }
}
