using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionWIndow : MonoBehaviour
{
    public static InteractionWIndow Instance;

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

    [SerializeField] CanvasGroup CanvasGroup;

    float canvasAlpha = 0f;
    public bool isEnabled = false;

    public void InitContents(string text0)
    {
        infoText0.text = text0;
    }

    public void ShowItemInfo()
    {
        isEnabled = true;
        this.gameObject.SetActive(true);
    }

    public void HideItemInfo()
    {
        isEnabled = false;
        print("falsing");
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
