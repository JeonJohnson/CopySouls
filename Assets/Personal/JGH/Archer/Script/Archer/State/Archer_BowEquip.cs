using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class Archer_BowEquip : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }



		archer.combatState = eCombatState.Alert;

		archer.animCtrl.SetTrigger("tEquip");
		archer.animCtrl.SetBool("bEquip", true);
		archer.weapon.gameObject.SetActive(true);


		if (me.distToTarget > me.status.atkRange)
		{
			archer.curSpd = Random.Range(1f, me.status.moveSpd);
			me.navAgent.speed = archer.curSpd;
			me.navAgent.SetDestination(me.targetObj.transform.position);
		}
		else
		{
			archer.curSpd = Random.Range(1f, me.status.moveSpd);
			me.navAgent.speed = archer.curSpd;
			me.navAgent.updateRotation = false;
			me.navAgent.updatePosition = false;
			me.navAgent.Move(-me.dirToTarget * archer.curSpd);
		}

		me.animCtrl.SetTrigger("tWalk");
		me.animCtrl.SetFloat("fWalkSpd", archer.curSpd);
		me.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1f);
	}

	public override void UpdateState()
	{

		//me.transform.rotation = me.LookAtSlow(me.transform, me.targetObj.transform, me.status.lookAtSpd);

		//	if (!archer.isEquip)
		//	{
		//		if (Funcs.IsAnimationAlmostFinish(me.animCtrl, "Archer_Equip", 0.7f))
		//		{
		//			if (!archer.isEquip)
		//			{
		//				archer.isEquip = true;
		//			}

		//			//if (me.isAlert)
		//			//{
		//			//	if (me.distToTarget > me.status.atkRange)
		//			//	{
		//			//		me.SetState((int)eArcherState.Walk_Careful);
		//			//	}
		//			//	else
		//			//	{
		//			//		archer.RandomAttack();
		//			//	}
		//			//}
		//			//else
		//			//{
		//			//	me.SetState((int)Enums.eArcherState.Idle);
		//			//}
		//		}
		//	}
		//	else 
		//	{
		//		if (me.distToTarget > me.status.atkRange)
		//		{
		//			me.SetState((int)eArcherState.Walk_Careful);
		//		}
		//		else
		//		{
		//			archer.RandomAttack();
		//		}
		//	}
		//}


	}

    public override void LateUpdateState()
    {
        base.LateUpdateState();

		//Vector3 dir = me.targetObj.transform.position - archer.headBoneTr.position;
		//me.LookAtSpecificBone(archer.headBoneTr, archer.targetSpineTr, dir);

		me.LookAtSpecificBone(archer.headBoneTr, archer.targetHeadTr, eGizmoDirection.Foward);
    }

    public override void ExitState()
	{
	}

}
