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
        Player.instance.animator.SetLayerWeight(1, 1f);
        PlayerLocomove.instance.Move();
    }

    public override void ExitState()
    {

    }
}
