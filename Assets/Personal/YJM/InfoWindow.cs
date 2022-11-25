using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    public static InfoWindow Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] Text infoText0;
    [SerializeField] Text infoText1;

    [SerializeField] CanvasGroup CanvasGroup;

    float canvasAlpha = 0f;
    public bool isEnabled = false;

    public void InitContents(string text0, string text1 = "E key : OK")
    {
        infoText0.text = text0;
        if(text1 != null)infoText1.text = text1;
    }

    public void ShowItemInfo()
    {
        isEnabled = true;
        this.gameObject.SetActive(true);
    }

    public void HideItemInfo()
    {
        isEnabled = false;
    }

    private void Update()
    {
        CanvasGroup.alpha = canvasAlpha;
        if (isEnabled)
        {
            canvasAlpha += Time.deltaTime * 5f;
            canvasAlpha = Mathf.Clamp(canvasAlpha, 0f, 1f);
        }
        else
        {
            canvasAlpha -= Time.deltaTime * 5f;
            if (canvasAlpha <= 0f) gameObject.SetActive(false);
            canvasAlpha = Mathf.Clamp(canvasAlpha, 0f, 1f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isEnabled = false;
        }
    }
}
