using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hit : Player_cState
{
    public override void EnterState(Player script)
    {
        base.EnterState(script);
        PlayerActionTable.instance.ResetGuardValue();
    }
    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        Player.instance.status.isParrying = false;
        PlayerActionTable.instance.isComboCheck = false;
        PlayerActionTable.instance.EnableWeaponMeshCol(0);
        Player.instance.SetModelCollider(true);
    }
}
