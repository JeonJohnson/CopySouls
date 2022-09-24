using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Atk : cState
{


    public override void EnterState(Enemy script)
    {
        

        base.EnterState(script);
        me.navAgent.isStopped = true;
        me.animCtrl.SetTrigger("isSlash");
    }
    public override void UpdateState()
    {
        if (me.distToPlayer > me.status.atkRange)
        {
            me.SetState((int)Enums.eSpiritState.Trace);
        }
    }

    public override void ExitState()
    {
    }
}
