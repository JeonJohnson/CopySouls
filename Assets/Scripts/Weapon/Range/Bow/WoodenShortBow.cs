using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBowState
{ 
    Idle,
    Hook, //화살 시위에 걸었을 때
    Pull, //활 시위 당기는 중
    //Full, //시위 다 당겼을 때
    Shoot, //활 시위 놓았을 때
    End
}
	
public class WoodenShortBow : Weapon, IPoolingObject
{
    public Arrow arrow;

    public eBowState state = eBowState.Idle;

    public Animator animCtlr;
    public float pullingAnimSpd;

    
    public Transform ownerRightIndexFingerTr;
    
    public Transform leverTr;
    public Transform stringTr;

    public Vector3 hookDir;
    

    public void ResetForReturn()
    {
        ((IPoolingObject)arrow).ResetForReturn();

    }


    protected override void weaponInitialize()
    {
        animCtlr = GetComponent<Animator>();
    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

		switch (state)
		{
			case eBowState.Idle:
                { 
                
                }
				break;
            case eBowState.Hook:
                {

                }
                break;
            case eBowState.Pull:
                { 
                    
                }
				break;
			case eBowState.Shoot:
                { 
                
                }
				break;
			case eBowState.End:
                { 
                
                }
				break;
			default:
				break;
		}
	}


    protected override void LateUpdate()
    {
        base.LateUpdate();

        switch (state)
        {
            case eBowState.Idle:
                {

                }
                break;
            case eBowState.Hook:
                {
                    stringTr.position = ownerRightIndexFingerTr.position;
                }
                break;
            case eBowState.Pull:
                {
                    stringTr.position = ownerRightIndexFingerTr.position;
                }
                break;
            case eBowState.Shoot:
                {

                }
                break;
            case eBowState.End:
                {

                }
                break;
            default:
                break;
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


}
