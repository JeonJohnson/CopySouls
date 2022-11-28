using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dodge : Player_cState
{
    public bool isRolling = false;
    public override void EnterState(Player script)
    {
        base.EnterState(script);
        PlayerActionTable.instance.ResetGuardValue();
    }
    public override void UpdateState()
    {
        if (Player.instance.status.isInputtable == false) return;

        if (PlayerActionTable.instance.StaminaCheck() == true)
        {
            if (PlayerActionTable.instance.isComboCheck == true)
            {
                if ((Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f) && Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerActionTable.instance.Rolling();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerActionTable.instance.Backstep();
                }

                if (Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.LeftShift))
                {
                    PlayerActionTable.instance.ChargeAttack();
                }
                else if (Input.GetButtonDown("Fire1"))
                {
                    PlayerActionTable.instance.RollingAttack();
                }
            }
        }
    }

    public override void ExitState()
    {
        Player.instance.status.isParrying = false;
    }
}
