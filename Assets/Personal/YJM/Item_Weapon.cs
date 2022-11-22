using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon : Item
{
    public Player_Weapon weapon;
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
        Player_Weapon playerWeapon = Player.instance.status.mainWeapon.GetComponent<Player_Weapon>();
        if (weapon.type == eWeaponType.Sheild && playerWeapon.type == eWeaponType.Sheild)
        {

        }
        playerWeapon.type = weapon.type;
        playerWeapon.Dmg = weapon.Dmg;
        playerWeapon.status = weapon.status;


        //playerWeapon.col = weapon.col;
        playerWeapon.gameObject.GetComponent<MeshFilter>().mesh = weapon.gameObject.GetComponent<MeshFilter>().mesh;
        //gameObject.tag = "Weapon";
        //gameObject.layer = LayerMask.NameToLayer("PlayerWeapon");
    }
}
