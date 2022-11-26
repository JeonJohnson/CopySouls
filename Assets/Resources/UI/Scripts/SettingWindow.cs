using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingWindow : MonoBehaviour
{
    static public SettingWindow Instance;
    public WindowIndex index = WindowIndex.SettingWindow;
    public static bool SettingActivated = false;
    public float mouseSensivility;
    [SerializeField] GameObject SettingBase;
    [SerializeField] GameObject advancedSettingWindow;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public void TryOpenSetting()
    {
        SettingActivated = !SettingActivated;
        if (SettingActivated)
        {
            OpenSetting();
        }
        else
        {
            CloseSetting();
        }
    }
    private void OpenSetting()
    {
        UiManager.Instance.WindowProcedure(true);
        GetComponent<Canvas>().sortingOrder = UiManager.WindowProcedureIndex;
        SettingBase.SetActive(true);
    }
    private void CloseSetting()
    {
        UiManager.Instance.WindowProcedure(false);

        SettingBase.SetActive(false);
        SettingActivated = false;
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
