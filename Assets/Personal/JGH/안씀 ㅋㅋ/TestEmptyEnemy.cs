using Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmptyEnemy : Enemy
{
	public override void Hit(DamagedStruct dmgStruct)
	{
		base.Hit(dmgStruct);
	}

	public override void InitializeState()
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



	// Start is called before the first frame update
}
