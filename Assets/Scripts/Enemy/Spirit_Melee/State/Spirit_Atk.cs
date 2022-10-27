using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum eSpirit_AtkPattern
{
    None,
    NoramlAtk,
    //SpinAtk,
    //ConsecutivelyAtk,
    //Strafe_Right,
    //Strafe_Left,
    //Strafe_Back,
    End
}

public class Spirit_Atk : cState
{
    //public eSpirit_AtkPattern curAtkPattern;

    public eSpirit_AtkPattern CurPattern;
    public Quaternion initailAngle;

    public float SpinSpeed = 500;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        Spirit_StartAtk();
        SelectAttPattern();
        PlayNormalAtk();
        Select(2);
    }

    public override void UpdateState()
    {
        //me.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //Orbit_Rotation(500);

        if (CurPattern == eSpirit_AtkPattern.None) return;
        //Debug.Log(CurPattern);

        if (((Spirit)me).complete_Atk)
        {
            if(!((Spirit)me).complete_AttReturn)
            {
                if(Mathf.Sqrt(me.transform.rotation.y - initailAngle.y) > 0)
                {
                    me.transform.rotation = Quaternion.Euler(0, me.transform.rotation.y - Time.deltaTime, 0);
                }
                else
                {
                    ((Spirit)me).complete_AttReturn = true;
                }
            }

            StopNormalAtk();
            if (me.isAlert)
            {
                ((Spirit)me).complete_Atk = false;
                if (me.distToTarget <= me.status.atkRange)
                {
                    SelectAttPattern();
                    PlayNormalAtk();
                }
                else if (me.distToTarget > me.status.atkRange)
                {
                    me.SetState((int)Enums.eSpiritState.Trace);
                }
            }
            else
            {
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
        }
           //Select(Random.Range(((int)eSpirit_AtkPattern.None) + 1, ((int)eSpirit_AtkPattern.End) + 1));
           //Select(2);
    }

    public override void ExitState()
    {
        Spirit_StopAtk();
    }

    public void SelectAttPattern()
    {
        int AttPatternIndex = Random.Range(((int)eSpirit_AtkPattern.None) + 1, ((int)eSpirit_AtkPattern.End));
        CurPattern = (eSpirit_AtkPattern)AttPatternIndex;
    }

    public void PlayerCurAttPattern(eSpirit_AtkPattern CurPattern)
    {
        switch (CurPattern)
        {
            case eSpirit_AtkPattern.None:
                break;
            case eSpirit_AtkPattern.NoramlAtk:
                PlayNormalAtk();
                break;
            case eSpirit_AtkPattern.End:
                break;
            default:
                break;
        }
    }


    public void Spirit_StartAtk()
    {
        me.animCtrl.SetBool("isAtk", true);
        me.MoveStop();
    }

    public void Spirit_StopAtk()
    {
        me.animCtrl.SetBool("isAtk", false);
    }
    


    //움직이면 공격모션 희안하게 나감

    public void PlayNormalAtk()
    {
        me.animCtrl.SetTrigger("isNormalAtk");
        initailAngle = me.transform.rotation;
        me.animCtrl.applyRootMotion = true;
    }

    public void StopNormalAtk()
    {
        me.animCtrl.applyRootMotion = false;
        //me.transform.rotation = Quaternion.Euler(initailAngle.x, initailAngle.y, initailAngle.z);
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
        }
    }
}
 