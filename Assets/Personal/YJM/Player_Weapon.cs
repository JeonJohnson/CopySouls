﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Player_Weapon : Weapon
{
    public WeaponStatus status;
    public List<Enemy> hittedEnemyList = new List<Enemy>();

    //int atk = 10;

    protected override void weaponInitialize()
    {
        status.isUsing = true;
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
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    new private void OnTriggerEnter(Collider other)
    {
        DamagedStruct dmgStruct = new DamagedStruct();
        dmgStruct.dmg = this.Dmg * PlayerActionTable.instance.curActAtkValue;
        if (PlayerActionTable.instance.holdType) dmgStruct.dmg *= 1.2f;
        dmgStruct.attackObj = owner;

        if (owner.gameObject.GetComponent<Player>() != null)
        {
            var hittedEnemy = other.transform.root.GetComponent<Enemy>();
            Debug.Log(hittedEnemy);
            if (hittedEnemy != null)
            {
                if(hittedEnemyList.Contains(hittedEnemy) == false)
                {
                    if (!hittedEnemy.status.isDead)
                    {
                        hittedEnemyList.Add(hittedEnemy);
                        hittedEnemy.Hit(dmgStruct);
                        print("deal Damage");
                    }
                }
            }
        }
    }

    //public void EnableWeaponMeshCollider(int i)
    //{
    //    if (i == 0)
    //    {
    //        meshCollider.enabled = false;
    //    }
    //    else
    //    {
    //        meshCollider.enabled = true;
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == 7)
    //    {
    //        print("Deal Damage To Enemy Funcs");
    //        // 데미지 계산식 : atk * PlyerActionTable.curActAtkValue
    //    }
    //    meshCollider.enabled = false; // <이건 빼야할수도
    //}
}
