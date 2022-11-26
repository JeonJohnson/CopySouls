using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interacting : Player_cState
{
    public override void EnterState(Player script)
    {
        base.EnterState(script);
        PlayerActionTable.instance.ResetGuardValue();
    }
    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        PlayerActionTable.instance.ResetGuardValue();
    }
}
