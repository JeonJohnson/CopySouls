using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : Player_cState
{
    bool isRolling = false;
    public override void EnterState(Player script)
    {
        base.EnterState(script);
        isRolling = false;
    }
    public override void UpdateState()
    {
        PlayerLocomove.instance.Move();

        if (PlayerLocomove.instance.isRun == false)
        {
            PlayerActionTable.instance.UpdateStamina();
        }

        if (Player.instance.status.isInputtable == false) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerActionTable.instance.Interaction();
        }
        PlayerActionTable.instance.NearObjectSearch();

        if (PlayerActionTable.instance.StaminaCheck() == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerActionTable.instance.Rolling();
                me.SetState(Enums.ePlayerState.Dodge);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse1))
            {
                PlayerActionTable.instance.Parrying();
            }

            if (Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.LeftShift))
            {
                PlayerActionTable.instance.ChargeAttack();
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                if (PlayerLocomove.instance.isMove == true && PlayerLocomove.instance.isRun == true)
                {
                    PlayerActionTable.instance.DashAttack();
                }
                else
                {
                    if (!PlayerActionTable.instance.HoldAttackCheck())
                    {
                        PlayerActionTable.instance.WeakAttack();
                    }
                }
            }
            PlayerActionTable.instance.Guard();
        }
    }

    public override void ExitState()
    {
        PlayerLocomove.instance.ResetValue();
    }
}
