using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Manager<InGameManager>
{
    public bool isBossCombat = false;
    public delegate void TempFunc();
    public void BossCombatStart(TempFunc func)
    {
        isBossCombat = true;
        func();
    }

	private void Awake()
	{

    }
	// Start is called before the first frame update
	void Start()
    {
        SetPlayer();
    }

    // Update is called once per frame
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
