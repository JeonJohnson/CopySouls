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
        if(Input.GetKeyDown(KeyCode.E))
        {
            PlayerActionTable.instance.Interaction();
        }
        PlayerActionTable.instance.NearObjectSearch();
        Player.instance.playerModel.transform.position = Player.instance.transform.position;
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            me.SetState(Enums.ePlayerState.Move);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerActionTable.instance.UseItem();
        }

        if (PlayerActionTable.instance.StaminaCheck() == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerActionTable.instance.Backstep();
            }

            if (Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.LeftShift))
            {
                PlayerActionTable.instance.ChargeAttack();
            }
            else if(Input.GetButtonDown("Fire1"))
            {
                if (!PlayerActionTable.instance.HoldAttackCheck())
                {
                    PlayerActionTable.instance.WeakAttack();
                }
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse1))
            {
                PlayerActionTable.instance.Parrying();
            }
        }
            PlayerActionTable.instance.Guard();
        PlayerActionTable.instance.UpdateStamina();
    }

    public override void ExitState()
    {
        
    }
}
