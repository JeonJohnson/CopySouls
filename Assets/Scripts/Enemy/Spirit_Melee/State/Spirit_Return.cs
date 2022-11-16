using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Return : cState
{
    public bool Complete_Schouting;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.MoveStop();
        me.animCtrl.SetBool("isReturn", true);
        me.GetComponent<FieldOfView>().viewAngle = ((Spirit)me).initFOVAngle;
    }

    public override void UpdateState()
    {
        //if (me.combatState == eCombatState.Alert) me.SetState((int)Enums.eSpiritState.Equipt);

        //함 쨰리보고 본래 자리로 돌아가서 idle

        if (me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("ReturnStart"))
        {
            if (((Spirit)me).isCurrentAnimationOver(me.animCtrl, 1f))
            {
                if (!me.animCtrl.GetBool("isSchouting"))
                {
                    me.animCtrl.SetBool("isSchouting", true);
                    Complete_Schouting = true;
                }
            }
        }

        if(Complete_Schouting)
        {
            if(Vector3.Distance(me.transform.position,((Spirit)me).respawnPos) > 0.5f)
            {
                Debug.Log("도착 못함");
                me.MoveOrder(((Spirit)me).respawnPos);
            }
            else if(Vector3.Distance(me.transform.position, ((Spirit)me).respawnPos) <= 0.5f || me.transform.position == ((Spirit)me).respawnPos)
            {
                Debug.Log("도착!!!!!!!!!");
                me.MoveStop();
                me.animCtrl.SetBool("isReturn", false);
                ((Spirit)me).isReturn = false;
                Complete_Schouting = false;
                me.SetState((int)Enums.eSpiritState.Idle);
            }
        }
    }
    public override void ExitState()
    {
        me.animCtrl.SetBool("isReturn", false);
        Complete_Schouting = false;
    }



}
