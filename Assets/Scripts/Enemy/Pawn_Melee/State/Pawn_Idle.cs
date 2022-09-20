using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_Idle : cState
{

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);


    }

    public override void UpdateState()
    {
        if (me.distToPlayer < me.status.patrolRange)
        {
            me.SetState(Enums.eEnmeyState.Move);
        }
    }

    public override void ExitState()
    {

    }

}
