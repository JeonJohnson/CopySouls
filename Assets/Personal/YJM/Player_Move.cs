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


        if (Input.GetKeyDown(KeyCode.Space)) // ����Ʈ �����鼭 �Է��ϸ� �ν� �ȵ�. �����Է� �����ֳ�?
        {
            PlayerActionTable.instance.Rolling();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            PlayerActionTable.instance.WeakAttack();
        }
    }

    public override void ExitState()
    {
        PlayerLocomove.instance.ResetValue();
    }
}
