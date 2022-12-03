using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemFist : Weapon
{

	protected override void weaponInitialize()
	{
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

		if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
		{
			CameraEffect.instance.PlayShake("Golem_Smash");
		}
	}

}
