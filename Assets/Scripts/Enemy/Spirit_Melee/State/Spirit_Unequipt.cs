using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Unequipt : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        //me.GetComponent<FieldOfView>().viewAngle = ((Spirit)me).initFOVAngle;
        me.MoveStop();
        me.animCtrl.SetBool("isUnequipt", true);
    }

    public override void UpdateState()
    {
        if (((Spirit)me).complete_Unequipt)
        {
            me.SetState((int)Enums.eSpiritState.Idle);
            //if (me.distToTarget <= me.status.ricognitionRange)
            //{
            //    me.SetState((int)Enums.eSpiritState.Equipt);
            //}
            //if (me.isAlert)
            //{
            //    me.SetState((int)Enums.eSpiritState.Equipt);
            //}
        }
    }

    public override void ExitState()
    {
        ((Spirit)me).complete_Unequipt = false;
        me.animCtrl.SetBool("isUnequipt", false);
    }
}
