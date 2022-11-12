using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Equipt -> Unequipt(범위벗어남) -> Idle
//Equipt -> Trace(장비후 추격)
//Equipt -> Att

public class Spirit_Equipt : cState
{

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Debug.Log("장비!!");
        me.MoveStop();
        ((Spirit)me).isEquipt = true;
        me.weaponEquipState = eEquipState.Equip;
        me.GetComponent<FieldOfView>().viewAngle = 360f;
        me.animCtrl.SetBool("isEquipt", true);
    }

    public override void UpdateState()
    {
        me.MoveStop();
        if (((Spirit)me).complete_Equipt)
        {
            if (me.combatState == eCombatState.Alert)
            {
                me.animCtrl.SetBool("isEquipt", false);
                if (me.distToTarget <= me.status.atkRange)
                {
                    me.SetState((int)Enums.eSpiritState.Atk);
                }
                else if (me.distToTarget > me.status.atkRange && me.distToTarget <= me.status.ricognitionRange)
                {
                    me.SetState((int)Enums.eSpiritState.Trace);
                }
            }
            else me.SetState((int)Enums.eSpiritState.Unequipt);
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
        ((Spirit)me).complete_Equipt = false;
        me.animCtrl.SetBool("isEquipt", false);
    }
}
