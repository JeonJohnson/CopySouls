using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public enum eArcherAttackMoveType
{
	Siege, //가만히 서서 쏘는거
	Kiting, //앞,뒤로 거리 조절만 하면서 쏘는거
	End
}

public enum eArcherAttackState
{ 
	DrawArrow,
	HangArrow,
	PullString,
	Shoot,
	End
}

public enum eArcherMoveDir
{ 
	Forward,
	Right,
	Left,
	Backward,
	End
}


public class Archer_ActionTable : MonoBehaviour
{
	Archer archer;
    public Archer SetArcher
    {
        set 
        {
            archer = value;
        }
    }

	


	#region Animation Events
	public void BowEquipAnimEvent()
	{
		archer.weapon.gameObject.SetActive(true);
		archer.weaponEquipState = eEquipState.Equip;
	}

	public void PullOutArrowAnimEvent()
	{ //화살통에서 화살 뽑았을 때 
		//화살 생성해야함
	
	}

	public void DrawArrowEndAnimEvent()
	{ //

	}
	public void StartPullStringAnimEvent()
	{ 
		
	}

	public void ShootArrowAnimEvent()
	{ 
	
	}
	#endregion

	public eArcherAttackMoveType RandAttackMoveType()
	{
		//return (eArcherAttackMoveType)Random.Range((int)eArcherAttackMoveType.Siege, (int)eArcherAttackMoveType.End);
		//return eArcherAttackMoveType.Siege;
		return eArcherAttackMoveType.Kiting;
	}

	public float CalcPullStringSpd(float pullingTime)
	{
		//pulling String Animation Max Frame = 0 ~ 111 => Total 111 Frame / About 3.7 sec

		return 3.7f/pullingTime;
	}

