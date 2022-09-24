using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Idle : Player_cState
{
    public override void EnterState(Player script)
    {
        base.EnterState(script);
    }
    public override void UpdateState()
    {
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            me.SetState(Enums.ePlayerState.Move);
        }
    }

    public override void ExitState()
    {

    }
}
