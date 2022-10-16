using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Structs;
public class Bow : MonoBehaviour
{
    public WeaponStatus status;

    public Arrow arrow;

    public Transform bowLeverTr;//활 거는 부분
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

        if (arrow != null)
        {
            arrow.maxRange = status.range;
            arrow = null;
        }
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
