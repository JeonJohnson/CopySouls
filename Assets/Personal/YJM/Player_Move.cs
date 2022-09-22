using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : Player_cState
{
    public override void EnterState(Player script)
    {
        base.EnterState(script);
    }
    public override void UpdateState()
    {
        PlayerMove.instance.Move();
    }

    public override void ExitState()
    {
        PlayerMove.instance.ResetValue();
    }
}
