using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon : Item
{
    public Weapon weapon;
    public override void Initialize()
    {

    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Start();
    }

    public override void PlayFuncs()
    {
        gameObject.tag = "Weapon";
        gameObject.layer = LayerMask.NameToLayer("PlayerWeapon");
    }
}
