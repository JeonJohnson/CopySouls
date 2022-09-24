using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_Idle : cState
{

    IEnumerator testCour()
    {

        yield return null;
        
    }

    IEnumerator testcoroutine;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);

        testcoroutine = testCour();
    }

    public override void UpdateState()
    {
        if (me.distToPlayer < me.status.patrolRange)
        {
            me.SetState(Enums.eEnmeyState.Patrol);
        }

        CoroutineHelper.Instance.StartCoroutine(testCour());
        CoroutineHelper.Instance.StopCoroutine(testcoroutine);
    }

    public override void ExitState()
    {

    }

}
