using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Death : cState
{

    //플레이어 액션테이블 Hit함수

    public float timer;
    public float R=1;
    public float G=1;
    public float B=1;
    public bool Complete_Black;
    public float value;
    public float per = 0.3f;

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.status.isDead = true;
        Debug.Log("사망이요");

        me.animCtrl.enabled = false;
        me.navAgent.enabled = false;
        me.GetComponent<FieldOfView>().enabled = false;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (!Complete_Black)
        {
            if (R < -1)
            {
                Complete_Black = true;
                timer = 0;
                ((Spirit)me).Material_Change(((Spirit)me).Material_Disable);
            }
            R -= Time.deltaTime;
            G -= Time.deltaTime;
            B -= Time.deltaTime;
            me.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = new Color(R, G, B);
        }
        else
        {
            value = 1 - (timer * per);
            if (value > 0)
            {
                me.GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("_FresnelPower", value);
            }
            else if (value <= 0)
            {
                //((Spirit)me).CreateRemainderWeapon(me.weapon.transform);
                me.gameObject.SetActive(false);
            }
        }
    }
    public override void ExitState()
    {
        
    }
}
