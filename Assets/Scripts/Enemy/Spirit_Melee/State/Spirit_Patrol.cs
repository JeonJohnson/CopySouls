using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Patrol : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        CoroutineHelper.Instance.StartCoroutine(((Spirit)me).MoveToTargetPos(5.0f,2.0f));
    }

    public override void UpdateState()
    {
        Spirit_PlayPatrolAnimation();

        if (me.distToPlayer <= me.status.ricognitionRange)
        {
            me.SetState((int)Enums.eSpiritState.Trace);
        }
    }

    public override void ExitState()
    {
        CoroutineHelper.Instance.StopCoroutine(((Spirit)me).MoveToTargetPos(5.0f, 2.0f));
        Spirit_StopPatrolAnimation();
    }

    public void Spirit_PlayPatrolAnimation()
    {
        if (((Spirit)me).isMoving) me.animCtrl.SetBool("isWalk", true);
        else me.animCtrl.SetBool("isWalk", false);
    }
    public void Spirit_StopPatrolAnimation()
    {
        me.animCtrl.SetBool("isWalk", false);
    }

}
