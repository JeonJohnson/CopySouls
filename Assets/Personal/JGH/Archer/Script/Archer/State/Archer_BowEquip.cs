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


		//me.animCtrl.SetTrigger("tWalk");
	}

	public override void UpdateState()
	{
		//archer.LookAtSlow(archer.transform, archer.targetObj.transform, 1f, false);

		if (Funcs.IsAnimationAlmostFinish(me.animCtrl, "Archer_Equip"))
		{ 
			
		}



		//if (me.distToTarget > me.status.atkRange)
		//{
		//	archer.curSpd = Random.Range(1f, me.status.moveSpd);
		//	me.navAgent.speed = archer.curSpd;
		//	me.navAgent.SetDestination(me.targetObj.transform.position);
		//}
		//else
		//{
		//	archer.curSpd = Random.Range(1f, me.status.moveSpd);
		//	me.navAgent.speed = archer.curSpd;
		//	me.navAgent.updateRotation = false;
		//	me.navAgent.updatePosition = false;
		//	me.navAgent.Move(-me.dirToTarget * archer.curSpd);
		//}

		
		//me.animCtrl.SetFloat("fWalkSpd", archer.curSpd);
		//me.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1f);

	}

    public override void LateUpdateState()
    {
        base.LateUpdateState();



		//Vector3 dir = me.targetObj.transform.position - archer.headBoneTr.position;
		//me.LookAtSpecificBone(archer.headBoneTr, archer.targetSpineTr, dir);

		//me.LookAtSpecificBone(archer.headBoneTr, archer.targetHeadTr, eGizmoDirection.Foward);

		archer.act.LookTargetRotate();
    }

    public override void ExitState()
	{
	}

}
