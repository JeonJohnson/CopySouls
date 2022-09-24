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
        me.SetState(Enums.eEnmeyState.Patrol);
    }

    public override void ExitState()
    {

    }
}
