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

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.status.isDead = true;
        DeathIndex = Random.Range((int)DeathPattern.Front, (int)DeathPattern.End);
        me.animCtrl.SetBool("isDeath", true);
        me.navAgent.enabled = false;
        me.GetComponent<FieldOfView>().enabled = false;
    }

    //애니메이션 재생안됨!!
    public override void UpdateState()
    {
        if(me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            me.animCtrl.SetFloat("AttIndex", DeathIndex);
            Debug.Log("Death애니메이션이 맞으면 , " + DeathIndex);
            Debug.Log(me.animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime);

            if (((Spirit)me).isCurrentAnimationOver(me.animCtrl, 0.5f))
            {
                Debug.Log("해당 애니메이션 절반 진행 후");
                Debug.Log("렉돌로 체인지");
                //me.animCtrl.enabled = false;
            }
        }

        //me.gameObject.SetActive(false);
    }
    public override void ExitState()
    {
        
    }
}
