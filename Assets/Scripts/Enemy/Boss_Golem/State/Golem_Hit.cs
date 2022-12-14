using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Hit : cGolemState
{
	public Golem_Hit(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.None;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);


		golem.animCtrl.SetTrigger("tHit");
		golem.animCtrl.SetFloat("fHpRatio", golem.status.curHp / golem.status.maxHp);
	}

	public override void UpdateState()
	{
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
	}
	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void ExitState()
	{
	}
}
