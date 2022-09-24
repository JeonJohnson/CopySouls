using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Patrol : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.ChangeTargetPos();
    }

    public override void UpdateState()
    {
        //공격상태로 전환
        if (me.distToPlayer < me.status.ricognitionRange)
        {
            me.SetState(Enums.eEnmeyState.Atk);
        }

    }

    public override void ExitState()
    {
        me.StopChangeTargetPos();
    }
}
