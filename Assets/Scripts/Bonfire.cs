using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Item
{
	bool isInteracting = false;
	public override void Initialize()
	{
	}

	public override void PlayFuncs()
	{
		base.PlayFuncs();

	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		SoundManager.Instance.PlayTempSound("BonFire", this.transform.position, 1f);
	}

	protected override void Update()
	{
		base.Update();
	}

}
