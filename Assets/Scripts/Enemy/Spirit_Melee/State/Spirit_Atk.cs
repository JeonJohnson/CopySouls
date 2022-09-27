using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Atk : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Spirit_StartAtk();
        Spirit_PlayAtkAnimation();
    }
    public override void UpdateState()
    {
        //if(공격모션이 끝난 후에 상태전환)

        //공격중에는 회전 불가
        me.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        if (me.distToPlayer > me.status.atkRange)
        {
            me.SetState((int)Enums.eSpiritState.Trace);
        }
        else if(me.distToPlayer > me.status.ricognitionRange)
        {
            me.SetState((int)Enums.eSpiritState.Patrol);
        }
    }

    public override void ExitState()
    {
        Spirit_StopAtk();
        Spirit_StopAtkAnimation();
    }











    public void Spirit_StartAtk()
    {
        me.animCtrl.SetBool("isAtk", true);
        me.navAgent.isStopped = true;
    }

    public void Spirit_StopAtk()
    {
        me.animCtrl.SetBool("isAtk", false);
        me.navAgent.isStopped = true;
    }

    public void Spirit_PlayAtkAnimation()
    {
        if (!((Spirit)me).isMoving) me.animCtrl.SetTrigger("isSlash");
    }

    public void Spirit_StopAtkAnimation()
    {
    }

}
