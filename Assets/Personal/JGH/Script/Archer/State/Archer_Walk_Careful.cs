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
		me.transform.rotation = me.LookAtSlow(me.transform, me.targetObj.transform, me.status.lookAtSpd);

		if (me.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Archer_Walk_Arm"))
		{
			me.navAgent.Move(me.dirToTarget * Time.deltaTime * me.status.moveSpd);
		}

		if (me.distToTarget <= me.status.atkRange + offsetRange)
		{
			int atkRandom = Random.Range(0, 1);
			if (atkRandom == 0)
			{
				me.SetState((int)Enums.eArcherState.Attack_Aiming);
			}
			else 
			{
				me.SetState((int)Enums.eArcherState.Attack_Rapid);
			}
		}
		
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
