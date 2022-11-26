using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;
using DG.Tweening;

public class SystemInfoWindow : MonoBehaviour
{
    public static SystemInfoWindow Instance;
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
        RenderSettings.fog = true;
    }

    [SerializeField] Text diedText;
    [SerializeField] Image bgImage;
    [SerializeField] CanvasGroup canvasGroup;

    public void PlayEffect()
    {
        print("1");
        diedText.gameObject.SetActive(true);
        bgImage.gameObject.SetActive(true);
        bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0f);
        diedText.color = new Color(diedText.color.r, diedText.color.g, diedText.color.b, 0f);
        canvasGroup.alpha = 1f;
        print("2");
        StartCoroutine(BgEffectCoro());
        StartCoroutine(TextEffectCoro());
        print("3");
    }

    float canvasAlpha = 1f;
    IEnumerator BgEffectCoro()
    {
        bgImage.DOColor(new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 1f), 1f);
        yield return null;
    }

    IEnumerator TextEffectCoro()
    {
        diedText.DOColor(new Color(diedText.color.r, diedText.color.g, diedText.color.b, 1f), 1f);
        yield return new WaitForSeconds(2f);
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return null; 
        }
    }
}
