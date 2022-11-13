using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Idle : cState
{
    public float curTime = 0;
    public float patrolWaitTime = 7f;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.MoveStop();
        me.animCtrl.SetBool("isIdle", true);
    }

    public override void UpdateState()
    {
        if (me.combatState == eCombatState.Alert) me.SetState((int)Enums.eSpiritState.Equipt);
        curTime += Time.deltaTime;
        if (curTime >= patrolWaitTime)
        {
            curTime = 0;
            me.SetState((int)Enums.eSpiritState.Patrol);
        }

        if (me.status.isBackHold)
        {
            me.SetState((int)Enums.eSpiritState.Hold);
        }
    }

    public override void ExitState()
    {
        me.animCtrl.SetBool("isIdle", false);
    }
}
