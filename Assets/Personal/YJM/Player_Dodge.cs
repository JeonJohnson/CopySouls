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
        if (PlayerActionTable.instance.StaminaCheck() == true)
        {
            Debug.Log("»ç½Ç");
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

                if (Input.GetButtonDown("Fire1"))
                {
                    PlayerActionTable.instance.RollingAttack();
                }

                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse1))
                {
                    PlayerActionTable.instance.Parrying();
                }
            }
        }
    }

    public override void ExitState()
    {
        PlayerLocomove.instance.PlayerPosFix();
    }
}
