using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Groggy : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isGroggy",true);
        ((Spirit)me).weapon.WeaponColliderOnOff(false);
    }
    public override void UpdateState()
    {
        if (((Spirit)me).complete_Groggy)
        {
            ((Spirit)me).complete_Groggy = false;

            if (me.combatState == eCombatState.Alert)
            {
                if (me.distToTarget <= me.status.atkRange)
                {
                    me.SetState((int)Enums.eSpiritState.Atk);
                }
                else if (me.distToTarget > me.status.atkRange && me.distToTarget <= me.status.ricognitionRange)
                {
                    me.SetState((int)Enums.eSpiritState.Trace);
                }
            }
            else
            {
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
        }
        else
        {
            if(me.status.isFrontHold || me.status.isBackHold)
            {
                me.SetState((int)Enums.eSpiritState.Hold);
            }
        }
    }

    public override void ExitState()
    {
        me.animCtrl.SetBool("isGroggy", false);
        me.status.isGroggy = false;
    }
}
