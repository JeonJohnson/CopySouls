using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventManager : Manager<EventManager>
{
	public Button geunheeSceneBtn;

	public void TestGotoScene(int index)
	{
		SceneManager.LoadScene(index);
	}

	public void Start()
	{
		geunheeSceneBtn.onClick.AddListener(() => TestGotoScene((int)eSceneChangeTestIndex.Geunhee));
	}

}
