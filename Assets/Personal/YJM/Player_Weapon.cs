using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Player_Weapon : Weapon
{
    public WeaponStatus status;
    public Item_Weapon item_Weapon;
    public List<Enemy> hittedEnemyList = new List<Enemy>();
    public TrailRenderer trailRenderer;

    //int atk = 10;

    protected override void weaponInitialize()
    {
        status.isUsing = true;
        item_Weapon = GetComponent<Item_Weapon>();
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
        //base.OnTriggerEnter(other);
        print("충돌");
        DamagedStruct dmgStruct = new DamagedStruct();

        UiManager.Instance.ppController.DoLens(9f,0.1f);
        InGameManager.Instance.HitStop(3);

        //UiManager.Instance.ppController.DoFocus(2.9f,0.3f);

        dmgStruct.dmg = this.Dmg * PlayerActionTable.instance.curActAtkValue;
        if (PlayerActionTable.instance.holdType) dmgStruct.dmg *= 1.2f;
        dmgStruct.attackObj = Player.instance.gameObject;

        if (owner != null)
        {
            var hittedEnemy = other.transform.root.GetComponent<Enemy>();
            Debug.Log(hittedEnemy);
            if (hittedEnemy != null)
            {
                if(hittedEnemyList.Contains(hittedEnemy) == false)
                {
                    if (!hittedEnemy.status.isDead)
                    {
                        //Shake
                        if (PlayerActionTable.instance.curActAtkValue < 1.2f) CameraEffect.instance.PlayShake("WeekHit_M");
                        else CameraEffect.instance.PlayShake("StrongHit_M");

                        hittedEnemyList.Add(hittedEnemy);
                        print("충돌중!!!");
                        hittedEnemy.Hit(dmgStruct);
                        GameObject effect = ObjectPoolingCenter.Instance.LentalObj("ScifiTris 1", 1);
                        effect.transform.position = other.ClosestPoint(transform.position);
                        effect.GetComponent<ParticleSystem>().Play();
                        if(PlayerActionTable.instance.curActAtkValue > 1.3f)
                        {
                            //InGameManager.Instance.TimeStopEffect0();
                            SoundManager.Instance.PlaySound("HardAttack_Hit", this.gameObject, 0.8f);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySound("WeakAttack_Hit", this.gameObject,0.8f);
                        }
                    }
                }
            }
            //print(other.name);
            if(other.gameObject.layer == 8)
            {
                //print("앍");
                GameObject effect = ObjectPoolingCenter.Instance.LentalObj("BasicSpark 4 (Shower)", 1);
                effect.transform.position = other.ClosestPoint(transform.position);
                effect.GetComponent<ParticleSystem>().Play();
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
