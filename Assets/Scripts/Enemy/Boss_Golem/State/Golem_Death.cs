using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Death : cGolemState
{

	public IEnumerator Explode(float time)
	{
		yield return new WaitForSeconds(time);

		Explode();
	}

	public void Explode()
	{
		golem.meshObj.SetActive(false);
		golem.rootObj.SetActive(false);
		golem.FragScript.gameObject.SetActive(true);

		golem.FragScript.animCtrl.SetTrigger("tExplode");
	}



	public Golem_Death(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.None;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		golem.navAgent.isStopped = true;
		golem.status.isDead = true;

		Explode();

		//golem.animCtrl.SetTrigger("tDeath");
		//golem.StartCoroutine(Explode(2f));
	}

	public override void UpdateState()
	{
		//if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, "Agony", 0.8f))
		//{
		//	Explode();
		//}

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
