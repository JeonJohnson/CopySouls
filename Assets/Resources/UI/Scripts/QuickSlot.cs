using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : Slot
{
    public void DragRegister()
    {
        Debug.Log("드래그로 퀵 등록!");
    }
    public void ButtonRegister()
    {
        Debug.Log("버튼으로 퀵 등록!");
    }
}
