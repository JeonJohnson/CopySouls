using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Unequipt : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        ((Spirit)me).isEquipt = false;
        me.MoveStop();
        me.animCtrl.SetBool("isUnequipt", true);
    }

    public override void UpdateState()
    {
        if (((Spirit)me).complete_Unequipt)
        {
            me.GetComponent<FieldOfView>().viewAngle = ((Spirit)me).initFOVAngle;
            if(me.isAlert)
            {
                me.SetState((int)Enums.eSpiritState.Equipt);
            }
            else
            {
                me.SetState((int)Enums.eSpiritState.Idle);
            }
        }
    }

    public override void ExitState()
    {
        ((Spirit)me).complete_Unequipt = false;
        me.animCtrl.SetBool("isUnequipt", false);
    }
}
