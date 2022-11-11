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

    public Vector3 dir;

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
                
                }
				break;
			case eArrowState.Hook:
                { 
                
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

		if (rightHandTr)
        { 
            transform.forward = rightHandTr.right;
            transform.localPosition = Vector3.zero;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
