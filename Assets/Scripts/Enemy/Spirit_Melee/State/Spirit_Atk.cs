using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum eSpirit_AtkPattern
{
    NoramlAtk,
    SpinAtk,
    ConsecutivelyAtk,
    Strafe_Right,
    Strafe_Left,
    Strafe_Back,
    End
}

public class Spirit_Atk : cState
{
    public eSpirit_AtkPattern curAtkPattern;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Spirit_StartAtk();
        Spirit_SelectAtkPattern();
    }
    public override void UpdateState()
    {
        //me.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        if (((Spirit)me).complete_Atk)
        {
            ((Spirit)me).complete_Atk = false;
            //221002 20:23 player -> targetObj
            if (me.distToTarget > me.status.atkRange && me.distToTarget <= me.status.ricognitionRange)
            {
                me.SetState((int)Enums.eSpiritState.Trace);
            }
            else if (me.distToTarget > me.status.ricognitionRange)
            {
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
            else
            {
                Spirit_SelectAtkPattern();
            }
        }
    }

    public override void ExitState()
    {
        Spirit_StopAtk();
    }







    public void Spirit_StartAtk()
    {
        me.animCtrl.SetBool("isAtk", true);
        me.navAgent.isStopped = true;
    }

    public void Spirit_StopAtk()
    {
        me.animCtrl.SetBool("isAtk", false);
    }



    public eSpirit_AtkPattern GetCurAtkPattern()
    {
        return curAtkPattern;
    }

    public void Spirit_SelectAtkPattern()
    {
        if (me.status.curHp >= me.status.maxHp / 2) PlayAtk(Random.Range(0, 3));
        else PlayAtk(Random.Range(2, 6));
    }

    public void PlayAtk(int patternIndex)
    {
        switch (patternIndex)
        {
            case 0:
                {
                    curAtkPattern = eSpirit_AtkPattern.NoramlAtk;
                    PlayNormalAtk();
                }
                break;
            case 1:
                {
                    curAtkPattern = eSpirit_AtkPattern.SpinAtk;
                    PlaySpinAtk();
                }
                break;
            case 2:
                {
                    curAtkPattern = eSpirit_AtkPattern.ConsecutivelyAtk;
                    PlayConsecutivelyAtk();
                }
                break;
            case 3:
                {
                    curAtkPattern = eSpirit_AtkPattern.Strafe_Back;
                    PlayNormalAtk();
                }
                break;
            case 4:
                {
                    curAtkPattern = eSpirit_AtkPattern.Strafe_Left;
                    PlaySpinAtk();
                }
                break;
            case 5:
                {
                    curAtkPattern = eSpirit_AtkPattern.Strafe_Right;
                    PlayConsecutivelyAtk();
                }
                break;
            default:
                break;
        }
    }


    public void PlayNormalAtk()
    {
        me.animCtrl.SetTrigger("isNormalAtk");
    }
    public void PlaySpinAtk()
    {
        me.animCtrl.SetTrigger("isSpinAtk");
    }
    public void PlayConsecutivelyAtk()
    {
        me.animCtrl.SetTrigger("isConsecutiveltAtk");
    }
    public void PlayStrafe_Back()
    {
        me.animCtrl.SetTrigger("isStrafeBack");
    }
    public void PlayStrafe_Left()
    {
        me.animCtrl.SetTrigger("isStrafeLeft");
    }
    public void PlayStrafe_Right()
    {
        me.animCtrl.SetTrigger("isStrafeRight");
    }
}
