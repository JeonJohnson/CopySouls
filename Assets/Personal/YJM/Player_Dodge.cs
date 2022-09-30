using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dodge : Player_cState
{
    public override void EnterState(Player script)
    {
        base.EnterState(script);
    }
    public override void UpdateState()
    {
        PlayerLocomove.instance.PlayerControlCam();
    }

    public override void ExitState()
    {
        PlayerLocomove.instance.PlayerPosFix();
    }
}
