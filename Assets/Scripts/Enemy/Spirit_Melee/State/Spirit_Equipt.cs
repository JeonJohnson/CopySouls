using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Equipt : cState
{

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Spirit_StartEquiptment();
    }

    public override void UpdateState()
    {
        //me.rd.velocity = Vector3.zero;
        
        //221002 20:23 player -> targetObj
        if (((Spirit)me).complete_Equipt)
        {
            if (me.distToTarget <= me.status.ricognitionRange)
            {
                me.SetState((int)Enums.eSpiritState.Trace);
            }
            else if(me.distToTarget <= me.status.atkRange)
            {
                me.SetState((int)Enums.eSpiritState.Atk);
            }
            else if(me.distToTarget > me.status.ricognitionRange)
            {
                //
                me.SetState((int)Enums.eSpiritState.Unequipt);
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
        me.navAgent.isStopped = true;
        me.animCtrl.SetBool("isEquipt", true);
    }

    
 
}
