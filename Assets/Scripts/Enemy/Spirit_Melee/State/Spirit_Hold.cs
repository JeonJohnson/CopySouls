using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Hold : cState
{

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetBool("isHold", true);
        //데미지 주기
        //앞잡 뒤잡 결정하기
        me.animCtrl.SetFloat("AttIndex", 1);
    }

    public override void UpdateState()
    {
        if (me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Hold"))
        {
            if (((Spirit)me).isCurrentAnimationOver(me.animCtrl, 1f))
            {
                if(me.animCtrl.GetBool("isHold"))
                {
                    me.animCtrl.SetBool("isHold", true);
                }
            }
        }
    }

    public override void ExitState()
    {
    }
}
