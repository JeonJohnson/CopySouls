using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Attack_Rushed : cState
{

	Archer archer = null;

	eArcherAttackState atkState;

	float pullAnimSpd;
	int curShootCount =0;

	public void AttackStartSetting()
	{
		archer.combatState = eCombatState.Combat;

		pullAnimSpd = archer.actTable.CalcOwnerPullStringSpd(0.25f);

		atkState = eArcherAttackState.DrawArrow;
		archer.animCtrl.SetTrigger("tAttack");
	}


	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		AttackStartSetting();
	}

	public override void UpdateState()
	{
		if (curShootCount < 3)
		{
			if (archer.actTable.AttackCycle(ref atkState, pullAnimSpd))
			{
				++curShootCount;
				AttackStartSetting();
			}
		}
		else
		{ 
			

		}
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
		
		archer.actTable.LookTargetRotate(4f);
	}

	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}
	public override void ExitState()
	{
		
	}
}
