using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Trace -> Unquipt
//Trace -> Att

public class Spirit_Trace : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isTrace", true);
        me.animCtrl.SetBool("isRun", true);
        me.navAgent.speed = me.status.runSpd;
        me.MoveOrder(me.targetObj.transform.position);
    }

    public override void UpdateState()
    {
        //me.transform.LookAt(me.targetObj.transform);
        me.SetDestination(me.targetObj.transform.position);

        if(me.status.isBackHold)
        {
            me.SetState((int)Enums.eSpiritState.Hold);
        }

        if (me.combatState == eCombatState.Alert)
        {
            if (me.distToTarget <= me.status.atkRange)
            {
                me.SetState((int)Enums.eSpiritState.Atk);
            }
            else if (me.distToTarget > me.status.ricognitionRange)
            {
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
        }
        else me.SetState((int)Enums.eSpiritState.Unequipt);
    }

    public override void LateUpdateState()
    {
        //hip
        //head

    }


    public override void ExitState()
    {
        me.animCtrl.SetBool("isTrace", false);
        me.animCtrl.SetBool("isRun", false);
        me.MoveStop();
    }
}
