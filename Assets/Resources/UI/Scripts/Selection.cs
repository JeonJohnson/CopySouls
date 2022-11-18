using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public GameObject selection_Equipt;
    public GameObject selection_Use;
    public GameObject selection_Register;

    public void Selection_EquiptOnOff(bool value) { selection_Equipt.gameObject.SetActive(value); }
    public void Selection_UseOnOff(bool value) { selection_Use.gameObject.SetActive(value); }
    public void Selection_RegisterOnOff(bool value) { selection_Register.gameObject.SetActive(value); }

    public void Selection_AllOff()
    {
        gameObject.SetActive(false);
        selection_Equipt.SetActive(false);
        selection_Use.SetActive(false);
        selection_Register.SetActive(false);
    }

    
}
