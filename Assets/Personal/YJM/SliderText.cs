using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Slider slider;
    float value = 0f;

    public void UpdateText()
    {
        text.text = ((int)(slider.value * 10)).ToString();
    }

    public void UpdateMouseSensivi()
    {
        //GameManager.Instance.mouseSensivility = 1 + slider.value * 2;
    }
}
