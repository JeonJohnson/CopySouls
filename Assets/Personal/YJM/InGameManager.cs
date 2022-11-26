using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : Manager<InGameManager>
{
    public bool isBossCombat = false;
    private GameObject fogWallObj = null;


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
        yield return new WaitForSeconds(4f);
        LoadingSceneController.Instance.LoadScene((int)eSceneChangeTestIndex.Title);
    }

    //IEnumerator TestFadeOutCoroutine()
    //{
    //    float curTimer = 0;
    //    while (curTimer <= 1f)
    //    {
    //        yield return null;
    //        curTimer += Time.unscaledDeltaTime * fadeSpd;

    //        float alphaVal = Mathf.Lerp(0f, 1f, curTimer);
    //    }
    //}
    //IEnumerator GotoCreditSceneCoroutine()
    //{
    //    yield return StartCoroutine(TestFadeOutCoroutine());

    //    SceneManager.LoadScene((int)eSceneChangeTestIndex.Credit);
    //}

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
    }

    void Update()
    {
        
    }

    void SetPlayer()
    {
        Vector3 startPos = new Vector3(4.55f, 5.1f, -130f);
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
