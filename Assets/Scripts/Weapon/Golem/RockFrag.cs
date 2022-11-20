using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFrag : Weapon
{
	enum eState
	{ 
		PickUp,
		Throw,
		End
	}

	eState curState = eState.PickUp;

	public float spd;
	public Transform golemRightHandTr;

	public Vector3 dir;
	protected override void weaponInitialize()
	{

	}

	public void Throw()
	{
		curState = eState.Throw;
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
					transform.position = golemRightHandTr.position;
				}
				break;
			case eState.Throw:
				{
					transform.position += dir * spd * Time.deltaTime;
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
		base.OnTriggerEnter(other);
	}
}
