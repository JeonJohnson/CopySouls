using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

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
    }

    [SerializeField] Text diedText;
    [SerializeField] Image bgImage;
    [SerializeField] CanvasGroup canvasGroup;
 
    public void PlayEffect()
    {
        print("1");
        diedText.gameObject.SetActive(true);
        bgImage.gameObject.SetActive(true);
        print("2");
        StartCoroutine(BgEffectCoro());
        StartCoroutine(TextEffectCoro());
        print("3");
    }

    float bgAlpha = 0f;
    float diedAlpha = 0f;

    float canvasAlpha = 1f;
    IEnumerator BgEffectCoro()
    {
        while (bgAlpha <= 1f)
        {
            bgAlpha += Time.deltaTime * 3f;
            bgImage.color = new Vector4(1f, 1f, 1f, bgAlpha);
            yield return null;
        }
    }

    IEnumerator TextEffectCoro()
    {
        Color col = diedText.color;
        while (diedAlpha <= 1f)
        {
            diedAlpha += Time.deltaTime * 1f;
            diedText.color = new Vector4(col.r, col.g, col.b, diedAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        while (canvasGroup.alpha > 0f)
        {
            canvasAlpha -= Time.deltaTime;
            canvasGroup.alpha = canvasAlpha;
            yield return null; 
        }
    }
}
