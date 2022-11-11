using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;


public class Archer_Attack_Precision : cState
{
	Archer archer = null;

	eArcherAttackMoveType moveType;
	eArcherAttackState atkState;

	//float moveRandMaxTime;

	float pullTime = 5f;
	float pullAnimSpd;

	float randBackRangeOffset;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);
		
		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		archer.combatState = eCombatState.Combat;

		moveType = archer.actTable.RandAttackMoveType();
		randBackRangeOffset = Random.Range(0.5f, 1.5f);

		pullAnimSpd = archer.actTable.CalcPullStringSpd(pullTime);

		atkState = eArcherAttackState.DrawArrow;
		archer.animCtrl.SetTrigger("tAttack");
	}

	public override void UpdateState()
	{
		archer.actTable.AttackCycle(ref atkState, pullAnimSpd);

		switch (moveType)
		{
			case eArcherAttackMoveType.Siege:
				{
					archer.actTable.MoveWhileAttack(eArcherMoveDir.End);
					//archer.actTable.StartLegLayerWeightCoroutine(-2f);
				}
				break;
			case eArcherAttackMoveType.Kiting:
				{
					if (archer.distToTarget > archer.status.atkRange)
					{
						archer.actTable.MoveWhileAttack(eArcherMoveDir.Forward);
					}
					else if (archer.distToTarget <= archer.status.atkRange + randBackRangeOffset
						&& archer.distToTarget >= archer.backwardRange + randBackRangeOffset)
					{
						archer.actTable.MoveWhileAttack(eArcherMoveDir.End);
						//archer.actTable.StartLegLayerWeightCoroutine(-2f);
					}
					else if (archer.distToTarget < archer.backwardRange)
					{
						archer.actTable.MoveWhileAttack(eArcherMoveDir.Backward);
					}
				}
				break;
			case eArcherAttackMoveType.End:
				break;
			default:
				break;
		}
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();

		archer.actTable.LookTargetRotate(2f);
	}

	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void ExitState()
	{
	}
}
