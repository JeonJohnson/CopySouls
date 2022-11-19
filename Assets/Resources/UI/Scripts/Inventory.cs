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
    //선택 창
    public static bool SelectionActivated = false;


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
    //[SerializeField]
    public GameObject SelectParent;

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
            if (SelectionActivated) CloseSelection();
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

    public void TryOpenSelection(Enums.ItemType _itemType, Vector3 Vec)
    {
        
        if(inventoryActivated && !DivisionActivated && !SelectionActivated)
        {
            Debug.Log("선택창 띄우기");
            OpenSelection(_itemType,Vec);
        }

        //if (inventoryActivated)
        //{
        //    DivisionActivated = !DivisionActivated;
        //    if (DivisionActivated) OpenDivision();
        //    else CloseDivision();
        //}
        //else return;
    }

    public void OpenSelection(Enums.ItemType _itemType,Vector3 vec)
    {
        SelectionActivated = !SelectionActivated;
        SelectParent.SetActive(true);
        Vector3 vec1 = new Vector3(vec.x + 60f, vec.y - 15f);
        SelectParent.transform.position = vec1;
        if (_itemType == Enums.ItemType.supply_Item || _itemType == Enums.ItemType.Production_Item)
        {
            SelectParent.GetComponent<Selection>().selection_Use.SetActive(true);
            SelectParent.GetComponent<Selection>().selection_Register.SetActive(true);
        }
        else if (_itemType == Enums.ItemType.weapon_Equiptment_Item || _itemType == Enums.ItemType.Defence_Equiptment_Item)
        {
            SelectParent.GetComponent<Selection>().selection_Equipt.SetActive(true);
            SelectParent.GetComponent<Selection>().selection_Register.SetActive(true);
        }
    }

    public void CloseSelection()
    {
        SelectionActivated = !SelectionActivated;
        SelectParent.SetActive(false);
        SelectParent.GetComponent<Selection>().Selection_AllOff();
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

    public void Exit_Inventory_Button()
    {
        if (inventoryActivated && !DivisionActivated)
        {
            if (SelectionActivated) CloseSelection();
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

    public void SelectionEquipt_Button()
    {
        Equipt(curSlot.item);
        SelectParent.GetComponent<Selection>().Selection_AllOff();
        SelectionActivated = !SelectionActivated;
        curSlot = null;
    }
    public void SelectionUse_Button()
    {
        Use(curSlot.item);
        SelectParent.GetComponent<Selection>().Selection_AllOff();
        SelectionActivated = !SelectionActivated;
        curSlot = null;
    }
    public void QuickSlotUse(QuickSlot _quickSlot)
    {
        if(_quickSlot.item != null) Use(_quickSlot.item, _quickSlot);
    }

    public void SelectionRegister_Button()
    {
        ButtonRegister(curSlot);
        SelectParent.GetComponent<Selection>().Selection_AllOff();
        SelectionActivated = !SelectionActivated;
        curSlot = null;
    }
    public void ButtonRegister(Slot _slot)
    {
        if(_slot != null)
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
                UiManager.Instance.quickSlot1.AddRegister(_slot, _slot.item, _slot.itemCount);
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
                UiManager.Instance.quickSlot2.AddRegister(_slot, _slot.item, _slot.itemCount);
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
                UiManager.Instance.quickSlot3.AddRegister(_slot, _slot.item, _slot.itemCount);
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
                UiManager.Instance.quickSlot4.AddRegister(_slot, _slot.item, _slot.itemCount);
            }
        }
    }


    //=========================================================================
    // 여기가 실제 기능 구현 하는 곳
    //=========================================================================

    public void Equipt(Item _item)
    {
        Debug.Log("장비!");
    }

    public void Use(Item _item)
    {
        if (_item.itemType == Enums.ItemType.Production_Item)
        {
            Debug.Log("상호 작용!");
        }
        else if (_item.itemType == Enums.ItemType.supply_Item)
        {
            Debug.Log("아이템 사용!");
        }
        curSlot.SetSlotCount(-1);
    }
    public void Use(Item _item,QuickSlot quickSlot)
    {
        if(_item.itemType == Enums.ItemType.Production_Item)
        {
            Debug.Log("상호 작용!");
        }
        else if (_item.itemType == Enums.ItemType.supply_Item)
        {
            Debug.Log("아이템 사용!");
        }

        Slot findSlot =  FindRegisterSlotFromInventory(quickSlot);
        if (findSlot) findSlot.SetSlotCount(-1);
        else Debug.Log("퀵에 등록된 아이템이 없음");

        quickSlot.SetSlotCount_q(-1);
    }

    //=========================================================================

    public Slot FindRegisterSlotFromInventory(QuickSlot _quickSlot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isQuick)
            {
                if(_quickSlot.item == slots[i].item)
                {
                    return slots[i];
                }
            }
        }
        return null;
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
                        if (curSlot.isQuick)
                        {
                            if(curSlot.item.itemType == Enums.ItemType.Production_Item)
                            {
                                UiManager.Instance.quickSlot1.SetSlotCount_q(-divisionCount);
                            }
                            else if (curSlot.item.itemType == Enums.ItemType.supply_Item)
                            {
                                UiManager.Instance.quickSlot2.SetSlotCount_q(-divisionCount);
                            }
                        }
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

    public void DivisionCancel()
    {
        if(DivisionInputField.text != "")
        {
            DivisionInputField.text = "";
        }
    }

    
}
