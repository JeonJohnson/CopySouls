using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Equipt -> Unequipt(범위벗어남) -> Idle
//Equipt -> Trace(장비후 추격)
//Equipt -> Att

public class Spirit_Equipt : cState
{

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.GetComponent<FieldOfView>().viewAngle = 360f;
        me.animCtrl.SetBool("isEquipt", true);
        me.MoveStop();
        ((Spirit)me).isEquipt = true;
    }

    public override void UpdateState()
    {
        me.MoveStop();
        if (((Spirit)me).complete_Equipt)
        {
            if (me.isAlert)
            {
                if (me.distToTarget <= me.status.atkRange)
                {
                    me.SetState((int)Enums.eSpiritState.Atk);
                }
                else if (me.distToTarget > me.status.atkRange && me.distToTarget <= me.status.ricognitionRange)
                {
                    me.SetState((int)Enums.eSpiritState.Trace);
                }
            }
            else
            {
                me.SetState((int)Enums.eSpiritState.Unequipt);
            }
        }
        else
        {
            //애니메이션이 발만 떠서 맞춰서 내렸다가 올려줘야함
            //Vector3 vec = new Vector3(0, me.transform.position.y - Time.deltaTime, 0);
            //me.transform.position = vec;
        }
    }

    public override void ExitState()
    {
        ((Spirit)me).complete_Equipt = false;
        me.animCtrl.SetBool("isEquipt", false);
    }


    public void Spirit_StartEquiptment()
    {
    }

    
 
}
