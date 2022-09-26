using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Trace : cState
{


    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        CoroutineHelper.Instance.StartCoroutine(StartTrace());
        me.animCtrl.SetBool("isRun", true);
        me.navAgent.isStopped = false;
    }

    public override void UpdateState()
    {
        me.curTargetPos = me.player.transform.position;
        me.transform.LookAt(me.curTargetPos);

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
        StopTrace();
    }

    public IEnumerator StartTrace()
    {
        yield return new WaitForSeconds(1.0f);
        me.animCtrl.SetTrigger("isTrace");
        yield return new WaitForSeconds(1.0f);
        me.animCtrl.SetBool("isRun", true);
        me.navAgent.isStopped = false;
    }

    public void StopTrace()
    {
        CoroutineHelper.Instance.StopCoroutine(StartTrace());
        me.animCtrl.SetBool("isRun", false);
        me.navAgent.isStopped = true;
    }

}
