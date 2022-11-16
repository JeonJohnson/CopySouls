using Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
	public override void DeathReset()
	{
		base.DeathReset();
	}

	public override void Hit(DamagedStruct dmgStruct)
	{
		base.Hit(dmgStruct);
	}

	public override void InitializeState()
	{
		throw new System.NotImplementedException();
	}

	public override void ResetEnemy()
	{
		base.ResetEnemy();
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
	protected override void LateUpdate()
	{
		base.LateUpdate();
	}
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
	}

	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
	}
}
