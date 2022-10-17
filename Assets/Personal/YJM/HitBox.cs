using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public void OnHit(float amount, Vector3 dir)
    {
        PlayerActionTable.instance.Hit((int)amount, dir);
    }
}
