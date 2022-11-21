using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    public Enums.ItemType OnlyType;
    public Item item;
    public Image Item_Image;
    public int itemCount;
    public Text ItemCount_Text;
    public Slot invenSlot;

    public void DragRegister(Slot _invenSlot,Item _item,int _itemCount)
    {
        if (_item.itemType == OnlyType)
        {
            //AddRegister(_invenSlot,_item, _itemCount);
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
        _invenSlot.SetRegister(true);
        _invenSlot.curRegisterQuickSlot = _quickSlot;
        item = _item;
        Item_Image.sprite = _item.itemImage;
        itemCount = _itemCount;
        ItemCount_Text.text = _itemCount.ToString();
        SetColor_q(1);
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
        ItemCount_Text.text = itemCount.ToString();
        if (itemCount <= 0) ClearSlot_q();
    }

    public void ClearSlot_q()
    {
        invenSlot.isQuick = false;
        invenSlot.SetRegister(false);
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
            Inventory.Instance.SelectionParent.Use(item,this);
        }
    }
    public void QuickSlotEquipt()
    {
        if (item != null)
        {
            Inventory.Instance.SelectionParent.Equipt(item);
        }
    }
}
