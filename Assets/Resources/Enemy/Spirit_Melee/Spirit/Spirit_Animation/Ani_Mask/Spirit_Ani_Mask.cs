using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAnimationLayer
{
    Base_Layer,
    Mask_Layer,
    End
}


public class Spirit_Ani_Mask : MonoBehaviour
{
    public Spirit me;
    public Animator ani;
    public AvatarMask None;
    public AvatarMask Up;
    public AvatarMask Down;
    public AvatarMask Left;
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

        if(ani.GetBool("isIdle") || ani.GetBool("isPatrol") || ani.GetBool("isTrace"))
        {
            ani.GetL
        }
        else if (ani.GetBool("isDash"))
        {

        }
        else
        {

        }

        if(ani.GetBool("isDash"))
        {
            if (temp <= 1)
            {
                temp += ((1 / me.dashTime) * Time.deltaTime);
            }
        }
        else temp = 0f;
        ani.SetLayerWeight((int)eAnimationLayer.Mask_Layer, temp);
    }
}
