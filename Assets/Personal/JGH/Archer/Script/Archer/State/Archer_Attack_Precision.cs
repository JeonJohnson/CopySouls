using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;


public class Archer_Attack_Precision : cState
{
	Archer archer = null;

	eArcherAttackMoveType moveType;
	eArcherAttackState atkState;

	float pullTime = 5f;
	float pullAnimSpd;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);
		
		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		archer.combatState = eCombatState.Combat;


		moveType = archer.actTable.RandAttackMoveType();

		pullAnimSpd = archer.actTable.CalcPullStringSpd(pullTime);
		

		atkState = eArcherAttackState.DrawArrow;
		archer.animCtrl.SetTrigger("tAttack");
	}

	public override void UpdateState()
	{

		switch (atkState)
		{
			case eArcherAttackState.DrawArrow:
				{
					if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "Archer_Atk_DrawArrow"))
					{
						atkState = eArcherAttackState.HangArrow;
						
					}
				}
				break;
			case eArcherAttackState.HangArrow:
				{
					if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "Archer_Atk_HangArrow"))
					{
						atkState = eArcherAttackState.PullString;
					}
				}
				break;
			case eArcherAttackState.PullString:
				{
					archer.animCtrl.speed = pullAnimSpd;

					if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "Archer_Atk_PullString"))
					{
						archer.animCtrl.speed = 1;
						atkState = eArcherAttackState.Shoot;
					}
				}
				break;
			case eArcherAttackState.Shoot:
				{ 
					
				}
				break;
			case eArcherAttackState.End:
				break;
			default:
				break;
		}
		archer.animCtrl.SetInteger("iAttackState", (int)atkState);

		if (moveType == eArcherAttackMoveType.Kiting)
		{
			if (archer.distToTarget > archer.status.atkRange)
			{ //앞으로 걸어가야 함
				archer.navAgent.updatePosition = true;
				archer.navAgent.updateRotation = true;

				archer.navAgent.SetDestination(archer.targetObj.transform.position);

				if (archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName("Archer_Walk_Aim_Forward"))
				{
					archer.animCtrl.SetBool("bMoveDir", false);
				}
				else
				{
					archer.animCtrl.SetBool("bMoveDir", true);
				}


				archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1f);
				archer.animCtrl.SetInteger("iMoveDir", 0);
			}
			else if (archer.distToTarget <= archer.backwardRange)
			{ //뒤로 걸어갈 것
				archer.navAgent.updatePosition = false;
				archer.navAgent.updateRotation = false;
				archer.navAgent.Move(archer.transform.forward * -Time.deltaTime * archer.status.moveSpd);

				if (archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName("Archer_Walk_Aim_Back"))
				{
					archer.animCtrl.SetBool("bMoveDir", false);
				}
				else
				{
					archer.animCtrl.SetBool("bMoveDir", true);
				}
				archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1f);
				archer.animCtrl.SetInteger("iMoveDir", 3);
			}
			else
			{ //옆으로 움직이면서 간보기
				archer.navAgent.updatePosition = false;
				archer.navAgent.updateRotation = false;
				archer.navAgent.Move(archer.transform.right * Time.deltaTime* archer.status.moveSpd);

				if (archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName("Archer_Walk_Aim_Right"))
				{
					archer.animCtrl.SetBool("bMoveDir", false);
				}
				else
				{
					archer.animCtrl.SetBool("bMoveDir", true);
				}

				archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1f);
				archer.animCtrl.SetInteger("iMoveDir", 1);
			}
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
