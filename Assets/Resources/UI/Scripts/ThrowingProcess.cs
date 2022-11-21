using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThrowingProcess : MonoBehaviour
{
    public static bool ThrowingActivated = false;

    public InputField ThrowInputField;
    public GameObject Throw_Button;
    public GameObject ThrowAll_Button;
    public GameObject ThrowCancel_Button;
    public void TryOpenThrow()
    {
        if (Inventory.inventoryActivated && !DivisionProcess.DivisionActivated)
        {
            ThrowingActivated = !ThrowingActivated;
            if (ThrowingActivated) OpenThrow();
            else CloseThrow();
        }
        else return;
    }

    public void TryOpenThrow(Vector3 vec)
    {
        gameObject.transform.position = vec;
        if (Inventory.inventoryActivated && !DivisionProcess.DivisionActivated)
        {
            ThrowingActivated = !ThrowingActivated;
            if (ThrowingActivated) OpenThrow();
            else CloseThrow();
        }
        else return;
    }

    private void OpenThrow()
    {
        gameObject.SetActive(true);
    }
    private void CloseThrow()
    {
        gameObject.SetActive(false);
    }


    //Button
    public void Button_SelectionThrowing()
    {
        Inventory.Instance.SelectionParent.Selection_AllOff();
        TryOpenThrow();
    }

    public void Button_Throw()
    {
        if (Inventory.Instance.curSlot == null) return;

        

        if (ThrowInputField.text != "")
        {
            int ThrowCount = int.Parse(ThrowInputField.text);
            if (ThrowCount > 0 && ThrowCount <= Inventory.Instance.curSlot.itemCount)
            {
                if (ThrowInputField.text != "") ThrowInputField.text = "";
                Throw(Inventory.Instance.curSlot, ThrowCount);
            }
            else
            {
                if (ThrowInputField.text != "") ThrowInputField.text = "";
                return;
            }
        }

        gameObject.SetActive(false);
        ThrowingActivated = !ThrowingActivated;
        Inventory.Instance.curSlot = null;
    }
    public void Button_ThrowAll()
    {
        if (Inventory.Instance.curSlot == null) return;

        if (ThrowInputField.text != "") ThrowInputField.text = "";

        Throw(Inventory.Instance.curSlot, Inventory.Instance.curSlot.itemCount);
        gameObject.SetActive(false);
        ThrowingActivated = !ThrowingActivated;
        Inventory.Instance.curSlot = null;
    }
    public void Button_ThrowCancel()
    {
        if (ThrowInputField.text != "") ThrowInputField.text = "";

        gameObject.SetActive(false);
        ThrowingActivated = !ThrowingActivated;
        Inventory.Instance.curSlot = null;
    }

    private void Throw(Slot _curSlot, int _itemCount)
    {
        if (_curSlot.isQuick) _curSlot.curRegisterQuickSlot.SetSlotCount_q(-_itemCount);

        Debug.Log(_curSlot.item.objName + " " + _itemCount + "�� ������");
        _curSlot.SetSlotCount(-_itemCount);
    }
}
