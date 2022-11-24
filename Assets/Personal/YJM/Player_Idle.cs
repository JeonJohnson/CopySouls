using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Archer_Hit_Hold;

public class Player_Idle : Player_cState
{
    public override void EnterState(Player script)
    {
        base.EnterState(script);
    }
    public override void UpdateState()
    {
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            me.SetState(Enums.ePlayerState.Move);
        }

        if (Player.instance.status.isInputtable == false) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerActionTable.instance.Interaction();
        }
        PlayerActionTable.instance.NearObjectSearch();
        Player.instance.playerModel.transform.position = Player.instance.transform.position;

        if (Input.GetKeyDown(KeyCode.R))
        {
            
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

        if(Player.instance.status.mainWeapon.type == eWeaponType.Sheild | Player.instance.status.subWeapon.type == eWeaponType.Sheild)
        {
            PlayerActionTable.instance.Guard();
        }

        if(Input.GetKeyDown(KeyCode.F))
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
        if(Input.GetKeyDown(KeyCode.P))
        {
            Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 3);
        }
        PlayerActionTable.instance.UpdateStamina();
    }

    public override void ExitState()
    {
        
    }
}
