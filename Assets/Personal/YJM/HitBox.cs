using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    /// <summary>
    /// 이제 안씀. Player_Hitbox 레이어 충돌하면 PlayerActionTable에서 Hit() 호출하는걸로 바꿈 - 11.22 정민
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="dir"></param>
    public void OnHit(float amount, Vector3 dir)
    {
        //PlayerActionTable.instance.Hit((int)amount, dir);
    }
}
