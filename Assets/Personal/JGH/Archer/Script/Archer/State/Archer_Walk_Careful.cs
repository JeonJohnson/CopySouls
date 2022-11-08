using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Walk_Careful : cState
{
	Archer archer = null;

	float offsetRange;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }


		me.animCtrl.SetTrigger("tWalk");

		offsetRange = Random.Range(-0.5f, 1f);

	}
	public override void UpdateState()
	{

		//if (!archer.CheckTargetIsHidingInFov(me.targetObj))
		//{
		//	me.SetState((int)Enums.eArcherState.LookAround);
		//}

		//me.transform.rotation = me.LookAtSlow(me.transform, me.targetObj.transform, me.status.lookAtSpd);

		//if (me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Archer_Walk_Arm"))
		//{
		//	me.navAgent.Move(me.dirToTarget * Time.deltaTime * me.status.moveSpd);
		//}

		//if (me.distToTarget <= me.status.atkRange + offsetRange)
		//{
		//	archer.RandomAttack();
		//}
		
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();

		me.LookAtSpecificBone(archer.headBoneTr, me.targetObj.transform, Enums.eGizmoDirection.Foward);
	}

	public override void ExitState()
	{
	}


}
