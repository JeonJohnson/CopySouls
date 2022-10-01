using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : Player_cState
{
    public override void EnterState(Player script)
    {
        base.EnterState(script);
        PlayerLocomove.instance.ResetValue();
    }
    public override void UpdateState()
    {
        PlayerLocomove.instance.Move();


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PlayerActionTable.instance.Rolling();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (PlayerLocomove.instance.isMove == true && PlayerLocomove.instance.isRun == true)
            {
                PlayerActionTable.instance.DashAttack();
            }
            else
            {
                PlayerActionTable.instance.WeakAttack();
            }
        }
        if (Input.GetButtonDown("Fire1") && Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerActionTable.instance.StrongAttack();
        }
    }

    public override void ExitState()
    {
        PlayerLocomove.instance.ResetValue();
    }
}
