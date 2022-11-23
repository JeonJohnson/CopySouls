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
        if (playerWeapon.type == eWeaponType.Sheild && weapon.type != eWeaponType.Sheild)
        {
            print("222");
            Player_Weapon playerSubWeapon = Player.instance.status.subWeapon.GetComponent<Player_Weapon>();
            playerSubWeapon.type = playerWeapon.type;
            playerSubWeapon.Dmg = playerWeapon.Dmg;
            playerSubWeapon.status = playerWeapon.status;
            playerSubWeapon.gameObject.GetComponent<MeshFilter>().mesh = playerWeapon.gameObject.GetComponent<MeshFilter>().mesh;
            print(playerSubWeapon.gameObject.GetComponent<MeshFilter>().mesh);
            Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 2);
        }
        else if(weapon.type == eWeaponType.Sheild)
        {
            Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 1);
            print("쉴드 픽");
        }
        else if(weapon.type == eWeaponType.Melee)
        {
            Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 0);
            print("이건 아닌듯");
            print("쏘오드 픽");
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
