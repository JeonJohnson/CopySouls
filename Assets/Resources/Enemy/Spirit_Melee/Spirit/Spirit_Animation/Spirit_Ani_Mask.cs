using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Ani_Mask : MonoBehaviour
{
    public Animator ani;
    float temp = 1;

    void Start()
    {
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            ani.SetTrigger("Equipt");
        }
        if (ani.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f)
        {
            if(temp > 0)
            {
                temp -= Time.deltaTime;
            }
        }
        else
        {
            temp = 1;
        }
        ani.SetLayerWeight(1, temp);
    }
}
