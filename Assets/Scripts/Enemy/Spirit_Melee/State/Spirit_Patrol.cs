using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Patrol : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Spirit_StartPatrol();
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
        Spirit_StopPatrol();
        Spirit_StopPatrolAnimation();
    }


    








    public void Spirit_StartPatrol()
    {
        me.animCtrl.SetBool("isPatrol", true);
        CoroutineHelper.Instance.StartCoroutine(((Spirit)me).AutoMove(5.0f,2.0f));
    }

    public void Spirit_StopPatrol()
    {
        me.animCtrl.SetBool("isPatrol", false);
        CoroutineHelper.Instance.StopCoroutine(((Spirit)me).AutoMove(5.0f, 2.0f));
        me.curTargetPos = Vector3.zero;
        me.navAgent.isStopped = true;
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