	public void AttackCycle(ref  eArcherAttackState  atkState, float pullAnimSpd )
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

	}

	public void MoveWhileAttack(eArcherMoveDir direction)
	{
		switch (direction)
		{
			case eArcherMoveDir.Forward:
				{
					archer.isMove = true;

					archer.navAgent.enabled = true;
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
				break;
			case eArcherMoveDir.Right:
				break;
			case eArcherMoveDir.Left:
				break;
			case eArcherMoveDir.Backward:
				{
					Vector3 nextPos = archer.transform.position +(-archer.dirToTarget * archer.status.moveSpd /** Time.deltaTime*/);

					

					if (IsCanMove(nextPos))
					{
						archer.isMove = true;

						//archer.navAgent.enabled = false;
						//archer.curSpd = archer.status.moveSpd;
						archer.navAgent.updateRotation = false;
						archer.navAgent.SetDestination(nextPos);
						//archer.rd.MovePosition(nextPos);

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
				}
				break;
			case eArcherMoveDir.End:
				{
					//archer.navAgent.enabled = true;
					archer.isMove = false;
					archer.animCtrl.SetBool("bMoveDir", false);
					archer.navAgent.SetDestination(archer.transform.position);
				}
				break;
			default:
				break;
		}

		



	}

	public bool IsCanMove(Vector3 destPos)
	{
		Vector3 dir = (destPos - archer.transform.position);

		//Ray ray = new Ray();
		//ray.origin = archer.transform.position;
		//ray.direction = dir;
		RaycastHit hitInfo;

		if (Physics.Raycast(archer.transform.position, dir, out hitInfo))
		{
			if (hitInfo.collider.gameObject.layer == LayerMask.GetMask("Environment"))
			{
				return false;
			}
		}

		return true;
	}




	public void StartLegLayerWeightCoroutine(float spd)
	{
		StartCoroutine(LegLayerWeightCoroutine(spd));
	}

	public IEnumerator LegLayerWeightCoroutine(float spd)
	{
		float amount = archer.animCtrl.GetLayerWeight((int)eHumanoidAvatarMask.Leg);

		while (true)
		{
			amount += spd * Time.deltaTime;

			archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, amount);

			if (spd > 0f)
			{
				if (amount >= 1f)
				{
					break;
				}
			}
			else if ( spd < 0f)
			{
				if (amount <= 0f)
				{
					break;
				}
			}
			else
			{
				break;
			}

			yield return null;
		}
	}

	public void LookTargetRotate(float bodyRotSpd = 1f)
	{//Use at LateUpdate!!!

		//움직임이 없을 경우
		//=> 얼굴이랑 같이 천천히 LookAt으로 돌아감 (딱히 발 모션없음)
		//=> 대신 90라던가 각도가 엄청 차이나면 회전 애니메이션 재생.

		//움직임이 있는 경우(걷기, 뛰기 등)
		//=> 발 애니메이션이 있기에 그냥 NavMeshAgent의 rotate로 해도 딱히 안 이상함.
		//=> 대신 이것도 각도 차이가 많이 나면 회전 애니메이션 재생함.

		//archer.navAgent.updateRotation = true;

		float archerAgentCurSpd = Vector3.Magnitude(archer.navAgent.velocity);

		float angleToTarget = Mathf.Acos(Vector3.Dot(transform.forward, archer.dirToTarget)) * Mathf.Rad2Deg;
		//Debug.Log(angleToTarget);
		//if (archer.navAgent.velocity != Vector3.zero)
		if (archer.isMove)
		{ //움직임이 있는 경우 
			
			//1. 얼굴만 돌리기
			archer.LookAtSpecificBone(archer.headBoneTr, archer.targetHeadTr, eGizmoDirection.Foward);
			archer.LookAtSlow(archer.transform, archer.targetObj.transform, bodyRotSpd);
			//2. 얼굴이랑 몸이랑 각도가 너무 차이 날 경우

		}
		else 
		{ //움직임이 없는 경우
			if (angleToTarget < 90f)
			{
				//차이가 90도 안일 경우

				//얼굴 그쪽으로 바라보면서 몸 돌리기 + 제자리 발 애니메이션
				//HeadRotate();
				archer.LookAtSpecificBone(archer.headBoneTr, archer.targetHeadTr, eGizmoDirection.Foward);
				archer.LookAtSlow(archer.transform, archer.targetObj.transform, bodyRotSpd);


				LegRotateInPlaceLayerWieght(angleToTarget);
			}
			else
			{
				//차이가 90도 보다 클 경우

				//animator apply root motion 키고 애니메이션 재생
				//eSideDirection dirNum = archer.TargetOnWhichSide(transform.forward,archer.dirToTarget,transform.up);
				//if (dirNum == eSideDirection.Left)
				//{

				//}
				//else if (dirNum == eSideDirection.Right)
				//{ 

				//}
				archer.LookAtSpecificBone(archer.headBoneTr, archer.targetHeadTr, eGizmoDirection.Foward);
				archer.LookAtSlow(archer.transform, archer.targetObj.transform, bodyRotSpd*2);


				LegRotateInPlaceLayerWieght(angleToTarget);
			}
		}
	}

	private void LegRotateInPlaceLayerWieght(float angle)
	{
		if (archer.isMove)
		{
			return;
		}

		eSideDirection dirNum = archer.TargetOnWhichSide(transform.forward, archer.dirToTarget, transform.up);
		string animName = "Leg_Rotate_InPlace_";

		if (dirNum == eSideDirection.Left)
		{
			animName += "Left";
		}
		else if (dirNum == eSideDirection.Right)
		{
			animName += "Right";
		}

		if (archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName(animName))
		{
			archer.animCtrl.SetBool("bRotate", false);
		}
		else
		{
			archer.animCtrl.SetBool("bRotate", true);
		}

		archer.animCtrl.SetInteger("iRotateDir", (int)dirNum);
		archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, angle / (archer.status.fovAngle *0.5f));
	}

	private void HeadRotate(float campAngle = 90f)
	{ //몸의 forward랑 head의 Forward랑 내적해서 90도 이상이면 더 못 돌리도록.
	  //그 이상이 되면 몸이 돌아갸아함.

	}


    public void Walk(eCombatState combatState)
    {
		switch (combatState)
		{
			case eCombatState.Idle:
				{ 
					


				
				}
				break;
			case eCombatState.Alert:
				{
					if (archer.distToTarget > archer.status.atkRange)
					{
						archer.curSpd = Random.Range(1f, archer.status.moveSpd);
						archer.navAgent.speed = archer.curSpd;
						archer.navAgent.SetDestination(archer.targetObj.transform.position);
					}
					else
					{
						archer.curSpd = Random.Range(1f, archer.status.moveSpd);
						archer.navAgent.speed = archer.curSpd;
						archer.navAgent.updateRotation = false;
						archer.navAgent.updatePosition = false;
						archer.navAgent.Move(-archer.dirToTarget * archer.curSpd);
					}
				}
				break;
			case eCombatState.Combat:
				{ 
				


				
				}
				break;
			default:
				break;
		}

	}
}
