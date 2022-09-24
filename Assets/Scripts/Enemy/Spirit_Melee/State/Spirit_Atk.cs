using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Atk : cState
{
    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.SetTrigger("isSlash");
    }
    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {

    }
}
