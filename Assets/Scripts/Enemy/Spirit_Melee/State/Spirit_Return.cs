using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Return : cState
{
    public bool Complete_Search;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.MoveStop();
        me.animCtrl.SetBool("isReturn", true);
        me.animCtrl.SetBool("isUnequipt", true);
    }

    public override void UpdateState()
    {
        //if (me.combatState == eCombatState.Alert) me.SetState((int)Enums.eSpiritState.Equipt);

        if (me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("UnEquipt"))
        {
            if (((Spirit)me).isCurrentAnimationOver(me.animCtrl, 1f))
            {
                if (me.animCtrl.GetBool("isUnequipt"))
                {
                    me.animCtrl.SetBool("isUnequipt", false);
                    me.GetComponent<FieldOfView>().viewAngle = ((Spirit)me).initFOVAngle;
                    Complete_Search = true;
                }
            }
        }

        if(Complete_Search)
        {
            if(Vector3.Distance(me.transform.position,((Spirit)me).respawnPos) > 0.5f)
            {
                me.MoveOrder(((Spirit)me).respawnPos);
                me.animCtrl.SetBool("isWalk", true);
                me.animCtrl.SetBool("isPatrol", true);
            }
            else
            {
                Debug.Log("µµÂø!!!!!!!!!");
                me.MoveStop();
                ((Spirit)me).isReturn = false;
                me.animCtrl.SetBool("isWalk", false);
                me.animCtrl.SetBool("isPatrol", false);
                Complete_Search = false;
                me.SetState((int)Enums.eSpiritState.Idle);
            }
        }
    }
    public override void ExitState()
    {
        me.animCtrl.SetBool("isReturn", false);
        me.animCtrl.SetBool("isWalk", false);
        me.animCtrl.SetBool("isPatrol", false);
        Complete_Search = false;
    }



}
