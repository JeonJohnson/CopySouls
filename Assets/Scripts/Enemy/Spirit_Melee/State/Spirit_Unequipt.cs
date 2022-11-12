using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Unequipt : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        ((Spirit)me).isEquipt = false;
        me.weaponEquipState = eEquipState.UnEquip;
        me.MoveStop();
        me.animCtrl.SetBool("isUnequipt", true);
    }

    public override void UpdateState()
    {
        if (((Spirit)me).complete_Unequipt)
        {
            me.GetComponent<FieldOfView>().viewAngle = ((Spirit)me).initFOVAngle;
            if(me.combatState == eCombatState.Alert)
            {
                me.SetState((int)Enums.eSpiritState.Equipt);
            }
            else
            {
                me.SetState((int)Enums.eSpiritState.Idle);
            }
        }
        else
        {
            if (me.status.isBackHold)
            {
                me.SetState((int)Enums.eSpiritState.Hold);
            }
        }
    }

    public override void ExitState()
    {
        ((Spirit)me).complete_Unequipt = false;
        me.animCtrl.SetBool("isUnequipt", false);
    }
}
