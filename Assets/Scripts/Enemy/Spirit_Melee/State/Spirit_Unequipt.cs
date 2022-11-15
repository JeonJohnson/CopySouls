using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Unequipt : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Debug.Log("�������");
        ((Spirit)me).isEquipt = false;
        me.weaponEquipState = eEquipState.UnEquip;
        me.MoveStop();
        me.animCtrl.SetBool("isUnequipt", true);
    }

    public override void UpdateState()
    {
        if (me.status.isBackHold)
        {
            me.SetState((int)Enums.eSpiritState.Hold);
        }

        if (!me.status.isBackHold && !me.status.isFrontHold)
        {
            if (me.HitCount > 0)
            {
                if (((Spirit)me).curState_e != Enums.eSpiritState.Damaged)
                {
                    me.SetState((int)Enums.eSpiritState.Damaged);
                }
            }
        }

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
            if (me.combatState == eCombatState.Alert)
            {
                me.SetState((int)Enums.eSpiritState.Equipt);
            }

            
        }
    }

    public override void ExitState()
    {
        ((Spirit)me).complete_Unequipt = false;
        me.animCtrl.SetBool("isUnequipt", false);
    }
}
