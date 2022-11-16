using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Damaged : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isDamaged", true);
        me.animCtrl.SetBool("ChangeDamaged", true);
    }

    public override void UpdateState()
    {
        if (me.status.isBackHold)
        {
            me.SetState((int)Enums.eSpiritState.Hold);
        }


        //if (me.status.isBackHold)
        //{
        //    //me.SetState((int)Enums.eSpiritState.Hold);
        //    Debug.Log("쳐맞는 도중에 잡기가 어캐 드옴?");
        //    me.status.isBackHold = false;
        //}
        //else if(me.status.isFrontHold)
        //{
        //    Debug.Log("쳐맞는 도중에 잡기가 어캐 드옴?");
        //    me.status.isFrontHold = false;
        //}

        //if (me.status.isBackHold)
        //{
        //    me.animCtrl.SetBool("ChangeDamaged", false);
        //    me.animCtrl.SetBool("isDamaged", false);
        //    ((Spirit)me).complete_Damaged = false;
        //    ((Spirit)me).HitCount = 0;
        //    me.SetState((int)Enums.eSpiritState.Hold);
        //}

        if (((Spirit)me).HitCount > 1)
        {
            if (me.animCtrl.GetBool("isDamaged")) me.animCtrl.SetBool("isDamaged", false);
            else me.animCtrl.SetBool("isDamaged", true);
            ((Spirit)me).HitCount--;
        }
        else
        {
            if (((Spirit)me).complete_Damaged)
            {
                ((Spirit)me).HitCount--;
                me.animCtrl.SetBool("ChangeDamaged", false);

                if (((Spirit)me).isReturn) me.SetState((int)Enums.eSpiritState.Return);

                if (me.combatState == eCombatState.Alert)
                {
                    if (me.weaponEquipState == eEquipState.Equip)
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
                    else me.SetState((int)Enums.eSpiritState.Equipt);
                }
                else me.SetState((int)Enums.eSpiritState.Unequipt);
            }
        }
    }

    public override void ExitState()
    {
        me.animCtrl.SetBool("ChangeDamaged", false);
        me.animCtrl.SetBool("isDamaged", false);
        ((Spirit)me).complete_Damaged = false;
        ((Spirit)me).HitCount = 0;
    }
}
