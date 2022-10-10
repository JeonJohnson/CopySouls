using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public Arrow arrow;

    public Transform bowLeverTr;//Ȱ �Ŵ� �κ�
    //public GameObject bowString;
    public Transform rightIndexFingerTr;
    public Transform stringTr;

    public bool isHook;
    public bool isPull;

    public Animator animCtrl;
    public float pullAnimSpd;

    public void HookArrow()
    {
        isHook = true;


    
    }

    public void StartStringPull()
    {
        isPull = true;


        animCtrl.SetFloat("fPullSpd", pullAnimSpd);
        animCtrl.SetTrigger("tPull");
    }

    public void ShootArrow()
    {
        isPull = false;

        animCtrl.SetTrigger("tReturn");

        arrow = null;
    }
   
	public void Awake()
	{
        animCtrl = GetComponent<Animator>();


    }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void LateUpdate()
	{

        if (isPull)
        { stringTr.position = rightIndexFingerTr.position; }
    }

    private void OnEnable()
	{
		

	}

	private void OnDisable()
	{
		
	}
}
