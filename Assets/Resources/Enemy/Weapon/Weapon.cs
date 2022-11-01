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
    public float Dmg;

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
}
