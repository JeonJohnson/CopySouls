using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Dash : cState
{
    public float maxDashSpeed = 3f;
    public float curDashSpeed = 1f;
    public float timer;
    public bool startDash;
    public bool completeDash;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isDash", true);
    }

    public override void UpdateState()
    {
        if (me.animCtrl.GetLayerWeight(10) < 0.7f) ((Spirit)me).dashCol.enabled = false;
        else ((Spirit)me).dashCol.enabled = true;

        Dash();
        if (completeDash)
        {
            if (me.isAlert)
            {
                if (me.distToTarget <= me.status.atkRange)
                {
                    me.SetState((int)Enums.eSpiritState.Atk);
                }
                else if (me.distToTarget > me.status.atkRange)
                {
                    if (me.distToTarget > 10f) ((Spirit)me).Think_Trace_Dash();
                    else me.SetState((int)Enums.eSpiritState.Trace);
                }
            }
            else
            {
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
        }
    }

    public override void ExitState()
    {
        completeDash = false;
        me.animCtrl.SetBool("isDash", false);
        ((Spirit)me).dashCol.enabled = false;
    }

    public void Dash()
    {
        if (!startDash)
        {
            me.transform.LookAt(me.targetObj.transform);
            startDash = true;
        }

        timer += Time.deltaTime;
        if (timer >= ((Spirit)me).dashTime)
        {
            me.rd.velocity = Vector3.zero;
            timer = 0;
            startDash = false;
            completeDash = true;
        }
        else
        {
            if (startDash)
            {
                Vector3 Dir = me.transform.forward;
                if (curDashSpeed <= maxDashSpeed) Dir = Dir.normalized * (curDashSpeed + timer);
                else Dir = Dir.normalized * maxDashSpeed;
                me.transform.position += Dir * Time.deltaTime;
            }
        }
    }
}
