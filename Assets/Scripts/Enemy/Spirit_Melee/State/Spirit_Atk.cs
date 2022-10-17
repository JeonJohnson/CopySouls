using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum eSpirit_AtkPattern
{
    None,
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
    //public eSpirit_AtkPattern curAtkPattern;

    public eSpirit_AtkPattern CurPattern;
    public float SpinSpeed = 500;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Spirit_StartAtk();
        //Spirit_SelectAtkPattern();
        Select(2);
    }
    public override void UpdateState()
    {
        //me.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));


        //Orbit_Rotation(500);


        if (me.targetObj != null)
        {
            if (((Spirit)me).complete_Atk)
            {
                ((Spirit)me).complete_Atk = false;
                if (me.distToTarget > me.status.atkRange)
                {
                    //me.SetState((int)Enums.eSpiritState.Trace);
                }
                else
                {
                    //Select(Random.Range(((int)eSpirit_AtkPattern.None) + 1, ((int)eSpirit_AtkPattern.End) + 1));
                    Select(2);
                }
            }
            else
            {
                Play(CurPattern);
            }
        }
        else
        {
            //me.SetState((int)Enums.eSpiritState.Unequipt);
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
    




    public void PlayNormalAtk()
    {
        me.animCtrl.SetTrigger("isNormalAtk");
    }
    public void PlaySpinAtk()
    {
        me.animCtrl.SetTrigger("isSpinAtk");
        //Orbit_Rotation(SpinSpeed);
        //MoveFoward();
    }

    public void MoveFoward()
    {
        //me.transform.position += 
    }

    public void Orbit_Rotation(float orbitSpeed)
    {
        me.transform.Rotate(Vector3.down * orbitSpeed * Time.deltaTime);
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

    public void Play(eSpirit_AtkPattern curpattern)
    {
        if (!((Spirit)me).complete_Atk)
        {
            switch (curpattern)
            {
                case eSpirit_AtkPattern.None:
                    break;
                case eSpirit_AtkPattern.NoramlAtk:
                    break;
                case eSpirit_AtkPattern.SpinAtk:
                    PlaySpinAtk();
                    break;
                case eSpirit_AtkPattern.ConsecutivelyAtk:
                    break;
                case eSpirit_AtkPattern.Strafe_Right:
                    break;
                case eSpirit_AtkPattern.Strafe_Left:
                    break;
                case eSpirit_AtkPattern.Strafe_Back:
                    break;
                case eSpirit_AtkPattern.End:
                    break;
                default:
                    break;
            }
        }
    }

    public void Select(int patternIndex)
    {
        switch (patternIndex)
        {
            case 0:
                CurPattern = eSpirit_AtkPattern.None;
                break;
            case 1:
                CurPattern = eSpirit_AtkPattern.NoramlAtk;
                break;
            case 2:
                CurPattern = eSpirit_AtkPattern.SpinAtk;
                break;
            case 3:
                CurPattern = eSpirit_AtkPattern.ConsecutivelyAtk;
                break;
            default:
                break;
        }
    }
}
 