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
            Player.instance.ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[2]);
            Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 2);
        }
        else if(weapon.type == eWeaponType.Sheild)
        {
            Player.instance.ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[1]);
            Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 1);
            print("���� ��");
        }
        else if(weapon.type == eWeaponType.Melee)
        {
            Player.instance.ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[0]);
            Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 0);
            print("����� ��");
        }
        playerWeapon.type = weapon.type;
        playerWeapon.Dmg = weapon.Dmg;
        playerWeapon.status = weapon.status;

        playerWeapon.gameObject.GetComponent<MeshFilter>().mesh = weapon.gameObject.GetComponent<MeshFilter>().mesh;
        playerWeapon.gameObject.GetComponent<BoxCollider>().size = weapon.gameObject.GetComponent<BoxCollider>().size;
        playerWeapon.gameObject.GetComponent<BoxCollider>().center = weapon.gameObject.GetComponent<BoxCollider>().center;
    }

    public void SetAsMainWeapon()
    {
        Player_Weapon playerWeapon = Player.instance.status.mainWeapon.GetComponent<Player_Weapon>();
        playerWeapon.type = weapon.type;
        playerWeapon.Dmg = weapon.Dmg;
        playerWeapon.status = weapon.status;
        playerWeapon.gameObject.GetComponent<MeshFilter>().mesh = weapon.gameObject.GetComponent<MeshFilter>().mesh;
        playerWeapon.gameObject.GetComponent<BoxCollider>().size = weapon.gameObject.GetComponent<BoxCollider>().size;
        playerWeapon.gameObject.GetComponent<BoxCollider>().center = weapon.gameObject.GetComponent<BoxCollider>().center;
        PlayerActionTable.instance.ChangeWeaponHoldType(false);
    }

    public void SetAsSubWeapon()
    {

        Player_Weapon playerWeapon = Player.instance.status.subWeapon.GetComponent<Player_Weapon>();
        playerWeapon.type = weapon.type;
        playerWeapon.Dmg = weapon.Dmg;
        playerWeapon.status = weapon.status;
        playerWeapon.gameObject.GetComponent<MeshFilter>().mesh = weapon.gameObject.GetComponent<MeshFilter>().mesh;
        PlayerActionTable.instance.ChangeWeaponHoldType(false);
    }

    public void DeselectWeapon()
    {
        Player_Weapon playerWeapon = this.gameObject.GetComponent<Player_Weapon>();
        playerWeapon.type = eWeaponType.None;
        playerWeapon.Dmg = 1;
        playerWeapon.gameObject.GetComponent<MeshFilter>().mesh = null;
        PlayerActionTable.instance.ChangeWeaponHoldType(false);
        playerWeapon.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, 0.5f);
        playerWeapon.gameObject.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
    }
}
