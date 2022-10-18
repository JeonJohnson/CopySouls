using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dodge : Player_cState
{
    public bool isRolling = false;
    public override void EnterState(Player script)
    {
        base.EnterState(script);
    }
    public override void UpdateState()
    {
        if (PlayerActionTable.instance.isComboCheck == true)
        {
            if ((Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f) && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("닷지 스페이스 입력");
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
        }
    }

    public override void ExitState()
    {
        PlayerLocomove.instance.PlayerPosFix();
    }
}
