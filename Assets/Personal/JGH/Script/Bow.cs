using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public Arrow arrow;

    public Transform bowLeverTr;
    //public GameObject bowString;
    public Transform rightIndexFingerTr;
    public Transform stringTr;

    //public Vector3 stringOriginPos;

    public Animator animCtrl;

	//public void DrawArrow()
 //   {

 //   }

    public void PullString(float fullTime)
    {

    }

    public void ShootingArrow()
    {
    
    
    }

    public void ReturnString()
    {

    }

    public IEnumerator StringReturnCoroutine()
    {
        


        yield return null;
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
		
	}

	private void OnEnable()
	{
		

	}

	private void OnDisable()
	{
		
	}
}
