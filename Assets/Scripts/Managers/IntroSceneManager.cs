using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class IntroSceneManager : Manager<IntroSceneManager>
{
    public Image logoImg0;
    public Image logoImg1;
    public Image fadeImg;

    public float fadeSpd;
    bool isChanged = false;

	public void Awake()
	{
        //logoImg.color = Color.white;
        StartCoroutine(LogoAnimCoro());
    }

    void Update()
    {
        //Color temp = logoImg.color;
        //temp.a -= Time.deltaTime * fadeSpd;
        //logoImg.color = temp;

        //if (temp.a <= 0f)
        //{
        //    if (isChanged == false) SceneManager.LoadScene((int)eSceneChangeTestIndex.Title);
        //    //if (isChanged == false) LoadingSceneController.Instance.LoadScene((int)eSceneChangeTestIndex.Title);
        //    isChanged = true;
        //    //SceneManager.LoadScene((int)eSceneChangeTestIndex.Title);
        //}
    }

    IEnumerator LogoAnimCoro()
    {
        yield return new WaitForSeconds(0.7f);
        logoImg0.DOFillAmount(1f, 1f).SetEase(Ease.OutExpo);

        yield return new WaitForSeconds(0.2f);
        logoImg1.DOFillAmount(1f, 1f).SetEase(Ease.OutExpo);

        yield return new WaitForSeconds(2.5f);
        Color fadeCol = new Vector4(0f, 0f, 0f, 1f);
        fadeImg.DOColor(fadeCol, 0.3f);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int)eSceneChangeTestIndex.Title);
    }

    void LogoAnim()
    {
        
    }
}
