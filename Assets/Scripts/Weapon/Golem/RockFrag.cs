using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFrag : Weapon, IPoolingObject
{
	enum eState
	{ 
		PickUp,
		Throw,
		End
	}

	eState curState = eState.PickUp;

	public Rigidbody rd;

	public float spd;
	
	public Transform golemRightHandTr;
	public Transform golemLeftHandTr;

	public Vector3 dir;
	protected override void weaponInitialize()
	{
		rd = GetComponent<Rigidbody>();
	}

	public void ResetForReturn()
	{
		curState = eState.PickUp;
		rd.velocity = Vector3.zero;
		dir = Vector3.zero;
	}

	public void Throw()
	{
		curState = eState.Throw;
		
	}

	public void Explode()
	{ 
	//나중에 이펙트 작업할 때
	
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

		switch (curState)
		{
			case eState.PickUp:
				{
					Vector3 pos = Vector3.Lerp(golemLeftHandTr.position, golemRightHandTr.position, 0.5f);
					transform.position = pos;
				}
				break;
			case eState.Throw:
				{
					rd.MovePosition(transform.position + (dir * spd * Time.deltaTime));
				}
				break;
			case eState.End:
				break;
			default:
				break;
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


	public override void OnTriggerEnter(Collider other)
	{
		if (curState != eState.Throw)
		{
			return;
		}

		base.OnTriggerEnter(other);

		LayerMask checkBitLayer = LayerMask.GetMask("Player_Hitbox")
				| LayerMask.GetMask("PlayerWeapon") | LayerMask.GetMask("Environment");

		if (((1 << other.gameObject.layer) & checkBitLayer) != 0)
		{
			Explode();
			ResetForReturn();
			ObjectPoolingCenter.Instance.ReturnObj(gameObject);
		}

	}


}
