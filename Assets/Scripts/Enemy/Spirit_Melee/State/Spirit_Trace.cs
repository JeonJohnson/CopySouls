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
        Spirit_StartTrace();
    }

    public override void UpdateState()
    {
        me.SetDestination(me.targetObj.transform.position);
        me.transform.LookAt(me.targetObj.transform);

        if(me.combatState == eCombatState.Alert)
        {
            if (me.distToTarget <= me.status.atkRange)
            {
                me.SetState((int)Enums.eSpiritState.Atk);
            }
        }
        else
        {
            me.SetState((int)Enums.eSpiritState.Unequipt);
        }

        if (me.distToTarget > me.status.ricognitionRange || me.combatState != eCombatState.Alert)
        {
        }
    }

    public override void ExitState()
    {
        Spirit_StopTraceAnimation();
        Spirit_StopTrace();
    }









    public void Spirit_StartTrace()
    {
        me.animCtrl.SetBool("isTrace", true);
        me.animCtrl.SetBool("isRun", true);
        me.navAgent.speed = me.status.runSpd;
        me.MoveOrder(me.targetObj.transform.position);
    }

    public void Spirit_StopTrace()
    {
        me.animCtrl.SetBool("isTrace", false);
        me.MoveStop();
    }

    public void Spirit_PlayTraceAnimation()
    {
    }
    public void Spirit_StopTraceAnimation()
    {
        me.animCtrl.SetBool("isRun", false);
    }





}
