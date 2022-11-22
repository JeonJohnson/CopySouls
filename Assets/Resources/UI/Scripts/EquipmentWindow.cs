using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindow : MonoBehaviour
{
    static public EquipmentWindow Instance;
    public static bool EquipmentActivated = false;

    [SerializeField]
    private GameObject EquiptmentWindowPanel;
    [SerializeField]
    private GameObject SlotParent;
    [SerializeField]
    private QuickSlot[] slots;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    void Start()
    {
        slots = SlotParent.GetComponentsInChildren<QuickSlot>();
    }

    public void TryOpenEquiptment()
    {
        EquipmentActivated = !EquipmentActivated;
        //UiManager.UIActivated = EquipmentActivated;
        if (EquipmentActivated)
        {
            OpenEquiptment();
        }
        else
        {
            CloseEquiptment();
        }
    }
    
    private void OpenEquiptment()
    {
        EquiptmentWindowPanel.SetActive(true);
    }
    private void CloseEquiptment()
    {
        EquiptmentWindowPanel.SetActive(false);
        EquipmentActivated = false;
    }

    public QuickSlot GetEquiptSlot(Enums.ItemType _itemType)
    {
        if (_itemType == Enums.ItemType.supply_Item || _itemType == Enums.ItemType.Production_Item) return null;
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].OnlyType == _itemType)
            {
                return slots[i];
            }
        }
        return null;
    }

}
