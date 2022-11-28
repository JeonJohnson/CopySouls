using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Death : Golem_SubState
{
	public Sub_Death(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 0;
	}

	public override void EnterState()
	{
		base.EnterState();

		golem.status.isDead = true;
		golem.DeathReset();

		golem.meshObj.SetActive(false);
		golem.rootObj.SetActive(false);
		golem.FragScript.gameObject.SetActive(true);
		golem.animCtrl.enabled = false;

		golem.FragScript.animCtrl.SetTrigger("tExplode");

		InGameManager.Instance.BossDeathEvent();
	}
	public override void UpdateState()
	{
		base.UpdateState();
	}

	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
	}

	public override void ExitState()
	{
		base.ExitState();
	}
}

