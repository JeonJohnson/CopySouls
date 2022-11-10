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
		archer.actTable.AttackCycle(ref atkState, pullAnimSpd);


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
			 // //archer.navAgent.updatePosition = false;
			 // archer.navAgent.updateRotation = false;
			 // //archer.navAgent.Move(archer.transform.forward * -Time.deltaTime * archer.status.moveSpd);
				//Vector3 nextPos = archer.transform.position +
					
				//	(-archer.transform.forward * /*Time.deltaTime **/ archer.status.moveSpd);
				//archer.navAgent.SetDestination(nextPos);

				//if (archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName("Archer_Walk_Aim_Back"))
				//{
				//	archer.animCtrl.SetBool("bMoveDir", false);
				//}
				//else
				//{
				//	archer.animCtrl.SetBool("bMoveDir", true);
				//}
				//archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1f);
				//archer.animCtrl.SetInteger("iMoveDir", 3);
			}
			else
			{ //옆으로 움직이면서 간보기
			 // //archer.navAgent.updatePosition = false;
			 // archer.navAgent.updateRotation = false;
				////archer.navAgent.Move(archer.transform.right * Time.deltaTime* archer.status.moveSpd);
				//Vector3 nextPos = archer.transform.position + 
				//	(archer.transform.right * /*Time.deltaTime **/ archer.status.moveSpd);
				//archer.navAgent.SetDestination(nextPos);


				//if (archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName("Archer_Walk_Aim_Right"))
				//{
				//	archer.animCtrl.SetBool("bMoveDir", false);
				//}
				//else
				//{
				//	archer.animCtrl.SetBool("bMoveDir", true);
				//}

				//archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1f);
				//archer.animCtrl.SetInteger("iMoveDir", 1);
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
