using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlotType
{
    QuickSlot,
    EquiptSlot,
    End,
}

public class QuickSlot : MonoBehaviour
{
    public SlotType slotType;
    public Enums.ItemType OnlyType;
    public Item item;
    public Image Item_Image;
    public int itemCount;
    public Text ItemCount_Text;
    public Slot invenSlot;

    public void DragRegister(Slot _invenSlot,Item _item,int _itemCount)
    {
        if (_invenSlot.isEquiptment) return;

        if (_item.itemType == OnlyType)
        {
            AddRegister(_invenSlot,_item, _itemCount, this);
        }
    }
    public void AddRegister(Slot _invenSlot, Item _item, int _itemCount,QuickSlot _quickSlot)
    {
        if (invenSlot != null)
        {
            invenSlot.SetRegister(false);
            invenSlot.curRegisterQuickSlot = null;
        }
        invenSlot = _invenSlot;
        invenSlot.SetRegister(true);
        invenSlot.curRegisterQuickSlot = _quickSlot;
        item = _item;
        Item_Image.sprite = _item.itemImage;
        itemCount = _itemCount;
        if (item.itemType == Enums.ItemType.Defence_Equiptment_Item || item.itemType == Enums.ItemType.weapon_Equiptment_Item)
        {
            ItemCount_Text.text = "";
        }
        else
        {
            ItemCount_Text.text = _itemCount.ToString();
        }
        SetColor_q(1);
    }
    public void DragEquiptment(Slot _invenSlot, Item _item, int _itemCount)
    {
        if (_invenSlot.item == item) return;

        if (_item.itemType == OnlyType)
        {
            AddEquiptment(_invenSlot, _item, _itemCount, (EquiptSlot)this);
        }
    }
    public void AddEquiptment(Slot _invenSlot, Item _item, int _itemCount, EquiptSlot _equiptSlot)
    {
        
        if (invenSlot != null)
        {
            Debug.Log(invenSlot.name);
            invenSlot.SetEquiptment(false);
            if (invenSlot.isQuick) Inventory.Instance.SelectionParent.Deregisteration(invenSlot);
            //invenSlot.curRegisterQuickSlot = null;
            //invenSlot.SetRegister(true);
            //invenSlot.curRegisterQuickSlot = _equiptSlot.invenSlot.curRegisterQuickSlot;
        }
        invenSlot = _invenSlot;
        invenSlot.SetEquiptment(true);
        invenSlot.curRegisterQuickSlot = _equiptSlot;
        item = _item;
        Debug.Log("�ٲ� ������ : " + item.name);
        Item_Image.sprite = _item.itemImage;
        itemCount = _itemCount;
        if (item.itemType == Enums.ItemType.Defence_Equiptment_Item || item.itemType == Enums.ItemType.weapon_Equiptment_Item)
        {
            ItemCount_Text.text = "";
        }
        else
        {
            ItemCount_Text.text = _itemCount.ToString();
        }
        SetColor_q(1);
        _equiptSlot.matchEquiptmentSlot_Q();
    }


    public void SetColor_q(float _alpha)
    {
        Color color = Item_Image.color;
        color.a = _alpha;
        Item_Image.color = color;
    }
    public void SetSlotCount_q(int _count)
    {
        itemCount += _count;
        if (item.itemType == Enums.ItemType.Defence_Equiptment_Item || item.itemType == Enums.ItemType.weapon_Equiptment_Item)
        {
            ItemCount_Text.text = "";
        }
        else
        {
            ItemCount_Text.text = itemCount.ToString();
        }
        if (itemCount <= 0) ClearSlot_q();
    }

