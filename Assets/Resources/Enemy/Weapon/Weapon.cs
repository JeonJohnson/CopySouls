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
    //owner ���� ���ݽô�
    //�Ʒ� �ڵ� void start()�� �ٿ��ֱ�!!
    // weapon = GetComponentInChildren<Weapon>();
    // weapon.owner = gameObject;
    //WeaponType�� �� �θ� ��ӹ��� ������ü���� �������ֱ�~

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
