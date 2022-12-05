using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleButtonEvents : MonoBehaviour
{
    [SerializeField] Image logoImage;
    [SerializeField] GameObject lightningBoltEffect;

    // Start is called before the first frame update
    void Start()
    {
        logoImage.DOColor(new Color(1f, 1f, 1f, 0.8f), 2f).SetLoops(-1, LoopType.Yoyo);
        Lightning();
        SoundManager.Instance.PlayBgm("fantasy");
    }

    float timer = 8f;
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0f)
        {
            timer = 10f;
            Lightning();
        }
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
        Debug.Log("¼³Á¤ ¿­¸²!");
    }

    Material lightningMat;
    public void Lightning()
    {
        Debug.Log("´«»Í!!!!");
        lightningMat = lightningBoltEffect.GetComponent<MeshRenderer>().sharedMaterial;
        Color originCol = lightningMat.color;
        lightningMat.DOColor(new Color(1f,1f,1f,0.9f),0.05f).OnComplete(() => { lightningMat.DOColor(originCol, 2f); });
    }
}
