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

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		rand = Random.Range(1, 3);
		ragdollTime = Random.Range(0.25f, 0.75f);

		me.ResetAllAnimTrigger(Defines.ArcherAnimTriggerStr);

		me.animCtrl.SetTrigger("tDeath");
		//me.animCtrl.SetInteger("iDeath", rand);
		me.animCtrl.SetLayerWeight((int)Enums.eHumanoidAvatarMask.Leg, 0);


		archer.actTable.DeleteArrow();
	}
	public override void UpdateState()
	{
		//if (Funcs.IsAnimationAlmostFinish(me.animCtrl, $"Archer_Death_0{rand}",0.25f))
		if (Funcs.IsAnimationAlmostFinish(me.animCtrl, $"Archer_Death_01", ragdollTime))
		{
			//ObjectPooling Center에 되돌릴 준비
			//GameObject ragdoll = ObjectPoolingCenter.Instance.LentalObj("Archer_Ragdoll");
			//Funcs.RagdollObjTransformSetting(me.transform, ragdoll.transform);
			//ragdoll.transform.position = me.transform.position;

			//나중에 얘도 다시 돌려놓기
			me.gameObject.SetActive(false);
		}
	}

	public override void ExitState()
	{


	}

} 
