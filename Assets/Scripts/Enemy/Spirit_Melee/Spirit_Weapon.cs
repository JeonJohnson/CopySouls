using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit_Weapon : Weapon
{
    public Spirit me;
    public Transform initPos;
    public Transform transPos;

    protected override void weaponInitialize()
    {
        type = eWeaponType.Melee;
        Dmg = 1;
    }
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        if (owner != null) me = owner.GetComponent<Spirit>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
    }

    protected override void LateUpdate()
    {
    }

    public void TransWeaponPos()
    {
        if (transPos == null) return;
        if(!me.preChangeWeaponPos)
        {
            TransPos();
            me.preChangeWeaponPos = true;
        }
    }

    public void ReturnWeaponPos()
    {
        if (initPos == null) return;

        if (me.preChangeWeaponPos)
        {
            ReturnPos();
            me.preChangeWeaponPos = false;
        }
    }

    public void TransPos()
    {
        transform.rotation = transPos.rotation;
    }

    public void ReturnPos()
    {
        transform.rotation = initPos.rotation;
    }
}

