using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum eSpirit_AtkPattern
{
    NormalAtk,
    DoubleAtk,
    TurnAtt,
    End
}
public class Spirit_Atk : cState
{
    public eSpirit_AtkPattern CurPattern;
    public bool startPattern;
    public int AttIndex;
    public bool isOver;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        ((Spirit)me).complete_Atk = true;
        me.animCtrl.SetBool("isAtk", true);
    }

    //앞잡시 배찔리는거
    //뒤잡시 등찔리고 앞으로 넘어짐

    public override void UpdateState()
    {
        if (((Spirit)me).weapon == null) return;
        if (((Spirit)me).atting) ((Spirit)me).weapon.WeaponColliderOnOff(true);
        else ((Spirit)me).weapon.WeaponColliderOnOff(false);

        if (!((Spirit)me).complete_Atk)
        {
            stop(CurPattern);
            if (me.status.isGroggy) me.SetState((int)Enums.eSpiritState.Groggy);
            else Play(CurPattern);
        }
        else if(((Spirit)me).complete_Atk)
        {
            stop(CurPattern);
            ((Spirit)me).complete_Atk = false;
            if (me.combatState == eCombatState.Alert)
            {
                if (me.distToTarget <= me.status.atkRange) Select();
                else if (me.distToTarget > me.status.atkRange) me.SetState((int)Enums.eSpiritState.Trace);
            }
            else me.SetState((int)Enums.eSpiritState.Unequipt);
        }


    }

    public override void ExitState()
    {
        
        startPattern = false;
        me.animCtrl.SetBool("isAtk", false);
        if(((Spirit)me).atting) ((Spirit)me).atting = false;


        ((Spirit)me).weapon.ReturnWeaponPos();
    }

    public void Select()
    {
        if (!startPattern)
        {
            int AttPatternIndex;
            AttPatternIndex = Random.Range(((int)eSpirit_AtkPattern.NormalAtk), ((int)eSpirit_AtkPattern.End));
            AttIndex = AttPatternIndex;

            CurPattern = (eSpirit_AtkPattern)AttPatternIndex;
            me.transform.LookAt(me.targetObj.transform);

            if (CurPattern == eSpirit_AtkPattern.DoubleAtk || CurPattern == eSpirit_AtkPattern.TurnAtt)  ((Spirit)me).weapon.TransWeaponPos();
            else ((Spirit)me).weapon.ReturnWeaponPos();

            startPattern = true;
        }
    }
   
    public void Play(eSpirit_AtkPattern CurPattern)
    {
       switch (CurPattern)
       {
           case eSpirit_AtkPattern.NormalAtk:
               PlayNormalAtk();
               break;
           case eSpirit_AtkPattern.DoubleAtk:
               PlayDoubleAtk();
               break;
           case eSpirit_AtkPattern.TurnAtt:
               PlayTurnAtk();
               break;
           case eSpirit_AtkPattern.End:
               break;
           default:
               break;
       }
    }

    public void stop(eSpirit_AtkPattern curPattern)
    {
        if (startPattern) startPattern = false;

        switch (CurPattern)
        {
            case eSpirit_AtkPattern.NormalAtk:
                StopNormalAtk();
                break;
            case eSpirit_AtkPattern.DoubleAtk:
                StopDoubleAtk();
                break;
            case eSpirit_AtkPattern.TurnAtt:
                StopTurnAtk();
                break;
            case eSpirit_AtkPattern.End:
                break;
            default:
                break;
        }
    }

    public void PlayNormalAtk()
    {
        if (!me.animCtrl.GetBool("isNormalAtk"))
        {
            me.animCtrl.SetFloat("AttIndex", AttIndex);
            me.animCtrl.SetBool("isNormalAtk", true);
        }
    }

    public void StopNormalAtk()
    {
        if (me.animCtrl.GetBool("isNormalAtk"))
        {
            me.animCtrl.SetBool("isNormalAtk", false);
        }
    }

    public void PlayDoubleAtk()
    {
        if (!me.animCtrl.GetBool("isDoubleAtk"))
        {
            me.animCtrl.SetFloat("AttIndex", AttIndex);
            me.animCtrl.SetBool("isDoubleAtk", true);
        }
    }

    public void StopDoubleAtk()
    {
        if (me.animCtrl.GetBool("isDoubleAtk"))
        {
            me.animCtrl.SetBool("isDoubleAtk", false);
        }
    }

    public void PlayTurnAtk()
    {
        if (!me.animCtrl.GetBool("isTurnAtk"))
        {
            me.animCtrl.SetFloat("AttIndex", AttIndex);
            me.animCtrl.SetBool("isTurnAtk", true);
        }
    }

    public void StopTurnAtk()
    {
        if (me.animCtrl.GetBool("isTurnAtk"))
        {
            me.animCtrl.SetBool("isTurnAtk", false);
        }
    }
}
 