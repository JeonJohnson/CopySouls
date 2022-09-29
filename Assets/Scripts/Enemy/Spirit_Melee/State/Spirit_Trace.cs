using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Trace : cState
{

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Spirit_StartTrace();
    }

    public override void UpdateState()
    {
        Spirit_PlayTraceAnimation();

        //느리게 회전 추가
        if (me.targetObj != null)
        {
            me.curTargetPos = me.targetObj.transform.position;
            me.transform.LookAt(me.targetObj.transform);
        }

       if (me.distToPlayer <= me.status.atkRange)
       {
           me.SetState((int)Enums.eSpiritState.Atk);
       }

       if (me.distToPlayer > me.status.ricognitionRange)
       {
           me.SetState((int)Enums.eSpiritState.Unequipt);
       }
    }

    public override void ExitState()
    {
        Spirit_StopTraceAnimation();
        Spirit_StopTrace();
    }









    public void Spirit_StartTrace()
    {
        me.targetObj = me.player;
        me.navAgent.isStopped = false;
        me.animCtrl.SetBool("isTrace", true);
        me.animCtrl.SetBool("isRun", true);
        me.navAgent.speed = me.status.runSpd;
    }

    public void Spirit_StopTrace()
    {
        me.animCtrl.SetBool("isTrace", false);
        me.navAgent.isStopped = true;
    }

    public void Spirit_PlayTraceAnimation()
    {
    }
    public void Spirit_StopTraceAnimation()
    {
        me.animCtrl.SetBool("isRun", false);
    }





}
