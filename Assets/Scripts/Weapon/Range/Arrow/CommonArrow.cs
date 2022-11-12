using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eArrowState
{
    Draw,
    Hook,
    Shoot,
    End
}

public class CommonArrow : Weapon, IPoolingObject
{
    public Rigidbody rd;

    public eArrowState state = eArrowState.Draw;

    public float spd;
    public float maxRange;
    [HideInInspector] public float mileage = 0f;

    public Transform rightHandTr;
    public Transform bowLeverTr;

    public Transform targetTr;
    //public Vector3 hookDir;
    public Vector3 shootDir;

    public void ResetForReturn()
    {

    }

    protected override void weaponInitialize()
    {
        rd = GetComponent<Rigidbody>();
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
			case eArrowState.Draw:
                {
                    transform.forward = rightHandTr.right;
                    transform.localPosition = Vector3.zero;
                }
				break;
			case eArrowState.Hook:
                {
                    transform.forward = bowLeverTr.forward;
                }
				break;
			case eArrowState.Shoot:
                { 
                    
                }
				break;
			case eArrowState.End:
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
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (state == eArrowState.Shoot)
        { 
            
        }
    }

}
