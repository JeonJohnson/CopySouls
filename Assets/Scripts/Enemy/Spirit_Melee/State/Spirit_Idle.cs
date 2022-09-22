using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Idle : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
    }

    public override void UpdateState()
    {
        me.distToPlayer = Vector3.Distance(me.player.transform.position, me.transform.position);
        if (me.distToPlayer < me.status.patrolRange)
        {
            me.SetState(Enums.eEnmeyState.Move);
        }
    }

    public override void ExitState()
    {

    }

}