    public void ClearSlot_q()
    {
        invenSlot.isQuick = false;
        invenSlot.isEquiptment = false;
        invenSlot.SetRegister(false);
        invenSlot.SetEquiptment(false);
        invenSlot = null;
        item = null;
        itemCount = 0;
        Item_Image.sprite = null;
        SetColor_q(0);
        ItemCount_Text.text = "";
    }
    public void QuickSlotUse()
    {
        if (item != null)
        {
            Inventory.Instance.SelectionParent.Use(invenSlot);
        }
    }
    public void QuickSlotEquipt(QuickSlot _quickSlot,EquiptSlot _equiptSlot)
    {
        if (_equiptSlot.invenSlot != null && invenSlot != null)
        {
            Slot temp = invenSlot;

            if (_equiptSlot.invenSlot.isEquiptment)
            {
                if (_equiptSlot.invenSlot.isEquiptment)
                {
                    //��� ���� ��Ű��
                    _equiptSlot.invenSlot.SetEquiptment(false);
                    _equiptSlot.invenSlot.SetRegister(true);
                    //�����â�� ����
                    Item_Image.sprite = _equiptSlot.invenSlot.item_Image.sprite;
                    _equiptSlot.Item_Image.sprite = null;
                    _equiptSlot.SetColor_q(0);
                    _equiptSlot.matchEquiptmentSlot_Q();
                }
            }
            //����ϵ� ����
            if (invenSlot.isQuick)
            {
                invenSlot.SetRegister(false);
                invenSlot.SetEquiptment(true);
                _equiptSlot.Item_Image.sprite = invenSlot.item_Image.sprite;
                _equiptSlot.SetColor_q(1);
                _equiptSlot.matchEquiptmentSlot_Q();
                //��ϵ� �κ� ���� ��ü�������
                invenSlot = _equiptSlot.invenSlot;
                _equiptSlot.invenSlot = temp;
            }

            //���⿣ ���� ����
            if(invenSlot.item.GetComponent<Player_Weapon>().type == eWeaponType.Melee)
            {
                invenSlot.item.GetComponent<Item_Weapon>().SetAsMainWeapon();
            }
            else if (invenSlot.item.GetComponent<Player_Weapon>().type == eWeaponType.Sheild)
            {
                invenSlot.item.GetComponent<Item_Weapon>().SetAsSubWeapon();
            }
        }
        else if (_equiptSlot.invenSlot == null && invenSlot != null)
        {
            Slot temp = _quickSlot.invenSlot;

            Debug.Log("�ָԿ��� ����� ��ü : " + temp.name);
            //����� ��ü
            if (invenSlot.isQuick)
            {
                invenSlot.SetRegister(false);
                invenSlot.SetEquiptment(true);
                //��񽽷����� �ٲ������
                invenSlot.curRegisterQuickSlot = _equiptSlot;
                _equiptSlot.Item_Image.sprite = invenSlot.item_Image.sprite;
                _equiptSlot.SetColor_q(1);
                _equiptSlot.invenSlot = _quickSlot.invenSlot;
                _equiptSlot.matchEquiptmentSlot_Q();
                //��ϵ� �κ� ���� ��ü�������
                //invenSlot = null;
                // = _quickSlot.invenSlot.curRegisterQuickSlot;
                _quickSlot.Item_Image.sprite = null;
                _quickSlot.invenSlot = null;
                _quickSlot.SetColor_q(0);

                //�ָԿ��� �����
                if (_equiptSlot.item.GetComponent<Player_Weapon>().type == eWeaponType.Melee)
                {
                    _equiptSlot.item.GetComponent<Item_Weapon>().SetAsMainWeapon();
                }
                else if (_equiptSlot.item.GetComponent<Player_Weapon>().type == eWeaponType.Sheild)
                {
                    _equiptSlot.item.GetComponent<Item_Weapon>().SetAsSubWeapon();
                }
            }
        }
        else if (_equiptSlot.invenSlot != null && invenSlot == null)
        {
            Debug.Log("���⿡�� �ָ����� ��ü");

            if (_equiptSlot.invenSlot.isEquiptment)
            {
                //�ָ����� ��ü
                if (_equiptSlot.invenSlot.isEquiptment)
                {
                    //��� ���� ��Ű��
                    _equiptSlot.invenSlot.SetEquiptment(false);
                    _equiptSlot.invenSlot.SetRegister(true);
                    //���������� �ٲ������
                    _equiptSlot.invenSlot.curRegisterQuickSlot = _quickSlot;
                    _quickSlot.invenSlot = _equiptSlot.invenSlot;
                    //�����â�� ����
                    Item_Image.sprite = _equiptSlot.Item_Image.sprite;
                    SetColor_q(1);

                    _equiptSlot.Item_Image.sprite = null;
                    _equiptSlot.SetColor_q(0);
                    _equiptSlot.invenSlot = null;
                    _equiptSlot.matchEquiptmentSlot_Q();

                    
                }

                //���⿡�� �ָ����� 
                Player.instance.status.mainWeapon.GetComponent<Item_Weapon>().DeselectWeapon();
            }
        }
        else return;



    }
}
