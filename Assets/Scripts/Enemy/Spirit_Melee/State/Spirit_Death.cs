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

    //�ִϸ��̼� ����ȵ�!!
    public override void UpdateState()
    {
        if(me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            me.animCtrl.SetFloat("AttIndex", DeathIndex);
            Debug.Log("Death�ִϸ��̼��� ������ , " + DeathIndex);
            Debug.Log(me.animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime);

            if (((Spirit)me).isCurrentAnimationOver(me.animCtrl, 0.5f))
            {
                Debug.Log("�ش� �ִϸ��̼� ���� ���� ��");
                Debug.Log("������ ü����");
                //me.animCtrl.enabled = false;
            }
        }

        //me.gameObject.SetActive(false);
    }
    public override void ExitState()
    {
        
    }
}
