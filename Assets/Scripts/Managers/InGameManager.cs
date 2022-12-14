using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class InGameManager : Manager<InGameManager>
{
    public bool isBossCombat = false;
    public bool isStop = false;

    public Vector3 playerInitPos;
    [SerializeField] Vector3 lastBonfirePos;
    public Vector3 LastBonFirePos
    {
        get
        {
            return lastBonfirePos;
        }
        set
        {
            lastBonfirePos = value;
        }
    }
    
    private GameObject fogWallObj = null;

    private ScreenEffect screenEffect;
    public float gameEndingEffectTime;
    public float creditScrollTime;
    public float creditScrollSpd;
    public bool isCreditEnd = false;

    public float playerDeathEffectTime;
    public Camera diedUICam;

    public void BossCombatStart()
    {
        SoundManager.Instance.PlayBgmFade("war-is-coming");
        isBossCombat = true;
        fogWallObj.SetActive(true);
    }

    public void BossDeathEvent()
    {
        isBossCombat = false;
        UiManager.Instance.endingCredit.gameObject.SetActive(true);
        StartCoroutine(GotoTitleSceneCoroutine());
    }

    public void PlayerDeathEvent()
    {
        diedUICam.gameObject.SetActive(true);
        StartCoroutine(PlayerDiedCoroutine());
        //UnitManager.Instance.ResetAllEnemies();
    }

    IEnumerator GotoTitleSceneCoroutine()
    {
        TimeStopEffect(4f);
        yield return new WaitForSeconds(3f);
        //스크린 이펙트 키면서 엔딩 크레딧 알파값 넣기
        //끝나면 크레딧 스크롤 ㄱㄱ
        yield return StartCoroutine(GameEndScreenEffectCoroutine(gameEndingEffectTime));
        yield return new WaitForSecondsRealtime(2f);
        yield return StartCoroutine(EndingCreditFadeCoroutine());
        yield return StartCoroutine(EndingCreditScrollCoroutine());
     
        isCreditEnd = true;
        UiManager.Instance.endingCredit.pressAnyKeyTxt.gameObject.SetActive(true);
    }

	IEnumerator GameEndScreenEffectCoroutine(float maxTime)
	{
        float time = 0f;
        while (time < maxTime)
        {//ratio : 0 to 1
            float ratio = time / maxTime;
            screenEffect.SetGrayScaleAmount(ratio);
            UiManager.Instance.SetBlurAmount(ratio);
            //UiManager.Instance.endingCreditCanvasGroup.alpha = ratio;
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
        float alpha = 0;
        while (alpha < 1f)
        {//ratio : 0 to 1
            alpha += Time.unscaledDeltaTime;

           UiManager.Instance.endingCreditCanvasGroup.alpha = alpha;
           yield return null;
        }
        
    }

    IEnumerator EndingCreditScrollCoroutine()
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
        

    //------------------------------------------------------------------------------//
    IEnumerator PlayerDiedScreenEffectCoroutine(float maxTime)
    {
        float time = 0f;
        while (time < maxTime)
        {//ratio : 0 to 1
            float ratio = time / maxTime;
            screenEffect.SetGrayScaleAmount(ratio);
            UiManager.Instance.SetBlurAmount(ratio);
            UiManager.Instance.endingCreditCanvasGroup.alpha = ratio;

            time += Time.deltaTime;

            yield return null;
        }

        screenEffect.SetGrayScaleAmount(1f);
        UiManager.Instance.SetBlurAmount(1f);
    }


    IEnumerator PlayerDiedCoroutine()
    {
        StartCoroutine(PlayerDiedScreenEffectCoroutine(playerDeathEffectTime));
        yield return new WaitForSeconds(2f);

        //YouDiedWindow.Instance.diedText.gameObject.SetActive(true);
        //YouDiedWindow.Instance.bgImage.gameObject.SetActive(true);
        YouDiedWindow.Instance.ResetWindow();
        StartCoroutine(YouDiedWindow.Instance.BgEffectCoro());
        yield return StartCoroutine(YouDiedWindow.Instance.DiedEffectCoro());
        
        screenEffect.SetGrayScaleAmount(0f);
        UiManager.Instance.SetBlurAmount(0f);
        
        YouDiedWindow.Instance.diedText.gameObject.SetActive(false);
        YouDiedWindow.Instance.bgImage.gameObject.SetActive(false);

        StartCoroutine(UiManager.Instance.ShowFogCoro(false));
        diedUICam.gameObject.SetActive(false);

        PlayerReborn();
        UnitManager.Instance.ResetAllEnemies();
    }

    public void PlayerReborn()
    {
        Vector3 startPos = lastBonfirePos;

        Vector3 dir = lastBonfirePos - startPos;
        dir = new Vector3(dir.x, 0f, dir.z);

        PlayerLocomove.instance.cc.enabled = false;
        Player.instance.transform.position = startPos;
        Player.instance.gameObject.transform.forward = dir;
        PlayerLocomove.instance.cameraManager.gameObject.transform.position = startPos;
        PlayerLocomove.instance.cameraManager.gameObject.transform.eulerAngles = Player.instance.transform.rotation.eulerAngles;
        PlayerLocomove.instance.cc.enabled = true;

        Player.instance.SetModelCollider(true);
        Player.instance.status.isDead = false;
        Player.instance.status.curHp = Player.instance.status.maxHp;
        Player.instance.status.curStamina = Player.instance.status.maxStamina;

        Player.instance.playerModel.GetComponent<Renderer>().material.SetFloat("_Cutoff",0f);

        StartCoroutine(PlayPlayerStartAnim());
    }

	private void Awake()
    {
        if (fogWallObj == null)
        {
            fogWallObj = GameObject.FindGameObjectWithTag("FogWall");
        }
        fogWallObj.SetActive(false);

        isCreditEnd = false;

        if (diedUICam)
        {
            diedUICam.gameObject.SetActive(false);
        }
    }

	void Start()
    {
        SetPlayer();
        screenEffect = Camera.main.GetComponent<ScreenEffect>();
        lastBonfirePos = playerInitPos;
        SoundManager.Instance.PlayTempSound("RainLoop", new Vector3(6.9f, 20.9f, -170.08f), 1f);
        SoundManager.Instance.PlayBgm("CaveLoop", 0.3f);
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
        PlayerLocomove.instance.cc.enabled = false;        
        Player.instance.transform.position = startPos;
        PlayerLocomove.instance.cameraManager.gameObject.transform.position = startPos;
        PlayerLocomove.instance.cameraManager.gameObject.transform.eulerAngles = startRot;
        PlayerLocomove.instance.cc.enabled = true;

        StartCoroutine(PlayPlayerStartAnim());
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

    public void HitStop(float fps)
    {
        if (isStop) return; 
        StartCoroutine(TimeStop(fps));
    }
    IEnumerator TimeStop(float fps)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(Time.deltaTime * fps);
        Time.timeScale = 1f;
    }

    public void TimeStopEffect(float time = 0.7f, float stopValue = 0.1f)
    {
        StartCoroutine(TimeStopEffectCoro(time, stopValue));
    }
    IEnumerator TimeStopEffectCoro(float time, float stopValue)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        isStop = true;
        Time.timeScale = stopValue;
        yield return new WaitForSecondsRealtime(time);
        while (Time.timeScale < 1f)
        {
            Time.timeScale += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
        if(Time.timeScale >= 1.0f)isStop = false;
    }

    public void TimeStopEffect0()
    {
        StartCoroutine(TimeStopEffectCoro0());
    }

    IEnumerator TimeStopEffectCoro0()
    {
        yield return new WaitForSecondsRealtime(0.07f);
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1f;
    }
}
