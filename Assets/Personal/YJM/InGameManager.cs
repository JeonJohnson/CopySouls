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
    public float creditScrollTime;
    public float creditScrollSpd;

    public bool isCreditEnd = false;

    public void BossCombatStart()
    {
        isBossCombat = true;
        fogWallObj.SetActive(true);
    }

    public void BossDeath()
    {
        isBossCombat = false;
        UiManager.Instance.endingCredit.gameObject.SetActive(true);
        StartCoroutine(GotoTitleSceneCoroutine());
    }

    IEnumerator GotoTitleSceneCoroutine()
    {
        //스크린 이펙트 키면서 엔딩 크레딧 알파값 넣기
        //끝나면 크레딧 스크롤 ㄱㄱ
        yield return StartCoroutine(GameEndScreenEffectCoroutine());
        yield return StartCoroutine(EndingCreditFadeCoroutine());
     
        isCreditEnd = true;
        UiManager.Instance.endingCredit.pressAnyKeyTxt.gameObject.SetActive(true);
    }

	IEnumerator GameEndScreenEffectCoroutine()
	{
        float time = 0f;
        while (time < gameEndingEffectTime)
        {//ratio : 0 to 1
            float ratio = time / gameEndingEffectTime;
            screenEffect.SetGrayScaleAmount(ratio);
            UiManager.Instance.SetBlurAmount(ratio);
            UiManager.Instance.endingCreditCanvasGroup.alpha = ratio;
            //UiManager.Instance.endingCreditCanvas.SetScrollVal(ratio);
            Time.timeScale = Mathf.Lerp(1f, 0.0f, ratio);
            
            time += Time.unscaledDeltaTime;
            
            yield return null;
        }

        screenEffect.SetGrayScaleAmount(1f);
        UiManager.Instance.SetBlurAmount(1f);
        Time.timeScale = 0f;
    }

    IEnumerator EndingCreditFadeCoroutine()
    {
        float time = 0;
        while (time < creditScrollTime)
        {//ratio : 0 to 1
            float ratio = time / creditScrollTime;
            UiManager.Instance.endingCredit.SetScrollVal(ratio);
            time += Time.unscaledDeltaTime;

           yield return null;
        }
    }

    //IEnumerator GameEndCoroutine()
    //{
    //    yield return StartCoroutine(GameEndCoroutine());



    //}

	private void Awake()
    {
        if (fogWallObj == null)
        {
            fogWallObj = GameObject.FindGameObjectWithTag("FogWall");
        }
        fogWallObj.SetActive(false);

        isCreditEnd = false;
    }

	void Start()
    {
        SetPlayer();
        screenEffect = Camera.main.GetComponent<ScreenEffect>();
    }

    void Update()
    {
        if (isCreditEnd)
        {
            if (Input.anyKeyDown)
            {
                UiManager.Instance.endingCreditCanvasGroup.gameObject.SetActive(false);
                UiManager.Instance.screenEffectCanvas.gameObject.SetActive(false);
                screenEffect.SetGrayScaleAmount(0f);
                LoadingSceneController.Instance.LoadScene((int)eSceneChangeTestIndex.Title);
                
            }
        }
    }

    void SetPlayer()
    {
        //Vector3 startPos = playerInitPos; //ㄹㅇ시작 위치
        Vector3 startPos = new Vector3(3.6f, -7.2f, 64.4f); //보스 위치 
        Vector3 startRot = Vector3.zero;
        PlayerLocomove.instance.cc.enabled = false;        Player.instance.transform.position = startPos;
        PlayerLocomove.instance.cameraManager.gameObject.transform.position = startPos;
        PlayerLocomove.instance.cameraManager.gameObject.transform.eulerAngles = startRot;
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
