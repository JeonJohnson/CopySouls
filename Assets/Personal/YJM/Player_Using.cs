using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Using : Player_cState
{
    float moveMentSpeed;
    bool isMoveDataSaved = false;
    public override void EnterState(Player script)
    {
        base.EnterState(script);
        PlayerActionTable.instance.ResetGuardValue();
        if(isMoveDataSaved == false)
        {
            moveMentSpeed = PlayerLocomove.instance.movementSpeed;
            isMoveDataSaved = true;
        }
        PlayerLocomove.instance.movementSpeed = 1f;
    }
    public override void UpdateState()
    {
        Player.instance.animator.SetLayerWeight(1, 1f);
        PlayerLocomove.instance.Move();
    }

    public override void ExitState()
    {
        PlayerActionTable.instance.ResetGuardValue();
        PlayerLocomove.instance.movementSpeed = moveMentSpeed;
    }
}
