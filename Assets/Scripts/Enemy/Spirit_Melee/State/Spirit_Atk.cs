using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격패턴 3개 구현하기
//2개는 normalPattern -> 50 : 50
//1개는 피가 30퍼 미만일 떄 ActivePattern -> 30 : 30 : 40 

[System.Serializable]
public enum eSpirit_AtkPattern
{
    None,
    NormalAtk,
    SwingAtk,
    TurnAtt,
    End
}
public class Spirit_Atk : cState
{
    public eSpirit_AtkPattern CurPattern;
    public Quaternion initailAngle;
    public bool startPattern;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Select();
        me.animCtrl.SetBool("isAtk", true);
    }

    public override void UpdateState()
    {
        if (me.weapon == null || CurPattern == eSpirit_AtkPattern.None || CurPattern == eSpirit_AtkPattern.End) return;
        if (((Spirit)me).atting) me.weapon.WeaponColliderOnOff(true);
        else me.weapon.WeaponColliderOnOff(false);
        //if (((Spirit)me).transWeaponPos) me.weapon.TransWeaponPos(me.weapon);
        //else me.weapon.reTurnWeaponPos(me.weapon);

        if (!((Spirit)me).complete_Atk)
        {
            Play(CurPattern);
        }
        else if (((Spirit)me).complete_Atk)
        {
            stop(CurPattern);
            if (me.isAlert)
            {
                ((Spirit)me).complete_Atk = false;
                if (me.distToTarget <= me.status.atkRange)
                {
                    Select();
                }
                else if (me.distToTarget > me.status.atkRange)
                {
                    if (me.distToTarget > 10f) ((Spirit)me).Think_Trace_Dash();
                    else me.SetState((int)Enums.eSpiritState.Trace);
                }
            }
            else
            {
                ((Spirit)me).complete_Atk = false;
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
        }
    }

    public override void ExitState()
    {
        me.animCtrl.SetBool("isAtk", false);
        if(((Spirit)me).atting) ((Spirit)me).atting = false;
    }

    public void Select()
    {
        int AttPatternIndex;
        AttPatternIndex = Random.Range(((int)eSpirit_AtkPattern.None) + 1, ((int)eSpirit_AtkPattern.End));
        CurPattern = (eSpirit_AtkPattern)AttPatternIndex;
    }

    public void Play(eSpirit_AtkPattern CurPattern)
    {
        if (!startPattern)
        {
            me.transform.LookAt(me.targetObj.transform);
            startPattern = true;
        }

        switch (CurPattern)
        {
            case eSpirit_AtkPattern.None:
                break;
            case eSpirit_AtkPattern.NormalAtk:
                PlayNormalAtk();
                break;
            case eSpirit_AtkPattern.SwingAtk:
                PlaySwingAtk();
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
        startPattern = false;

        switch (CurPattern)
        {
            case eSpirit_AtkPattern.None:
                break;
            case eSpirit_AtkPattern.NormalAtk:
                StopNormalAtk();
                break;
            case eSpirit_AtkPattern.SwingAtk:
                StopSwingAtk();
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

    public void PlaySwingAtk()
    {
        if (!me.animCtrl.GetBool("isSwingAtk"))
        {
            me.animCtrl.SetBool("isSwingAtk", true);
        }
    }

    public void StopSwingAtk()
    {
        if (me.animCtrl.GetBool("isSwingAtk"))
        {
            me.animCtrl.SetBool("isSwingAtk", false);
        }
    }

    public void PlayTurnAtk()
    {
        if (!me.animCtrl.GetBool("isTurnAtk"))
        {
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
 