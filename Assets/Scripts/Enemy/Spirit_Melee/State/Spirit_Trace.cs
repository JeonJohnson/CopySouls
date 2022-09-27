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

        me.curTargetPos = me.player.transform.position;

        //느리게 회전 추가
        me.transform.LookAt(me.targetObj.transform);

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
        me.targetObj = me.player;
        me.animCtrl.SetBool("isTrace", true);
        me.navAgent.speed = me.status.runSpd;
        CoroutineHelper.Instance.StartCoroutine(((Spirit)me).EquiptAndTargetSelction());
    }

    public void Spirit_StopTrace()
    {
        me.targetObj = null;
        me.animCtrl.SetBool("isTrace", false);
        CoroutineHelper.Instance.StopCoroutine(((Spirit)me).EquiptAndTargetSelction());
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
