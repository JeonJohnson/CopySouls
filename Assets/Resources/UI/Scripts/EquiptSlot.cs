using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquiptSlot : QuickSlot
{
    public EquiptSlot_Q equalSlot;

    void Start()
    {
        slotType = SlotType.EquiptSlot;
        if (OnlyType == Enums.ItemType.weapon_Equiptment_Item)
        {
            if (equalSlot == null && UiManager.Instance.EquiptSlotQ_Weapon)
            {
                equalSlot = UiManager.Instance.EquiptSlotQ_Weapon;
            }
        }
        else if (OnlyType == Enums.ItemType.Defence_Equiptment_Item)
        {
            if (equalSlot == null && UiManager.Instance.EquiptSlotQ_Defence)
            {
                equalSlot = UiManager.Instance.EquiptSlotQ_Defence;
            }
        }
    }

    //ó���� ��� �ø��� �ΰ� ����

    public void matchEquiptmentSlot_Q()
    {
        equalSlot.Item_Image.sprite = Item_Image.sprite;
        equalSlot.Item_Image.color = Item_Image.color;
    }
}
