using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    static public Inventory Instance;

    //인벤토리 활성화시 모든 포커스가 인벤토리로 갈수 있도록 해주는 정적변수
    public static bool inventoryActivated = false;

    //나누기 창
    public static bool DivisionActivated = false;


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
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        slots = SlotParent.GetComponentsInChildren<Slot>();
       
    }
    public void update()
    {
        
        
        //esc -> 현재 창 끄기
        //enter -> 분할 창 끄기
    }
    public void TryOpenInventory()
    {
        inventoryActivated = !inventoryActivated;
        if (inventoryActivated) OpenInventory();
        else CloseInventory();
    }
    private void OpenInventory()
    {
        if (!DivisionActivated)
        {
            InventoryBase.SetActive(true);
        }
    }
    private void CloseInventory()
    {
        if (!DivisionActivated)
        {
            InventoryBase.SetActive(false);
        }
        else
        {
            Debug.Log("분할창 켜져있음");
        }
    }


    public void TryOpenDivision()
    {
        if (inventoryActivated)
        {
            DivisionActivated = !DivisionActivated;
            if (DivisionActivated) OpenDivision();
            else CloseDivision();
        }
        else return;
    }

    private void OpenDivision()
    {
        DivisionParent.SetActive(true);
    }
    private void CloseDivision()
    {
        DivisionParent.SetActive(false);
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
        if (inventoryActivated)
        {
            if (!DivisionActivated) InventoryBase.SetActive(false);
        }
    }
    public void Division_Button()
    {
        if (inventoryActivated)
        {
            if (DivisionActivated)
            {
                Division();
                DivisionParent.SetActive(false);
                DivisionActivated = !DivisionActivated;
                curSlot = null;
            }
        }
    }

    public void Exit_Division_Button()
    {
        if (inventoryActivated)
        {
            if (DivisionActivated)
            {
                DivisionCancel();
                DivisionParent.SetActive(false);
                DivisionActivated = !DivisionActivated;
                curSlot = null;
            }
        }
    }

    private void Exit_inventory_Button()
    {
        if (!DivisionActivated)
        {
            inventoryActivated = false;
            InventoryBase.SetActive(false);
        }
        else
        {
            Debug.Log("분할창 켜져있음");
        }
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

    public void Division()
    {
        if(curSlot != null)
        {
           if(DivisionInputField.text != "")
           {
               int divisionCount = int.Parse(DivisionInputField.text);
                if (divisionCount > 0 && divisionCount < curSlot.itemCount)
                {
                    if (DivisionInputField.text != "") DivisionInputField.text = "";

                    if (DivisionItemIn(curSlot.item, divisionCount))
                    {
                        curSlot.SetSlotCount(-divisionCount);
                    }
                    else
                    {
                        Debug.Log("인벤토리 칸이 부족합니다!!");
                        return;
                    }
                }
                else
                {
                    if (DivisionInputField.text != "") DivisionInputField.text = "";
                    return;
                }
           }
        }
    }
    public bool DivisionItemIn(Item _item,int _count)
    {
        if (_item.itemType == Enums.ItemType.Production_Item)
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

    public void DivisionCancel()
    {
        if(DivisionInputField.text != "")
        {
            DivisionInputField.text = "";
        }
    }
   
}
