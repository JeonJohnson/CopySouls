using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;


public class Archer_Attack_Precision : cState
{
	Archer archer = null;



	//float moveRandMaxTime;

	float pullTime = 2f;
	float pullAnimSpd;

	float randBackRangeOffset;

	public void AttackStartSetting()
	{
		archer.combatState = eCombatState.Combat;
		archer.moveType = archer.actTable.RandAttackMoveType();
		randBackRangeOffset = Random.Range(-0.5f, 1.5f);

		pullAnimSpd = archer.actTable.CalcOwnerPullStringSpd(pullTime);
		archer.actTable.bowPullingAnimSpd = archer.actTable.CalcBowPullStringSpd(pullTime);
		archer.actTable.curSideWalkDir = (eArcherSideWalkDir)Random.Range((int)eArcherSideWalkDir.Right, (int)eArcherSideWalkDir.End);

		archer.atkState = eArcherAttackState.DrawArrow;
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
		if (archer.actTable.PrecisionAttackCycle(ref archer.atkState, pullAnimSpd))
		{
			if (archer.actTable.RandomAttackState() == eArcherState.Attack_Rushed)
			{
				archer.SetState((int)eArcherState.Attack_Rushed);
			}
			else
			{
				AttackStartSetting();
			}
		}

		switch (archer.moveType)
		{
			case eArcherAttackMoveType.Siege:
				{
					archer.actTable.MoveWhileAttack(eArcherMoveDir.End);
					archer.moveType = eArcherAttackMoveType.End;
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
					}
					else if (archer.distToTarget < archer.backwardRange)
					{
						archer.actTable.MoveWhileAttack(eArcherMoveDir.Backward);
					}

				}
				break;

			case eArcherAttackMoveType.AllDir:
				{
					if (archer.distToTarget > archer.status.atkRange)
					{
						archer.actTable.MoveWhileAttack(eArcherMoveDir.Forward);
					}
					else if (archer.distToTarget <= archer.status.atkRange + randBackRangeOffset
						&& archer.distToTarget >= archer.backwardRange + randBackRangeOffset)
					{
						//플레이어 움직임에 따라서로 바꿔주기?
						//아니면 갈 수 있는곳 아닌곳 판단해서 왓다리 갔다리?
						//archer.actTable.MoveWhileAttack(eArcherMoveDir.Right);
						//archer.actTable.MoveWhileAttack(archer.actTable.IsSideCanMove(archer.actTable.curSideWalkDir));
						archer.actTable.SideWalkThink(archer.actTable.curSideWalkDir);
					}
					else if (archer.distToTarget < archer.backwardRange)
					{
						archer.actTable.MoveWhileAttack(eArcherMoveDir.Backward);
					}
				}
				break;

			case eArcherAttackMoveType.End:
				{

				}
				break;
			 default:
				break;
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
		archer.atkState = eArcherAttackState.End;
		archer.moveType = eArcherAttackMoveType.End;



		archer.actTable.DeleteArrow(); 
	}
}
