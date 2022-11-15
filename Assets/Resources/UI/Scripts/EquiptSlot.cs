using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquiptSlot : Slot
{
    public static int SlotIndex = 1;

    void Start()
    {
        text_Count.text = SlotIndex.ToString();
        SlotIndex++;
    }

    void Update()
    {
        
    }
}
