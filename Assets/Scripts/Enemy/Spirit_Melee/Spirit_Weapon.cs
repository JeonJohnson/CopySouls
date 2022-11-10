using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Weapon : Weapon
{
    public Spirit me;
    public Transform initPos;
    public Transform transPos;
    public bool changePos;

    protected override void weaponInitialize()
    {
    }
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        me = owner.GetComponent<Spirit>();
        if (me == null) Debug.Log("Get Script Error");
    }

    protected override void Update()
    {
        base.Update();

        if (me.curState_e == Enums.eSpiritState.Atk)
        {
            if (me.transWeaponPos) TransWeaponPos();
            else ReturnWeaponPos();
        }
    }

    protected override void FixedUpdate()
    {
    }

    protected override void LateUpdate()
    {
    }

    public void TransWeaponPos()
    {
        Debug.Log("위치 바꾸기");
    }
    public void ReturnWeaponPos()
    {
        Debug.Log("위치 돌리기");
    }


}

