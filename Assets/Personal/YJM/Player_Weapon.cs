using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Player_Weapon : Weapon
{
    public WeaponStatus status;

    //int atk = 10;

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
