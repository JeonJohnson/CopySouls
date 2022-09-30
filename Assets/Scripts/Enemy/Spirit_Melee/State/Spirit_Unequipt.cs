using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Unequipt : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);

        me.navAgent.isStopped = true;
        me.animCtrl.SetBool("isUnequipt", true);
    }

    public override void UpdateState()
    {
        if (me.distToPlayer <= me.status.ricognitionRange)
        {
            me.SetState((int)Enums.eSpiritState.Equipt);
        }
        else
        {
            if (((Spirit)me).complete_Unequipt)
            {
                me.SetState((int)Enums.eSpiritState.Idle);
            }
        }
    }

    public override void ExitState()
    {
        ((Spirit)me).complete_Unequipt = false;
        me.animCtrl.SetBool("isUnequipt", false);
    }
}
