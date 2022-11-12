using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Structs;


//몬스터 연속딜 차단



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
    public eWeaponType type;
    public int Dmg;
    public GameObject owner;

    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;

    //공용
    //public bool isColliderEnter; //1회타격 제한 bool;

    protected abstract void weaponInitialize();

    protected virtual void Awake()
    {
        weaponInitialize();
        col = GetComponent<Collider>();
        PlayerLayer = LayerMask.GetMask("Player_Hitbox");
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

    // 딜은 두상태모두 씨게박히게
    
    // 패링시 -> 스턴 + 데미지
    // 압잡뒤잡조건 -> 스턴이 걸린상태에서 딜을 넣는 기술
    // 적이 플레이어 타격시 연속딜 들어가는거 수정

    // 몬스터 강공처리
    // dmgStruct.atkType = Enums.eAttackType.Strong;

    //===============================================================================================================================
    // 데미지 주고받기
    public void Att(GameObject HittedObj)
    {

        //owner : 적
        //HittedObj : 플레이어

        //맞은 놈 : player
        if (HittedObj.transform.root.GetComponent<Player>() != null)
        {
            Structs.DamagedStruct dmgStruct = new DamagedStruct();

            dmgStruct.dmg = Dmg;
            dmgStruct.attackObj = owner;

            PlayerActionTable temp = HittedObj.transform.root.GetComponent<Player>().playerAt;

            if (temp != null)
            {
                if(Player.instance.status.isParrying == true)
                {
                    ParryingToEnemy(owner.GetComponent<Enemy>());
                    Debug.Log("Payying");
                }
                else
                {
                    temp.Hit(dmgStruct);
                    Debug.Log("Hit");
                }
            }
        }

        //맞은 놈 : enemy 
        else if (HittedObj.GetComponent<Enemy>() != null)
        {
            if (!HittedObj.GetComponent<Enemy>().status.isDead)
            {
                HittedObj.GetComponent<Enemy>().status.curHp -= Dmg;
            }
            else
            {
                return;
            }
        }
    }

    //===============================================================================================================================

    //===============================================================================================================================
    // ParryingToSpirit
    //===============================================================================================================================
    public void ParryingToEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        if (enemy.GetCurState<Enums.eSpiritState>() == Enums.eSpiritState.Atk)
        {
            if (enemy.GetComponent<Spirit>().atting && !enemy.GetComponent<Spirit>().status.isGroggy)
            {
                enemy.GetComponent<Spirit>().status.isGroggy = true;
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
                if(other.transform.root.GetComponent<Player>() != null)
                {
                    Att(other.gameObject);
                }
            }
        }
    }
    //===============================================================================================================================

}

// 비대면 온라인 채팅
//용석 : dsgjsfg;
