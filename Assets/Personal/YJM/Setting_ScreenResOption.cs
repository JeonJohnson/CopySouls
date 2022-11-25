using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting_ScreenResOption : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;

    private void Start()
    {
        InitUI();
    }

    void InitUI()
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

    public void DropboxOptionChanged(int i)
    {
        resolutionNum = i;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, FullScreenMode.FullScreenWindow);
    }
}
