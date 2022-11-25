using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingWindow : MonoBehaviour
{
    static public SettingWindow Instance;
    public static bool SelectionActivated = false;
    public float mouseSensivility;
    [SerializeField] GameObject SettingBase;

    [SerializeField] GameObject advancedSettingWindow;

    private void Awake()
    {

        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public void TryOpenInventory()
    {
        SelectionActivated = !SelectionActivated;
        if (SelectionActivated)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }
    private void OpenInventory()
    {
            SettingBase.SetActive(true);
    }
    private void CloseInventory()
    {
            SettingBase.SetActive(false);
            SelectionActivated = false;
        advancedSettingWindow.SetActive(false);
    }

    public void SetMouseSensivility(float i)
    {
        //감도조절
    }

    public void GotoMainMenu()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
