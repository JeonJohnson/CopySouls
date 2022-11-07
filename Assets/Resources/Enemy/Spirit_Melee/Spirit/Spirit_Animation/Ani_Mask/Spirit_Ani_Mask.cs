using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAnimationLayer
{
    Base,
    Idle,
    Walk,
    Equipt,
    UnEquipt,
    Trace,
    Damaged,
    NormalAtt,
    SwingAttUp,
    SwingAttDown,
    DashAtkUp,
    DashAtkDown,
    End,
}


public class Spirit_Ani_Mask : MonoBehaviour
{
    public Spirit me;
    public Animator ani;
    public float temp = 0.3f;
    public bool complete;

    void Start()
    {
        me = GetComponentInParent<Spirit>();
        ani = GetComponentInParent<Animator>();
    }

    void Update()
    {
        //Debug.Log(ani.GetLayerWeight((int)eAnimationLayer.DashAtkUp));

        //if (ani.GetLayerWeight((int)eAnimationLayer.DashAtkUp) <= 1f && ani.GetLayerWeight((int)eAnimationLayer.DashAtkDown) <= 1f)

        if(ani.GetBool("isDash"))
        {
            if (temp <= 1)
            {
                temp += ((1 / me.dashTime) * Time.deltaTime);
            }
        }
        else temp = 0.3f;
        ani.SetLayerWeight((int)eAnimationLayer.DashAtkUp, temp);
        ani.SetLayerWeight((int)eAnimationLayer.DashAtkDown, temp);
    }
}
