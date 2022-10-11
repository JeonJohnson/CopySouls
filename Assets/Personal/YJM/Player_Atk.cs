using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Atk : Player_cState
{
    Transform playerTr;
    public override void EnterState(Player script)
    {
        base.EnterState(script);
        playerTr = Player.instance.playerModel.transform;
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        PlayerLocomove.instance.PlayerPosFix();
    }
}
