using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Structs;

public class Inventory : MonoBehaviour
{
    static public Inventory Instance;
    public static bool inventoryActivated = false;

    public Slot curSlot;

    [SerializeField]
    public GameObject InventoryBase;
    [SerializeField]
    private GameObject SlotParent;
    public SelectionProcess SelectionParent;
    public ThrowingProcess ThrowingParent;
    public DivisionProcess DivisionParent;

    //슬롯들
    [SerializeField]
    private Slot[] slots;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    void Start()
    {
        slots = SlotParent.GetComponentsInChildren<Slot>();
    }
    public void TryOpenInventory()
    {
        inventoryActivated = !inventoryActivated;
        if (inventoryActivated)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }
    private void OpenInventory()
    {
        if (!DivisionProcess.DivisionActivated && !ThrowingProcess.ThrowingActivated)
        {
            InventoryBase.SetActive(true);
            Player.instance.ActivatePlayerInput(false);
        }
    }
    private void CloseInventory()
    {
        if (!DivisionProcess.DivisionActivated && !ThrowingProcess.ThrowingActivated)
        {
            if (SelectionProcess.SelectionActivated) SelectionParent.CloseSelection();
            InventoryBase.SetActive(false);
            Player.instance.ActivatePlayerInput(true);
            inventoryActivated = false;
        }
        else
        {
            Debug.Log("분할창 켜져있음");
        }
    }

    public bool ItemIn(Item _item, int _count = 1)
    {
        if (_item.itemType == Enums.ItemType.Production_Item || _item.itemType == Enums.ItemType.supply_Item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && slots[i].itemCount < slots[i].MaxCount)
                {
                    if (slots[i].item.objName == _item.objName)
                    {
                        slots[i].SetSlotCount(_count);
                        return true;
                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return true;
            }
        }
        return false;
    }

    public bool DivisionItemIn(Item _item, int _count)
    {
        if (_item.itemType == Enums.ItemType.Production_Item || _item.itemType == Enums.ItemType.supply_Item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    slots[i].AddItem(_item, _count);
                    return true;
                }
            }
        }
        return false;
    }

    public void Button_InventoryExit()
    {
        if (!DivisionProcess.DivisionActivated)
        {
            inventoryActivated = false;
            InventoryBase.SetActive(false);
        }
        else
        {
            Debug.Log("분할창 켜져있음");
        }
    }
}
