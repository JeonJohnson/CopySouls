using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSceneManager : Manager<IntroSceneManager>
{
    public Image logoImg;
    public float fadeSpd;

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
            SceneManager.LoadScene((int)eSceneChangeTestIndex.Title);
        }
    }
}
