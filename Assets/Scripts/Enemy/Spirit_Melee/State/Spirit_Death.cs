using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeathPattern
{
    Front,
    Back,
    End
}

public class Spirit_Death : cState
{
    public int DeathIndex;
    public float timer;
    public float existTime = 3f;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        if(!me.ragdoll.gameObject.activeSelf)
        {
            me.status.isDead = true;
            DeathIndex = Random.Range((int)DeathPattern.Front, (int)DeathPattern.End);
            me.animCtrl.SetBool("isDeath", true);
            me.animCtrl.SetFloat("AttIndex", DeathIndex);
        }
        me.navAgent.enabled = false;
        me.GetComponent<FieldOfView>().enabled = false;
    }

    public override void UpdateState()
    {
        if (!me.ragdoll.gameObject.activeSelf)
        {
            if (me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                if (((Spirit)me).isCurrentAnimationOver(me.animCtrl, 0.5f))
                {
                    //((Spirit)me).CreateRemainderWeapon(((Spirit)me).weapon.transform);
                    ((Spirit)me).ChangeToRagDoll();
                    ((Spirit)me).DeathReset();
                }
            }
        }

    }
    public override void ExitState()
    {
        
    }


    //렉돌 위치 미쳣음
    //애니메이션 공들이기
}
