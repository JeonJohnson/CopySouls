using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public void Button_IconSetting()
    {

    }
    public void Button_IconInventory()
    {
        Inventory.Instance.TryOpenInventory();
    }
    public void Button_IconEquiptment()
    {
        EquipmentWindow.Instance.TryOpenEquiptment();
    }
    public void Button_IconChatting()
    {

    }
}
