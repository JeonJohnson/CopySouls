using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionProcess : MonoBehaviour
{
    public static bool SelectionActivated = false;

    public GameObject Equipt_Button;
    public GameObject Use_Button;
    public GameObject Register_Button;
    public GameObject Throw_Button;

    public void Selection_AllOff()
    {
        SelectionActivated = !SelectionActivated;
        gameObject.SetActive(false);
        Equipt_Button.SetActive(false);
        Use_Button.SetActive(false);
        Register_Button.SetActive(false);
        Throw_Button.SetActive(false);
    }

    public void TryOpenSelection(Enums.ItemType _itemType, Vector3 Vec)
    {
        if (Inventory.inventoryActivated && !DivisionProcess.DivisionActivated && !SelectionActivated)
        {
            OpenSelection(_itemType, Vec);
        }
    }

    public void OpenSelection(Enums.ItemType _itemType, Vector3 vec)
    {
        SelectionActivated = !SelectionActivated;
        gameObject.SetActive(true);
        Vector3 vec1 = new Vector3(vec.x + 60f, vec.y - 15f);
        gameObject.transform.position = vec1;
        if (_itemType == Enums.ItemType.supply_Item || _itemType == Enums.ItemType.Production_Item)
        {
            Use_Button.SetActive(true);
        }
        else if (_itemType == Enums.ItemType.weapon_Equiptment_Item || _itemType == Enums.ItemType.Defence_Equiptment_Item)
        {
            Equipt_Button.SetActive(true);
        }
        Register_Button.SetActive(true);
        Throw_Button.SetActive(true);
    }

    public void CloseSelection()
    {
        gameObject.SetActive(false);
        Selection_AllOff();
    }

    public void Button_Equipt()
    {
        Equipt(Inventory.Instance.curSlot.item);
        Selection_AllOff();
        Inventory.Instance.curSlot = null;
    }
    public void Button_Use()
    {
        Use(Inventory.Instance.curSlot.item);
        Selection_AllOff();
        Inventory.Instance.curSlot = null;
    }

    public void Button_Register()
    {
        ButtonRegister(Inventory.Instance.curSlot);
        Selection_AllOff();
        Inventory.Instance.curSlot = null;
    }
    public void Button_Throw()
    {
        Inventory.Instance.ThrowingParent.TryOpenThrow();
        Selection_AllOff();
    }

    public void ButtonRegister(Slot _slot)
    {
        if (_slot != null)
        {
            _slot.SetRegister(true);
            if (_slot.item.itemType == Enums.ItemType.Production_Item)
            {
                if (UiManager.Instance.quickSlot1.invenSlot != null)
                {
                    if (UiManager.Instance.quickSlot1.invenSlot != _slot)
                    {
                        UiManager.Instance.quickSlot1.invenSlot.SetRegister(false);
                    }
                }
                UiManager.Instance.quickSlot1.AddRegister(_slot, _slot.item, _slot.itemCount, UiManager.Instance.quickSlot1);
            }
            else if (_slot.item.itemType == Enums.ItemType.weapon_Equiptment_Item)
            {
                if (UiManager.Instance.quickSlot2.invenSlot != null)
                {
                    if (UiManager.Instance.quickSlot2.invenSlot != _slot)
                    {
                        UiManager.Instance.quickSlot2.invenSlot.SetRegister(false);
                    }
                }
                UiManager.Instance.quickSlot2.AddRegister(_slot, _slot.item, _slot.itemCount, UiManager.Instance.quickSlot2);
            }
            else if (_slot.item.itemType == Enums.ItemType.supply_Item)
            {
                if (UiManager.Instance.quickSlot3.invenSlot != null)
                {
                    if (UiManager.Instance.quickSlot3.invenSlot != _slot)
                    {
                        UiManager.Instance.quickSlot3.invenSlot.SetRegister(false);
                    }
                }
                UiManager.Instance.quickSlot3.AddRegister(_slot, _slot.item, _slot.itemCount, UiManager.Instance.quickSlot3);
            }
            else if (_slot.item.itemType == Enums.ItemType.Defence_Equiptment_Item)
            {
                if (UiManager.Instance.quickSlot4.invenSlot != null)
                {
                    if (UiManager.Instance.quickSlot4.invenSlot != _slot)
                    {
                        UiManager.Instance.quickSlot4.invenSlot.SetRegister(false);
                    }
                }
                UiManager.Instance.quickSlot4.AddRegister(_slot, _slot.item, _slot.itemCount, UiManager.Instance.quickSlot4);
            }
        }
    }


    public void Equipt(Item _item)
    {
        Debug.Log("장비!");
    }

    public void Use(Item _item)
    {
        if (_item.itemType == Enums.ItemType.Production_Item)
        {
            Debug.Log("상호 작용!");
            //UiManager.Instance.quickSlot1.SetSlotCount_q(-1);
        }
        else if (_item.itemType == Enums.ItemType.supply_Item)
        {
            Debug.Log("아이템 사용!");
            //UiManager.Instance.quickSlot2.SetSlotCount_q(-1);
        }
        Inventory.Instance.curSlot.SetSlotCount(-1);
    }
    public void Use(Item _item, QuickSlot quickSlot)
    {
        if (_item.itemType == Enums.ItemType.Production_Item)
        {
            Debug.Log("상호 작용!");
        }
        else if (_item.itemType == Enums.ItemType.supply_Item)
        {
            Debug.Log("아이템 사용!");
        }

        Slot findSlot = Inventory.Instance.FindRegisterSlotFromInventory(quickSlot);
        if (findSlot) findSlot.SetSlotCount(-1);
        else Debug.Log("퀵에 등록된 아이템이 없음");

        quickSlot.SetSlotCount_q(-1);
    }
}
