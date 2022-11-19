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

    public void DragRegister(Item _item,int _itemCount)
    {
        if (_item.itemType == OnlyType)
        {
            AddRegister(_item, _itemCount);
        }
    }
    public void AddRegister(Item _item, int _itemCount)
    {
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
        item = null;
        itemCount = 0;
        Item_Image.sprite = null;
        SetColor_q(0);
        ItemCount_Text.text = "";
    }
}
