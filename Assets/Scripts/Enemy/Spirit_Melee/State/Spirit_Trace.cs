using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Trace : cState
{
    public int count;

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
            if (me.navAgent.isStopped) me.navAgent.isStopped = false;
        }

        if (me.distToPlayer <= me.status.atkRange)
        {
            me.SetState((int)Enums.eSpiritState.Atk);
        }
        if (me.distToPlayer > me.status.ricognitionRange)
        {
            me.SetState((int)Enums.eSpiritState.Patrol);
        }
    }

    public override void ExitState()
    {
        Spirit_StopTraceAnimation();
        Spirit_StopTrace();
    }









    public void Spirit_StartTrace()
    {
        //me.navAgent.isStopped = true;
       
        CoroutineHelper.Instance.StartCoroutine(((Spirit)me).DelayNavOn());
        CoroutineHelper.Instance.StopCoroutine(((Spirit)me).DelayNavOn());
        me.navAgent.speed = me.status.runSpd;
        //me.navAgent.isStopped = false;

    }

    public void Spirit_StopTrace()
    {
        me.targetObj = null;
        me.animCtrl.SetBool("isTrace", false);
        me.navAgent.isStopped = true;
    }

    public void Spirit_PlayTraceAnimation()
    {
        if (((Spirit)me).isMoving) me.animCtrl.SetBool("isRun", true);
        else me.animCtrl.SetBool("isRun", false);
    }
    public void Spirit_StopTraceAnimation()
    {
        me.animCtrl.SetBool("isRun", false);
    }





}
