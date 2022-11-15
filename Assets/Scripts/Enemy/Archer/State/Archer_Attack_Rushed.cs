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

		//curShootCount = 0;

	}


	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		curShootCount = 0;
		AttackStartSetting();
	}

	public override void UpdateState()
	{
	

		if (curShootCount < 3)
		{
			if (archer.actTable.RushedAttackCycle(ref atkState, pullAnimSpd, curShootCount))
			{
				++curShootCount;
				AttackStartSetting();
			}
		}
		else
		{
			if (archer.atkState == eArcherAttackState.End)
			{
				if (archer.CheckTargetIsHiding())
				{
					Debug.Log("시야에서 사라짐");
					archer.SetState((int)Enums.eArcherState.Chase);
					return;
				}
			}
			archer.SetState((int)Enums.eArcherState.Attack_Precision);
		}

	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
		
		archer.actTable.LookTargetRotate(6f);
	}

	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}
	public override void ExitState()
	{


		archer.weapon.state = eBowState.Shoot;

		archer.actTable.DeleteArrow();
	}
}
