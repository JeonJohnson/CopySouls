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
        //여기서 분기점이 갈림
        //장비하고 있는 무기를 퀵에 등록할 수 있는가? 없다
        //-> 그럼 이미 장비중인 아이템에서 선택창을 띄울 시 선택지 안줌
        //-> 드래그로 퀵슬롯 등록시 그냥 반환 처리

        if (invenSlot != null)
        {
            invenSlot.SetRegister(false);
            invenSlot.curRegisterQuickSlot = null;
        }
        invenSlot = _invenSlot;
        _invenSlot.SetRegister(true);
        _invenSlot.curRegisterQuickSlot = _quickSlot;
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
            invenSlot.SetEquiptment(false);
            invenSlot.curRegisterQuickSlot = null;
        }
        invenSlot = _invenSlot;
        _invenSlot.SetEquiptment(true);
        _invenSlot.curRegisterQuickSlot = _equiptSlot;
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
    public void QuickSlotEquipt(QuickSlot _quickSlot)
    {
        if (_quickSlot == null)
        {
            Debug.Log("등록된 장비 없음");
            return;
        }
            
        if (item != null)
        {
            //장비하고 이미지 위아래로 바꿔야함
            //Inventory.Instance.SelectionParent.Equipt(_quickSlot.item);

        }
    }
}
