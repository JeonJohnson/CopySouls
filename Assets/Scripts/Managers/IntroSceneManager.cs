using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSceneManager : Manager<IntroSceneManager>
{
    public Image logoImg;
    public float fadeSpd;
    bool isChanged = false;

	public void Awake()
	{
        logoImg.color = Color.white;
	}

    void Update()
    {
        Color temp = logoImg.color;
        temp.a -= Time.deltaTime * fadeSpd;
        logoImg.color = temp;

        if (temp.a <= 0f)
        {
            if (isChanged == false) SceneManager.LoadScene((int)eSceneChangeTestIndex.Title);
            //if (isChanged == false) LoadingSceneController.Instance.LoadScene((int)eSceneChangeTestIndex.Title);
            isChanged = true;
            //SceneManager.LoadScene((int)eSceneChangeTestIndex.Title);
        }
    }
}
