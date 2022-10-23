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
        Player.instance.playerModel.transform.position = Player.instance.transform.position;
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {     
            me.SetState(Enums.ePlayerState.Move);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerActionTable.instance.Backstep();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            PlayerActionTable.instance.WeakAttack();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse1))
        {
            PlayerActionTable.instance.Parrying();
        }

        PlayerActionTable.instance.Guard();
    }

    public override void ExitState()
    {
        
    }
}
