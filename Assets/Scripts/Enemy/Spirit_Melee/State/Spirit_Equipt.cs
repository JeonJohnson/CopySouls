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
        me.animCtrl.SetBool("isEquipt", true);
        me.MoveStop();
        ((Spirit)me).isEquipt = true;
    }

    public override void UpdateState()
    {
        if (((Spirit)me).complete_Equipt)
        {
            if(me.targetObj == null)
            {
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
            else
            {
                if (me.distToTarget <= me.status.atkRange)
                {
                    me.SetState((int)Enums.eSpiritState.Atk);
                }
                else
                {
                    me.SetState((int)Enums.eSpiritState.Trace);
                }
            }
        }
    }

    public override void ExitState()
    {
        ((Spirit)me).complete_Equipt = false;
        me.animCtrl.SetBool("isEquipt", false);
    }


    public void Spirit_StartEquiptment()
    {
    }

    
 
}
