using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    /// <summary>
    /// ���� �Ⱦ�. Player_Hitbox ���̾� �浹�ϸ� PlayerActionTable���� Hit() ȣ���ϴ°ɷ� �ٲ� - 11.22 ����
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="dir"></param>
    public void OnHit(float amount, Vector3 dir)
    {
        //PlayerActionTable.instance.Hit((int)amount, dir);
    }
}
