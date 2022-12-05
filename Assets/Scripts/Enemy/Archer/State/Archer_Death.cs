using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Death : cState
{
	Archer archer = null;

	int rand;

	float ragdollTime;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (!archer)
		{
			archer = script.GetComponent<Archer>();
		}
		

		rand = Random.Range(1, 3);
		ragdollTime = Random.Range(0.5f, 0.75f);

		me.animCtrl.SetTrigger("tDeath");
		me.animCtrl.SetLayerWeight((int)Enums.eHumanoidAvatarMask.Leg, 0);
		archer.actTable.DeleteArrow();

		archer.actTable.DeathSoundTest();

	}
	public override void UpdateState()
	{
		//if (Funcs.IsAnimationAlmostFinish(me.animCtrl, $"Archer_Death_0{rand}",0.25f))
		if (Funcs.IsAnimationAlmostFinish(me.animCtrl, $"Archer_Death_01", ragdollTime))
		{
			archer.status.isDead = true;
			archer.ActiveRagdoll();

			Inventory.Instance.Routing(archer.transform.position);

			//���߿� �굵 �ٽ� ��������
			archer.DeathReset();
			me.gameObject.SetActive(false);
		}
	}

	public override void ExitState()
	{


	}

} 
