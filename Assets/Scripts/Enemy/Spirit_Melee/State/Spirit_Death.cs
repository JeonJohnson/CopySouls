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

    public override void EnterState(Enemy script)
    {
        base.EnterState(script);
        me.animCtrl.enabled = false;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            ((Spirit)me).CreateRemainderWeapon(me.weapon.transform);
            me.gameObject.SetActive(false);
        }
        else
        {
            if (R > -1)
            {
                R -= Time.deltaTime;
                G -= Time.deltaTime;
                B -= Time.deltaTime;
                me.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = new Color(R, G, B);
            }
        }
        Debug.Log("사망이요");
    }
    public override void ExitState()
    {
    }
}
