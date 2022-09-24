using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Patrol : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        CoroutineHelper.Instance.StartCoroutine(RandomTargetPos(5.0f,2.0f));
    }

    public override void UpdateState()
    {
        if (me.distToPlayer <= me.status.ricognitionRange)
        {
            me.SetState(Enums.eEnmeyState.Trace);
        }
    }

    public override void ExitState()
    {
        CoroutineHelper.Instance.StopCoroutine(RandomTargetPos(5.0f, 2.0f));
        me.animCtrl.SetBool("isWalk", false);
    }

    public Vector3 ThinkTargetPos()
    {
        int randX = Random.Range(-1, 2) * 10;
        int randZ = Random.Range(-1, 2) * 10;
        if (randX == 0 && randZ == 0) ThinkTargetPos();
        me.curTargetPos = new Vector3(me.transform.position.x + randX, 0, me.transform.position.z + randZ);
        if (me.curTargetPos == me.preTargetPos) ThinkTargetPos();
        if (Mathf.Abs(me.curTargetPos.x - me.preTargetPos.x) < 5 && Mathf.Abs(me.curTargetPos.y - me.preTargetPos.y) < 5) ThinkTargetPos();
        return me.curTargetPos;
    }

    public IEnumerator RandomTargetPos(float delayTime, float startTime)
    {
        me.preTargetPos = me.curTargetPos;
        me.navAgent.isStopped = false;
        me.animCtrl.SetBool("isWalk", true);
        me.curTargetPos = ThinkTargetPos();
        yield return new WaitForSeconds(delayTime);
        me.navAgent.isStopped = true;
        me.animCtrl.SetBool("isWalk", false);
        yield return new WaitForSeconds(startTime);
        CoroutineHelper.Instance.StartCoroutine(RandomTargetPos(5.0f, 2.0f));
    }

}
