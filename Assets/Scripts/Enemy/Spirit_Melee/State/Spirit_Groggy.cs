using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Groggy : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.HitCount = 0;
        me.animCtrl.SetBool("isGroggy",true);
        ((Spirit)me).weapon.WeaponColliderOnOff(false);

        CameraShake.Instance.AddShakeEvent(((Spirit)me).data);

    }
    public override void UpdateState()
    {
        //if (((Spirit)me).isReset) me.SetState((int)Enums.eSpiritState.Idle);

        if (me.status.isFrontHold)
        {
            me.SetState((int)Enums.eSpiritState.Hold);
        }
        else if (me.status.isBackHold)
        {
            me.SetState((int)Enums.eSpiritState.Hold);
        }
        else if(!me.status.isBackHold && !me.status.isFrontHold)
        {
            if (me.HitCount > 0)
            {
                if (((Spirit)me).curState_e != Enums.eSpiritState.Damaged)
                {
                    me.SetState((int)Enums.eSpiritState.Damaged);
                }
            }
        }

        if (((Spirit)me).complete_Groggy)
        {
            ((Spirit)me).complete_Groggy = false;

            if (((Spirit)me).isReturn) me.SetState((int)Enums.eSpiritState.Return);

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
            
        }
    }

    public override void ExitState()
    {
        me.animCtrl.SetBool("isGroggy", false);
        me.status.isGroggy = false;
        ((Spirit)me).complete_Groggy = false;
    }
}
