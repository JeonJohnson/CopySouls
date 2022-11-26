using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class Setting_ScreenResOption : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown windowModDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;
    int windowModNum;
    public FullScreenMode screenMode;

    private void Start()
    {
        InitRes();
        InitWindow();
    }

    void InitRes()
    {
        resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();

        int optionNum = 0;

        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();
    }

    void InitWindow()
    {
        windowModDropdown.options.Clear();
        int optionNum = 0;
        for (int i = 0; i < System.Enum.GetValues(typeof(FullScreenMode)).Length; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = Enum.GetName(typeof(FullScreenMode), i).ToString();
            print(option.text);
            windowModDropdown.options.Add(option);
            if (Enum.GetName(typeof(FullScreenMode),i).ToString() == Screen.fullScreenMode.ToString())
            {
                print("¸Â³×");
                windowModDropdown.value = optionNum;
                optionNum++;
                screenMode = (FullScreenMode)i;
            }
        }
        windowModDropdown.RefreshShownValue();
    }

    public void ResDropboxOptionChanged(int i)
    {
        resolutionNum = i;
    }

    public void WindowModDropboxOptionChanged(int i)
    {
        windowModNum = i;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}
