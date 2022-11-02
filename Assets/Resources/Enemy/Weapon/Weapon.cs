using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWeaponType
{
    None,
    Melee,
    Arrow,
    Range,
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
    public eWeaponType Type;
    public GameObject owner;
    public int Dmg;

    void Start()
    {
        col = GetComponent<Collider>();
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
            other.GetComponent<Player>().status.curHp -= Dmg;
        }
        //맞은 놈 : enemy 
        else if (other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().status.curHp -= Dmg;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if(owner.gameObject.GetComponent<Enemy>() != null)
        {
            if(!owner.gameObject.GetComponent<Enemy>().isDead)
            {
                if (other.gameObject.layer == owner.gameObject.GetComponent<Enemy>().player_Hitbox)
                {
                    Att(other.gameObject);
                }
            }
        }
        else if (owner.gameObject.GetComponent<Player>() != null)
        {
            //if (!owner.gameObject.GetComponent<Enemy>().isDead)
            if (other.gameObject.layer == 7)
            {
                Att(other.gameObject);
            }
        }
    }
}
