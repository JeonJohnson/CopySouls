using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleButtonEvents : MonoBehaviour
{
    [SerializeField] Image logoImage;

    // Start is called before the first frame update
    void Start()
    {
        logoImage.DOColor(new Color(1f, 1f, 1f, 0.8f), 2f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotoIngameScene()
    {
        LoadingSceneController.Instance.LoadScene((int)eSceneChangeTestIndex.InGame);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SettingWindowOpen()
    {
        Debug.Log("설정 열림!");
    }
}
