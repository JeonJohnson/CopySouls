using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public enum eArcherAttackMoveType
{
	Siege, //가만히 서서 쏘는거
	Kiting, //앞,뒤로 거리 조절만 하면서 쏘는거
	Side,
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


	public float bowPullingAnimSpd;

	#region Animation Events
	public void BowEquipAnimEvent()
	{
		archer.weapon.gameObject.SetActive(true);
		archer.weaponEquipState = eEquipState.Equip;
	}

	public void DrawArrowAnimEvent()
	{ //화살통에서 화살 뽑았을 때 
	  //화살 생성해야함
		archer.arrow = ObjectPoolingCenter.Instance.LentalObj(archer.arrowName).GetComponent<CommonArrow>();
		archer.arrow.owner = archer.gameObject;
		
		archer.arrow.state = eArrowState.Draw;

		archer.arrow.transform.SetParent(archer.rightIndexFingerBoneTr);
		archer.arrow.transform.localPosition = Vector3.zero;

		archer.arrow.rightHandTr = archer.rightHandTr;
		archer.arrow.bowLeverTr = archer.weapon.leverTr;

		archer.arrow.targetTr = archer.targetSpineTr;
		
		archer.arrow.WeaponColliderOnOff(false);

		//archer.arrow.transform.forward = archer.animCtrl.GetBoneTransform(HumanBodyBones.RightHand).right;
		//archer.arrow.transform.localRotation = Quaternion.identity;
	}

	public void HookArrowAnimEvent()
	{ //화살에 걸었을때,
	  //이때부터 화살 걸이부분으로 forward맞춰주기
		archer.weapon.state = eBowState.Hook;
		archer.arrow.state = eArrowState.Hook;


		//x: 0.21823844, y: 0.29749483, z: -0.0024048486
	}

	public void StartPullStringAnimEvent()
	{
		archer.weapon.state = eBowState.Pull;

		archer.weapon.animCtlr.SetTrigger("tPull");
		archer.weapon.animCtlr.SetFloat("fPullSpd", bowPullingAnimSpd);
	}

	public void ShootArrowAnimEvent()
	{
		archer.weapon.state = eBowState.Shoot;
		archer.weapon.animCtlr.SetTrigger("tReturn");
		
		archer.arrow.state = eArrowState.Shoot;
		archer.arrow.WeaponColliderOnOff(true);
		archer.arrow.LookTarget();
		archer.arrow.transform.SetParent(null);
		archer.arrow.StartCoroutine(archer.arrow.AliveCoroutine());

		archer.arrow = null;
	}
	#endregion



	public eArcherAttackMoveType RandAttackMoveType()
	{
		return (eArcherAttackMoveType)Random.Range((int)eArcherAttackMoveType.Siege, (int)eArcherAttackMoveType.Side);
		//return eArcherAttackMoveType.Siege;
		//return eArcherAttackMoveType.Kiting;
	}

	public float CalcOwnerPullStringSpd(float pullingTime)
	{
		//pulling String Animation Max Frame = 0 ~ 111 => Total 111 Frame / About 3.7 sec

		if (pullingTime <= 0f)
		{
			Debug.LogError("활 쏘는 애니메이션 시간이 왜 0 이하냐 수정해!!");
		}

		return 3.7f/pullingTime;
	}

	public float CalcBowPullStringSpd(float pullingTime)
	{
		return 0.25f / pullingTime;
	}

	public bool AttackCycle(ref  eArcherAttackState  atkState, float pullAnimSpd )
	{
		bool returnBoolVal = false;
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
					if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "Archer_Atk_Shoot"))
					{
						archer.atkRefCoolTime = Random.Range(archer.atkMinCoolTime, archer.atkMaxCoolTime);
						archer.moveType = eArcherAttackMoveType.Side;
						atkState = eArcherAttackState.End;
					}
				}
				break;
			case eArcherAttackState.End:
				{
					archer.atkCurCoolTime += Time.deltaTime;

					if (archer.atkCurCoolTime >= archer.atkRefCoolTime)
					{
						archer.atkCurCoolTime = 0f;
						   returnBoolVal = true;
					}
				}
				break;
			default:
				break;
		}

		archer.animCtrl.SetInteger("iAttackState", (int)atkState);

		return returnBoolVal;
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
					archer.animCtrl.SetInteger("iMoveDir", (int)direction);
				}
				break;
			case eArcherMoveDir.Right:
			case eArcherMoveDir.Left:
				{
					Vector3 nextPos = archer.transform.position + (archer.transform.right * (archer.status.moveSpd * 0.75f)/** Time.deltaTime*/);
					if (direction == eArcherMoveDir.Left)
					{ nextPos *= -1f; }

					archer.isMove = true;

					archer.navAgent.updateRotation = false;
					archer.navAgent.SetDestination(nextPos);

					//if (archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName("Archer_Walk_Aim_Right"))
					//{
						archer.animCtrl.SetBool("bMoveDir", !archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName("Archer_Walk_Aim_Right"));
					//}
					//else
					//{
					//	archer.animCtrl.SetBool("bMoveDir", true);
					//}
					archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1f);
					archer.animCtrl.SetInteger("iMoveDir", (int)direction);
				}
				break;
			case eArcherMoveDir.Backward:
				{
					Vector3 nextPos = archer.transform.position +(-archer.dirToTarget * (archer.status.moveSpd *0.75f)/** Time.deltaTime*/);

					archer.isMove = true;

					archer.navAgent.updateRotation = false;
					archer.navAgent.SetDestination(nextPos);

					if (archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName("Archer_Walk_Aim_Back"))
					{
						archer.animCtrl.SetBool("bMoveDir", false);
					}
					else
					{
						archer.animCtrl.SetBool("bMoveDir", true);
					}
					archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1f);
					archer.animCtrl.SetInteger("iMoveDir", (int)direction);
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

	//public bool IsCanMove(Vector3 destPos)
	//{

		
	//	//Vector3 dir = (destPos - archer.transform.position);

	//	////Ray ray = new Ray();
	//	////ray.origin = archer.transform.position;
	//	////ray.direction = dir;
	//	//RaycastHit hitInfo;

	//	//if (Physics.Raycast(archer.transform.position, dir, out hitInfo))
	//	//{
	//	//	if (hitInfo.collider.gameObject.layer == LayerMask.GetMask("Environment"))
	//	//	{
	//	//		return false;
	//	//	}
	//	//}

	//	//return true;

		
	//}



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

		//221111 1428 걍 다 기각하고 움직임 없을 때 머리랑 몸 다 도는걸루~

		float archerAgentCurSpd = Vector3.Magnitude(archer.navAgent.velocity);

		float angleToTarget = Mathf.Acos(Vector3.Dot(transform.forward, archer.dirToTarget)) * Mathf.Rad2Deg;

		
		archer.LookAtSpecificBone(archer.spineBoneTr, archer.targetSpineTr, Enums.eGizmoDirection.Back, new Vector3(0f, 0f, -90f));
		
		archer.LookAtSpecificBone(archer.headBoneTr, archer.targetHeadTr, eGizmoDirection.Foward);

		archer.LookAtSlow(archer.transform, archer.targetObj.transform, bodyRotSpd);
		LegRotateInPlaceLayerWieght(angleToTarget);

		#region theOldThings
		//if (!archer.isMove)
		//{

		//}


		//if (archer.isMove)
		//{ //움직임이 있는 경우 
		//	////1. 얼굴만 돌리기
		//	////2. 얼굴이랑 몸이랑 각도가 너무 차이 날 경우
		//	////ㄴㄴ 그냥 발만 조절해주기
		//}
		//else 
		//{ //움직임이 없는 경우
		//	if (angleToTarget < 90f)
		//	{
		//		//차이가 90도 안일 경우
		//		//얼굴 그쪽으로 바라보면서 몸 돌리기 + 제자리 발 애니메이션
		//		//HeadRotate();
		//		LegRotateInPlaceLayerWieght(angleToTarget);
		//	}
		//	else
		//	{
		//		//차이가 90도 보다 클 경우

		//		//animator apply root motion 키고 애니메이션 재생
		//		//eSideDirection dirNum = archer.TargetOnWhichSide(transform.forward,archer.dirToTarget,transform.up);
		//		//if (dirNum == eSideDirection.Left)
		//		//{
		//		//}
		//		//else if (dirNum == eSideDirection.Right)
		//		//{ 
		//		//}
		//		archer.LookAtSpecificBone(archer.headBoneTr, archer.targetHeadTr, eGizmoDirection.Foward);
		//		archer.LookAtSlow(archer.transform, archer.targetObj.transform, bodyRotSpd*2);
		//		LegRotateInPlaceLayerWieght(angleToTarget);
		//	}
		//}
		#endregion
	}

	private void LegRotateInPlaceLayerWieght(float angle)
	{
		if (archer.isMove)
		{
			archer.animCtrl.SetBool("bRotate", false);
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

	//private void HeadRotate(float campAngle = 90f)
	//{ //몸의 forward랑 head의 Forward랑 내적해서 90도 이상이면 더 못 돌리도록.
	//  //그 이상이 되면 몸이 돌아갸아함.
	//	int a = 2;
	//	a = 15;
	//}


//    public void Walk(eCombatState combatState)
//    {
//		switch (combatState)
//		{
//			case eCombatState.Idle:
//				{ 
					


				
//				}
//				break;
//			case eCombatState.Alert:
//				{
//					if (archer.distToTarget > archer.status.atkRange)
//					{
//						archer.curSpd = Random.Range(1f, archer.status.moveSpd);
//						archer.navAgent.speed = archer.curSpd;
//						archer.navAgent.SetDestination(archer.targetObj.transform.position);
//					}
//					else
//					{
//						archer.curSpd = Random.Range(1f, archer.status.moveSpd);
//						archer.navAgent.speed = archer.curSpd;
//						archer.navAgent.updateRotation = false;
//						archer.navAgent.updatePosition = false;
//						archer.navAgent.Move(-archer.dirToTarget * archer.curSpd);
//					}
//				}
//				break;
//			case eCombatState.Combat:
//				{ 
				


				
//				}
//				break;
//			default:
//				break;
//		}
//	}
}
