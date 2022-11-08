using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Damaged : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isDamaged", true);
    }

    //연속 데미지 주기
    //애니메이션 바꾸기

    public override void UpdateState()
    {
        if(((Spirit)me).complete_Damaged)
        {
            if (me.isAlert)
            {
                if (((Spirit)me).isEquipt)
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
                    me.SetState((int)Enums.eSpiritState.Equipt);
                }
            }
            else 
            {
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
        }
    }

    public override void ExitState()
    {
        me.animCtrl.SetBool("isDamaged", false);
        ((Spirit)me).complete_Damaged = false;
    }
}
