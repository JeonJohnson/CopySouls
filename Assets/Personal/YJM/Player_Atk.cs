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
        PlayerActionTable.instance.ResetGuardValue();
    }
    public override void UpdateState()
    {
        if (PlayerActionTable.instance.StaminaCheck() == true)
        {
            if (PlayerActionTable.instance.isComboCheck == true)
            {
                if (Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.LeftShift))
                {
                    PlayerActionTable.instance.ChargeAttack();
                }
                else if (Input.GetButtonDown("Fire1"))
                {
                    PlayerActionTable.instance.WeakAttack();
                }

                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse1))
                {
                    PlayerActionTable.instance.Parrying();
                }

                if ((Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f) && Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerActionTable.instance.Rolling();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerActionTable.instance.Backstep();
                }
            }
        }
    }
    public override void ExitState()
    {
        PlayerLocomove.instance.PlayerPosFix();
    }
}
