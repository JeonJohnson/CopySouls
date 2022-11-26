using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class YouDiedWindow : MonoBehaviour
{
    public static YouDiedWindow Instance;
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
 
    public void PlayDieEffect()
    {
        diedText.gameObject.SetActive(true);
        bgImage.gameObject.SetActive(true);
        StartCoroutine(BgEffectCoro());
        StartCoroutine(DiedEffectCoro());
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

    IEnumerator DiedEffectCoro()
    {
        while (diedAlpha <= 1f)
        {
            Color col = diedText.color;
            diedAlpha += Time.deltaTime * 1f;
            diedText.color = new Vector4(col.r, col.g, col.b, diedAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        while (canvasGroup.alpha > 0f)
        {
            canvasAlpha -= Time.deltaTime;
            canvasGroup.alpha = canvasAlpha;
            GameManager.Instance.PlayerDie();
            yield return null; 
        }
    }
}
