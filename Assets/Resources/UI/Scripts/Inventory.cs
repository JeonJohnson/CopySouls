using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    static public Inventory Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //인벤토리 활성화시 모든 포커스가 인벤토리로 갈수 있도록 해주는 정적변수
    public static bool inventoryActivated = false;

    //public static bool SortActivated = false;

    public Slot curSlot;

    [SerializeField]
    public GameObject InventoryBase;
    //grid_settingUI
    [SerializeField]
    private GameObject SlotParent;
    [SerializeField]
    private GameObject DivisionParent;
    [SerializeField]
    public InputField DivisionInputField;
    [SerializeField]
    private Slider slider;


    //슬롯들
    [SerializeField]
    private Slot[] slots;
    void Start()
    {
        slots = SlotParent.GetComponentsInChildren<Slot>();
        if (SlotParent.activeSelf) SlotParent.SetActive(false);
    }
    public void update()
    {
        TryOpenInventory();
    }
    public void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;
            if (inventoryActivated) OpenInventory();
            else CloseInventory();
        }
    }

    private void OpenInventory()
    {
        InventoryBase.SetActive(true);
    }
    private void CloseInventory()
    {
        InventoryBase.SetActive(false);
    }

    public bool ItemIn(Item _item,int _count = 1)
    {
        if(_item.itemType == Enums.ItemType.Production_Item)
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

    public void Exit_Inventory_Button()
    {
        if(inventoryActivated) InventoryBase.SetActive(false);
    }


    public void Slider()
    {
        slider.minValue = 1;
        slider.maxValue = curSlot.itemCount;
        DivisionInputField.text = (slider.value).ToString();
    }

    

    public void DivisionTextUpdate()
    {
        if (!Input.GetKeyDown(KeyCode.Backspace))
        {
            int value = int.Parse(DivisionInputField.text);
            if (curSlot.itemCount < value)
            {
                value = curSlot.itemCount;
                DivisionInputField.text = value.ToString();
            }
            else if (value <= 0)
            {
                value = 1;
                DivisionInputField.text = value.ToString();
            }
            slider.value = value;
        }
    }

    //public void Division()
    //{
    //    if(curSlot != null)
    //    {
    //        if(SortInputField.text != "")
    //        {
    //            int divisionCount = int.Parse(SortInputField.text);
    //            if (divisionCount > curSlot.itemCount) divisionCount = curSlot.itemCount;
    //            else if (divisionCount <= 0) divisionCount = 1;
    //            else if (divisionCount == curSlot.itemCount)
    //            {
    //                TryOpenSort();
    //                return;
    //            }
    //            DivisionItemIn(curSlot.item, divisionCount);
    //            curSlot.SetSlotCount(-divisionCount);
    //            TryOpenSort();
    //        }
    //    }
    //}
    public void DivisionItemIn(Item _item,int _count)
    {
        //return true 받고 밖으로 하나 튕겨서 버리기
        if (_item.itemType == Enums.ItemType.Production_Item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    slots[i].AddItem(_item, _count);
                    return;
                }
            }
        }
    }

   
}
