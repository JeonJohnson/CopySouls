using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImage : MonoBehaviour
{
    [SerializeField] Image fadeImg;
    private void Start()
    {
        fadeImg.DOFade(0f, 0.3f).OnComplete(() => { DisableThisObj(); });
    }

    void DisableThisObj()
    {
        this.gameObject.SetActive(false);
    }
}
