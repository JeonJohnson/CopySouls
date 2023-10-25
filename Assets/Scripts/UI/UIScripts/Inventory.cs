using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Structs;

enum itemNameIndex
{
    DefaultWeapon = 5,
    DefaultDefence = 6,
    End,
}


public class Inventory : MonoBehaviour
{
    static public Inventory Instance;
    public static bool inventoryActivated = false;

    private Dictionary<string, Item> Dic_items = new Dictionary<string, Item>();
    //private List<GameObject> All_items = new List<GameObject>();


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
    public Slot[] slots;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    void Start()
    {
        slots = SlotParent.GetComponentsInChildren<Slot>();
        SetAll_Items();
        GetItems();
    }

    private void Update()
    {

    }


    public void TryOpenInventory()
    {
        inventoryActivated = !inventoryActivated;

        if (inventoryActivated)
        {
            OpenInventory();
            UiManager.UIActivated = true;
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
            inventoryActivated = true;
            UiManager.Instance.WindowProcedure(true, GetComponent<Canvas>());
        }
    }
    private void CloseInventory()
    {
        if (!DivisionProcess.DivisionActivated && !ThrowingProcess.ThrowingActivated)
        {
            if (SelectionProcess.SelectionActivated) SelectionParent.CloseSelection();
            InventoryBase.SetActive(false);
            inventoryActivated = false;
            UiManager.Instance.WindowProcedure(false, GetComponent<Canvas>());
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
                        if (slots[i].isQuick) slots[i].curRegisterQuickSlot.SetSlotCount_q(_count);
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

    public void GetItem(Item _item)
    {
        ItemIn(_item);
    }
    public void GetItem(string _itemName,Vector3 pos, int count = 1)
    {
        Debug.Log(_itemName);
        GameObject itemObj = ObjectPoolingCenter.Instance.LentalObj(_itemName, count);

        if (itemObj.tag == "Item")
        {
            Item item = itemObj.GetComponent<Item>();
            if(item.itemType == Enums.ItemType.supply_Item) item.Count = count;
        }
        itemObj.transform.position = pos;
    }
    public void GetItem(string _itemName,int count = 1)
    {
        ItemIn(Dic_items[_itemName], count);
    }

    public void GetItems()
    {
        GetItem("Shield0_Item"); // 60

        GetItem("Shield1_Item"); // 40
        GetItem("Shield2_Item"); // 20
        GetItem("Shield3_Item"); // 10

        GetItem("Sword0_Item");  // 60
        GetItem("Sword1_Item");  // 40
        GetItem("Sword2_Item");  // 20
        GetItem("Sword3_Item");  // 10

        GetItem("Potion_Item", 10);  //70


    }
    public void Routing(Vector3 pos)
    {
        Vector3 Pos1 = new Vector3(pos.x,pos.y + 1f,pos.z);
        int index = Random.Range(0, 11);
        switch (index)
        {
            case 0: Draw("Shield0_Item", 100, Pos1);  //Lv 0     25
                break;
            case 1: Draw("Sword0_Item", 100, Pos1);   //Lv 0     25      
                break;
            case 2: Draw("Shield1_Item", 100, Pos1);  //Lv 1     20  
                break;
            case 3: Draw("Sword1_Item", 100, Pos1);   //Lv 1     20
                break;
            case 4: Draw("Shield2_Item", 100, Pos1);  //Lv 2     15
                break;
            case 5: Draw("Sword2_Item", 100, Pos1);   //Lv 2     15
                break;
            case 6: Draw("Shield3_Item", 100, Pos1);  //Lv 3      5
                break;
            case 7: Draw("Sword3_Item", 100, Pos1);   //Lv 3      5
                break;
            case 8:
            case 9:
            case 10:
                Draw("Potion_Item", 100, pos);       //potion        85
                break;
            default:
                break;
        }
    }

    private bool Draw<T>(T itemname,float probability,Vector3 pos)
    {
        if(RandDraw(probability))
        {
            GameObject item = ObjectPoolingCenter.Instance.LentalObj(itemname.ToString());
            item.transform.position = pos;
            Debug.Log(item.name);
            Debug.Log("뽑기 성공!");
            return true;
        }
        Debug.Log("뽑기 실패");
        return false;
    }
    private bool RandDraw(float probability)
    {
        bool Success = false;
        float Value;
        float total = Dic_items.Count;
        if (probability == 0) Value = -1;
        else Value = (probability * total) / 100f;
        int Rand = Random.Range(0, (int)total);
        if(Rand <= Value) Success = true;
        return Success;
    }

    private void SetAll_Items()
    {
        foreach (GameObject item in ObjectPoolingCenter.Instance.prefabs)
        {
            if (item.GetComponent<Item>() != null)
            {
                Dic_items.Add(item.name, item.GetComponent<Item>());
            }
        }
    }

    //풀링센터에 있는 원본 프리펩

    // 인베토리 생성시 풀링센터에 등록된 원본 프리펩을 인벤토리 딕셔너리로 받아옴

}
