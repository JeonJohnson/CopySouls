using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Archer_Hit_Hold;

public class Player_Idle : Player_cState
{
    public override void EnterState(Player script)
    {
        base.EnterState(script);
        CameraEffect.instance.Stop = true;
    }
    public override void UpdateState()
    {
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            me.SetState(Enums.ePlayerState.Move);
        }

        if (Player.instance.status.isInputtable == false) return;

        if (Input.GetButtonDown("Interaction"))
        {
            PlayerActionTable.instance.Interaction();
        }
        PlayerActionTable.instance.NearObjectSearch();

        if (PlayerActionTable.instance.StaminaCheck() == true)
        {
            if (Input.GetButtonDown("Dodge"))
            {
                PlayerActionTable.instance.Backstep();
            }

            if (Input.GetButtonDown("Fire1") && Input.GetButton("Fire3"))
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

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("Fire2"))
            {
                PlayerActionTable.instance.Parrying();
            }
        }

        PlayerActionTable.instance.Guard();


        if(Input.GetButtonDown("Hold"))
        {
            if(PlayerActionTable.instance.holdType == false)
            {
                PlayerActionTable.instance.ChangeWeaponHoldType(true);
                PlayerActionTable.instance.holdType = true;
            }
            else
            {
                PlayerActionTable.instance.ChangeWeaponHoldType(false);
                PlayerActionTable.instance.holdType = false;
            }
        }    
        PlayerActionTable.instance.UpdateStamina();

        if(Player.instance.status.isDead == true)
        {
            Player.instance.SetState(Enums.ePlayerState.Hit);
        }
    }

    public override void ExitState()
    {
        
    }
}
