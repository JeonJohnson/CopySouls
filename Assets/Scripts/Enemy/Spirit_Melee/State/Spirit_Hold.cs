using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Hold : cState
{
    public bool complete_Hold;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Debug.Log("¿‚±‚!!");
        me.status.isBackHold = false;
        me.status.isFrontHold = false;
        me.animCtrl.SetBool("isHold", true);
        if (me.status.isBackHold) me.animCtrl.SetFloat("AttIndex", 0);
        else if (me.status.isFrontHold) me.animCtrl.SetFloat("AttIndex", 1);
    }

    public override void UpdateState()
    {
        me.MoveStop();
        if (me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Hold"))
        {
            if (((Spirit)me).isCurrentAnimationOver(me.animCtrl, 1f))
            {
                if(me.animCtrl.GetBool("isHold"))
                {
                    me.animCtrl.SetBool("isHold", false);
                    complete_Hold = true;
                }
            }
        }

        if (!me.animCtrl.GetBool("isHold"))
        {
            if (me.status.curHp <= 0) me.status.isDead = true;
            if (me.status.isDead)
            {
                ((Spirit)me).CreateRemainderWeapon(((Spirit)me).weapon.transform);
                ((Spirit)me).ChangeToRagDoll();
            }
            else
            {
                me.animCtrl.SetBool("isGetUp",true);
            }
        }

        if (me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("GetUp"))
        {
            if (((Spirit)me).isCurrentAnimationOver(me.animCtrl, 1f))
            {
                if (me.animCtrl.GetBool("isGetUp"))
                {
                    me.animCtrl.SetBool("isGetUp", false);
                }
            }
        }

        if (complete_Hold && !me.animCtrl.GetBool("isGetUp"))
        {
            if (me.weaponEquipState == eEquipState.Equip)
            {
                if (me.distToTarget <= me.status.atkRange)
                {
                    me.SetState((int)Enums.eSpiritState.Atk);
                }
                else if (me.distToTarget > me.status.atkRange && me.distToTarget <= me.status.ricognitionRange)
                {
                    me.SetState((int)Enums.eSpiritState.Trace);
                }
                else
                {
                    me.SetState((int)Enums.eSpiritState.Unequipt);
                }
            }
            else
            {
                if (me.distToTarget <= me.status.ricognitionRange)
                {
                    me.SetState((int)Enums.eSpiritState.Equipt);
                }
                else
                {
                    me.SetState((int)Enums.eSpiritState.Unequipt);
                }
            }
        }
    }

    public override void ExitState()
    {
        
        complete_Hold = false;
    }
}
