using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : MonoBehaviour
{
    static public SettingWindow Instance;
    public static bool SettingActivated = false;
    public float mouseSensivility;
    [SerializeField] GameObject SettingBase;
    [SerializeField] GameObject advancedSettingWindow;

    [SerializeField] Slider[] Sliders;

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
        UiManager.Instance.WindowProcedure(true, GetComponent<Canvas>());
        SettingBase.SetActive(true);
    }
    private void CloseSetting()
    {
        UiManager.Instance.WindowProcedure(false, GetComponent<Canvas>());
        SettingBase.SetActive(false);
        SettingActivated = false;
        advancedSettingWindow.SetActive(false);
    }

    public void SetMouseSensivility(float i)
    {
        GameManager.Instance.mouseSensivility = i * 4;
    }

    public void GotoMainMenu()
    {
        LoadingSceneController.Instance.LoadScene((int)eSceneChangeTestIndex.Title);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
