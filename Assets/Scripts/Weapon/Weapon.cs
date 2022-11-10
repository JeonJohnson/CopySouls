using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Structs;

//쉴드는 일단 맨들때 배제
public enum eWeaponType
{
    None,
    Melee,
    Sheild,
    Arrow,
    Range,

    //나중가면
    //한손검, 두손검
    //활, 방패
    //마법 촉매 등등으로 세분화 하기
    End
}


//owner 연결 해줍시당
//weapon 상속받은거 각자 필요한 유닛에게 적용시키기
//weapon = GetComponentInChildren<Spirit_Weapon>();
//if (weapon != null)
//{ weapon.owner = gameObject; }
//weapon.Type = eWeaponType.Melee;

public abstract class Weapon : MonoBehaviour
{
    public Collider col;
    public eWeaponType Type;
    public GameObject owner;
    public int Dmg;

    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;

    //공용
    //public bool isColliderEnter; //1회타격 제한 bool;

    protected abstract void weaponInitialize();

    protected virtual void Awake()
    {
        weaponInitialize();
        col = GetComponent<Collider>();
        PlayerLayer = LayerMask.GetMask("Player_Hit");
        EnemyLayer = LayerMask.GetMask("Enemy");
    }

    protected virtual void Start()
    {
       
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void LateUpdate()
    {
    }

    //===============================================================================================================================
    // ColliderOnOff

    public void WeaponColliderOnOff(bool value)
    {
        if (owner != null)
        {
            col.enabled = value;
        }
    }

    public void WeaponColliderOnOff(int value)
    {
        if (owner != null)
        {
            col.enabled = Funcs.I2B(value);
        }
    }

    //===============================================================================================================================
    
    //===============================================================================================================================
    // 데미지 주고받기

    public void Att(GameObject other)
    {
        //맞은 놈 : player
        if(other.GetComponent<Player>() != null)
        {
            Structs.DamagedStruct dmgStruct = new DamagedStruct();

            dmgStruct.dmg = Dmg;
            dmgStruct.attackObj = owner;

            PlayerActionTable temp = other.transform.root.GetComponent<PlayerActionTable>();
            if(temp != null)
            {
                if(Player.instance.status.isParrying == true)
                {
                    //적이 스턴되는 함수
                }
                else
                {
                    temp.Hit(dmgStruct);
                    Debug.Log("Hit");
                }
            }
        }



        //맞은 놈 : enemy 
        else if (other.GetComponent<Enemy>() != null)
        {
            if (!other.GetComponent<Enemy>().status.isDead)
            {
                //other.GetComponent<Enemy>().status.curHp -= Dmg;
                //other.GetComponent<Enemy>().Hit()
            }
            else
            {
                return;
            }
        }
    }

    //===============================================================================================================================


    //===============================================================================================================================
    // trigger

    public void OnTriggerEnter(Collider other)
    {
        //Enemy -> Player
        if (owner.gameObject.GetComponent<Enemy>() != null)
        {
            if(!owner.gameObject.GetComponent<Enemy>().status.isDead)
            {

                //other.transform.root.GetComponent<PlayerActionTable>();

                if (other.gameObject.layer == PlayerLayer)
                {
                    Debug.Log(other.name);
                }

                //Att(other.gameObject);
            }
        }



        //Player -> Enemy
        else if (owner.gameObject.GetComponent<Player>() != null)
        {
            if (other.gameObject.layer == EnemyLayer) Att(other.gameObject);
        }
    }

    //===============================================================================================================================

}
