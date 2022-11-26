using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : Manager<InGameManager>
{
    public bool isBossCombat = false;

    public Vector3 playerInitPos;
    
    private GameObject fogWallObj = null;
    private ScreenEffect screenEffect;
    public float gameEndingEffectTime;


    public void BossCombatStart()
    {
        isBossCombat = true;
        fogWallObj.SetActive(true);
    }

    public void BossDeath()
    {
        isBossCombat = false;
        StartCoroutine(GotoTitleSceneCoroutine());
    }

    IEnumerator GotoTitleSceneCoroutine()
    {
        //yield return new WaitForSeconds(4f);
        yield return StartCoroutine(GameEndScreenEffectCoroutine());
        LoadingSceneController.Instance.LoadScene((int)eSceneChangeTestIndex.Title);

        screenEffect.SetGrayScaleAmount(0f);
        UiManager.Instance.SetBlurAmount(0f);
        Time.timeScale = 1f;
    }

	IEnumerator GameEndScreenEffectCoroutine()
	{
        float time = 0f;
        while (time < gameEndingEffectTime)
        {//ratio : 0 to 1
            float ratio = time / gameEndingEffectTime;
            screenEffect.SetGrayScaleAmount(ratio);
            UiManager.Instance.SetBlurAmount(ratio);
            Time.timeScale = Mathf.Lerp(1f, 0.0f, ratio);
            time += Time.unscaledDeltaTime;
            
            yield return null;
        }

        screenEffect.SetGrayScaleAmount(1f);
        UiManager.Instance.SetBlurAmount(1f);
        Time.timeScale = 0f;
    }

    IEnumerator GameEndCoroutine()
    {
        yield return StartCoroutine(GameEndCoroutine());



    }

	private void Awake()
    {
        if (fogWallObj == null)
        {
            fogWallObj = GameObject.FindGameObjectWithTag("FogWall");
        }
        fogWallObj.SetActive(false);

        
    }

	void Start()
    {
        SetPlayer();
        screenEffect = Camera.main.GetComponent<ScreenEffect>();
    }

    void Update()
    {
 
    }

    void SetPlayer()
    {
        Vector3 startPos = playerInitPos; //ㄹㅇ시작 위치
        //Vector3 startPos = new Vector3(3.6f, -7.2f, 64.4f); //보스 위치 
        Vector3 startRot = Vector3.zero;
        PlayerLocomove.instance.cc.enabled = false;
        Player.instance.transform.position = startPos;
        PlayerLocomove.instance.cameraManager.gameObject.transform.position = startPos;
        PlayerLocomove.instance.cc.enabled = true;

        //StartCoroutine(PlayPlayerStartAnim());
    }

    IEnumerator PlayPlayerStartAnim()
    {
        Player.instance.animator.SetTrigger("CrouchToStand");
        Player.instance.SetState(Enums.ePlayerState.Hit);
        Player.instance.animator.speed = 0f;
        yield return new WaitForSeconds(2f);
        Player.instance.animator.speed = 1f;
        yield return null;
    }


}
