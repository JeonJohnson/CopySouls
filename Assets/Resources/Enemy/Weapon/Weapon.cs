using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Structs;

//����� �ϴ� �ǵ鶧 ����
public enum eWeaponType
{
    None,
    Melee,
    Sheild,
    Arrow,
    Range,

    //���߰���
    //�Ѽհ�, �μհ�
    //Ȱ, ����
    //���� �˸� ������� ����ȭ �ϱ�
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

    //public WeaponStatus status; //���߿� ������ �κ�

    public eWeaponType Type;
    public GameObject owner;
    public int Dmg;

    //==
    //����
    //public bool isColliderEnter; //1ȸŸ�� ���� bool;
    //==



    //public Transform initPos; //�ٲٱ� �� ��ġ
    //public Transform transPos; //�ٲ� ��ġ
    //public bool isPosChange;

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
        //���� �� : player
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
                    //���� ���ϵǴ� �Լ�
                }
                else
                {
                    temp.Hit(dmgStruct);
                }
            }
        }
        //���� �� : enemy 
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

    //Ư�� �ִϸ��̼ǿ��� ���� ��ġ�� ������ �ٲ� ��Ȳ�� ���
    //public void TransWeaponPos(Weapon weapon)
    //{
    //    if (weapon != null)
    //    {
    //        Debug.Log("�ٲܲ�");
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
    //            Debug.Log("������");
    //            weapon.transform.position = weapon.initPos.position;
    //            weapon.transform.rotation = weapon.initPos.rotation;
    //            isPosChange = false;
    //        }
    //    }
    //
    //}
}
