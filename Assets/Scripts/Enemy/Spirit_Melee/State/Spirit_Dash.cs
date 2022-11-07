using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Dash : cState
{
    public float maxDashSpeed = 10f;
    public float curDashSpeed = 5f;
    public float timer;
    public bool completeDash;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.transform.LookAt(me.targetObj.transform);
        me.animCtrl.SetBool("isDash", true);
    }

    public override void UpdateState()
    {
        if (me.animCtrl.GetLayerWeight(10) < 0.7f) ((Spirit)me).dashCol.enabled = false;
        else ((Spirit)me).dashCol.enabled = true;
        Dash();
    }

    public override void ExitState()
    {
        timer = 0;
        me.animCtrl.SetBool("isDash", false);
        ((Spirit)me).dashCol.enabled = false;
    }

    public void Dash()
    {
        timer += Time.deltaTime;
        if (timer >= ((Spirit)me).dashTime)
        {
            timer = 0;

            if (me.isAlert)
            {
                if (me.distToTarget <= me.status.atkRange)
                {
                    me.SetState((int)Enums.eSpiritState.Atk);
                }
                else if (me.distToTarget > me.status.atkRange)
                {
                    me.SetState((int)Enums.eSpiritState.Trace);
                }
            }
            else
            {
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
        }
        else
        {
            Vector3 Dir = me.transform.forward;
            if (curDashSpeed <= maxDashSpeed) Dir = Dir.normalized * (curDashSpeed + timer);
            else Dir = Dir.normalized * maxDashSpeed;
            me.transform.position += Dir * Time.deltaTime;
        }
    }
}
