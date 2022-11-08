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

public class Weapon : MonoBehaviour
{
    //owner 연결 해줍시당
    //아래 코드 void start()에 붙여넣기!!
    // weapon = GetComponentInChildren<Weapon>();
    // weapon.owner = gameObject;
    //WeaponType은 각 부모를 상속받은 하위객체에서 결정해주기~

    public Collider col;

    //public WeaponStatus status; //나중에 정리할 부분

    public eWeaponType Type;
    public GameObject owner;
    public int Dmg;

    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;

    //==
    //공용
    //public bool isColliderEnter; //1회타격 제한 bool;
    //==

    //public Transform initPos; //바꾸기 전 위치
    //public Transform transPos; //바꿀 위치
    //public bool isPosChange;

    void Start()
    {
        col = GetComponent<Collider>();
        PlayerLayer = 1 << LayerMask.GetMask("Player");
        EnemyLayer = 1 << LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
    }

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
                }
            }
        }
        //맞은 놈 : enemy 
        else if (other.GetComponent<Enemy>() != null)
        {
            if (!other.GetComponent<Enemy>().status.isDead) other.GetComponent<Enemy>().status.curHp -= Dmg;
            else return;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //Enemy -> Player
        if (owner.gameObject.GetComponent<Enemy>() != null)
        {
            if(!owner.gameObject.GetComponent<Enemy>().status.isDead)
            {
                if (other.gameObject.layer == PlayerLayer) Att(other.gameObject);
            }
        }
        //Player -> Enemy
        else if (owner.gameObject.GetComponent<Player>() != null)
        {
            if (other.gameObject.layer == EnemyLayer) Att(other.gameObject);
        }
    }

    //특정 애니메이션에서 무기 위치나 각도가 바뀔 상황시 사용
    //public void TransWeaponPos(Weapon weapon)
    //{
    //    if (weapon != null)
    //    {
    //        Debug.Log("바꿀께");
    //        weapon.transform.position = weapon.transPos.position;
    //        weapon.transform.rotation = weapon.transPos.rotation;
    //        isPosChange = true;
    //    }
    //}

    //public void reTurnWeaponPos(Weapon weapon)
    //{
    //    if (weapon != null)
    //    {
    //        if(isPosChange)
    //        {
    //            Debug.Log("돌릴께");
    //            weapon.transform.position = weapon.initPos.position;
    //            weapon.transform.rotation = weapon.initPos.rotation;
    //            isPosChange = false;
    //        }
    //    }
    //
    //}
}
