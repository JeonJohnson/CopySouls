using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public enum eArcherAttackMoveType
{
	Siege, //가만히 서서 쏘는거
	Kiting, //앞,뒤로 거리 조절만 하면서 쏘는거
	AllDir, //아예 모든 방향으로 움직이는거 
	End
}

public enum eArcherSideWalkDir
{ //활 쏘고나서 대기시간에 AllDir에서 좌,우 움직임 나누기 위해서
	Right,
	Left,
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

	public eArcherSideWalkDir curSideWalkDir;

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
		return (eArcherAttackMoveType)Random.Range((int)eArcherAttackMoveType.Siege, (int)eArcherAttackMoveType.End);
		//return eArcherAttackMoveType.Siege;
		//return eArcherAttackMoveType.Kiting;
	}

	public eArcherState RandomAttackState()
	{
		if (Random.Range(0f, 100f) <= 70f)
		{
			return eArcherState.Attack_Precision;
		}
		else
		{
			return eArcherState.Attack_Rushed;
		}
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

	public bool PrecisionAttackCycle(ref  eArcherAttackState  atkState, float pullAnimSpd )
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
					archer.animCtrl.SetFloat("fBowPullSpd", pullAnimSpd);

					if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "Archer_Atk_PullString"))
					{

						archer.animCtrl.SetFloat("fBowPullSpd", 1f);
						atkState = eArcherAttackState.Shoot;
					}
				}
				break;
			case eArcherAttackState.Shoot:
				{
					if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "Archer_Atk_Shoot"))
					{
						archer.atkRefCoolTime = Random.Range(archer.atkMinCoolTime, archer.atkMaxCoolTime);

						archer.moveType = RandAttackMoveType();
						curSideWalkDir = (eArcherSideWalkDir)Random.Range((int)eArcherSideWalkDir.Right, (int)eArcherSideWalkDir.End);

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


	public bool RushedAttackCycle(ref eArcherAttackState atkState, float pullAnimSpd, int ShootCount)
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
					archer.animCtrl.SetFloat("fBowPullSpd", pullAnimSpd);

					if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "Archer_Atk_PullString"))
					{

						archer.animCtrl.SetFloat("fBowPullSpd", 1f);
						atkState = eArcherAttackState.Shoot;
					}
				}
				break;
			case eArcherAttackState.Shoot:
				{
					if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "Archer_Atk_Shoot"))
					{
						atkState = eArcherAttackState.End;
					}
				}
				break;
			case eArcherAttackState.End:
				{

					returnBoolVal = true;
					//archer.atkCurCoolTime += Time.deltaTime;

					//if (archer.atkCurCoolTime >= archer.atkRefCoolTime)
					//{
					//	archer.atkCurCoolTime = 0f;
					//}
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

					float animSpd = Vector3.Magnitude(archer.navAgent.velocity) / archer.status.moveSpd;
					if (animSpd >= 0.1f)
					{
						archer.animCtrl.SetFloat("fWalkSpd", animSpd);
						archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1);
						archer.animCtrl.SetInteger("iMoveDir", (int)direction);
					}
					else
					{
						archer.isMove = false;
						archer.animCtrl.SetBool("bMoveDir", false);
					}

					//LegIKToGround(AvatarIKGoal.RightFoot);
					//LegIKToGround(AvatarIKGoal.LeftFoot);
				}
				break;
			case eArcherMoveDir.Right:
			case eArcherMoveDir.Left:
				{
					Vector3 nextPos = archer.transform.position + (archer.transform.right * (archer.status.moveSpd * 0.8f)/** Time.deltaTime*/);

					if (direction == eArcherMoveDir.Left)
					{ nextPos *= -1f; }

					IsCanMove(nextPos);

					archer.isMove = true;

					archer.navAgent.updateRotation = false;
					archer.navAgent.SetDestination(nextPos);

					string animName = "Archer_Walk_Aim_";
					animName += direction == eArcherMoveDir.Right ? "Right" : "Left";

					//if (archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName(animName))
					//{
						//archer.animCtrl.SetBool("bMoveDir", false);
						archer.animCtrl.SetBool("bMoveDir", !archer.animCtrl.GetCurrentAnimatorStateInfo((int)Enums.eHumanoidAvatarMask.Leg).IsName(animName));
					//}
					//else
					//{
					//	archer.animCtrl.SetBool("bMoveDir", true);
					//}

					float animSpd = Vector3.Magnitude(archer.navAgent.velocity) / archer.status.moveSpd;
					if (animSpd >= 0.1f)
					{
						archer.animCtrl.SetFloat("fWalkSpd", animSpd);
						archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1);
						archer.animCtrl.SetInteger("iMoveDir", (int)direction);
					}
					else
					{
						archer.isMove = false;
						archer.animCtrl.SetBool("bMoveDir", false);
					}
				}
				break;
			case eArcherMoveDir.Backward:
				{
					Vector3 nextPos = archer.transform.position +(-archer.dirToTarget * (archer.status.moveSpd *0.6f)/** Time.deltaTime*/);

					if (!IsCanMove(nextPos))
					{
						archer.actTable.MoveWhileAttack(eArcherMoveDir.End);
						return;
					}
					
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

					float animSpd = Vector3.Magnitude(archer.navAgent.velocity) / archer.status.moveSpd;
					if (animSpd >= 0.1f)
					{
						archer.animCtrl.SetFloat("fWalkSpd", animSpd);
						archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 1);
						archer.animCtrl.SetInteger("iMoveDir", (int)direction);
					}
					else
					{
						archer.isMove = false;
						archer.animCtrl.SetBool("bMoveDir", false);
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

	public void SideWalkThink(eArcherSideWalkDir curDirection)
	{
		//현재 방향 검사하고 다른 방향도 검사해서 둘다 안되면, 멈추기
		eArcherMoveDir moveDir;
		eArcherSideWalkDir oppositeDir = curDirection == eArcherSideWalkDir.Right ? eArcherSideWalkDir.Right : eArcherSideWalkDir.Left;

		//if (Random.Range(0f, 100f) <= 70f)
		//{
		//	moveDir = eArcherMoveDir.End;
		//}
		//else
		//{
			if (IsSideCanMove(curDirection) == curDirection)
			{
				//해당 방향으로 갈 수 있으면 걍 가삼
				moveDir = curDirection == eArcherSideWalkDir.Right ? eArcherMoveDir.Right : eArcherMoveDir.Left;
			}
			else
			{//해당 방향ㅇ로 못가면 반대방향으로도 검사

				if (IsSideCanMove(oppositeDir) == curDirection)
				{ //반대방향도 못가는 경우
					moveDir = eArcherMoveDir.End;
				}
				else
				{//반대방향으로는 갈 수 있는 경우
					moveDir = curDirection == eArcherSideWalkDir.Right ? eArcherMoveDir.Left : eArcherMoveDir.Right;
				}
			}
		//}

		archer.actTable.MoveWhileAttack(moveDir);
		//archer.actTable.MoveWhileAttack(IsSideCanMove(curDirection));
	}



	//}
	public bool IsCanMove(Vector3 destPos)
	{
		float upOffset = 0.25f;
		
		Vector3 dest = destPos;
		dest.y += upOffset;

		Vector3 archerPos = archer.transform.position;
		archerPos.y += upOffset;
		
		Vector3 dir = dest - archerPos;
		float distance = Vector3.Magnitude(dir);

		//Ray ray = new Ray();
		//ray.direction = dir;
		//RaycastHit hitInfo;

		if (Physics.Raycast(archerPos, dir, distance, LayerMask.GetMask("Environment")))
		{
			return false;
		}

		return true;
	}

	public eArcherSideWalkDir IsSideCanMove(eArcherSideWalkDir dirEnum)
	{
		Vector3 destPos = archer.transform.position + (archer.transform.right * (archer.status.moveSpd * 0.8f));
		if (dirEnum == eArcherSideWalkDir.Left)
		{ destPos *= -1f; }

		float upOffset = 0.25f;

		Vector3 dest = destPos;
		dest.y += upOffset;

		Vector3 archerPos = archer.transform.position;
		archerPos.y += upOffset;

		Vector3 dir = dest - archerPos;
		float distance = Vector3.Magnitude(dir);

		LayerMask mask = LayerMask.GetMask("Environment") | LayerMask.GetMask("Enemey");


		if (Physics.Raycast(archerPos, dir, distance, mask))
		{
			//return dirEnum == eArcherSideWalkDir.Right ? eArcherMoveDir.Left: eArcherMoveDir.Right;
			return dirEnum == eArcherSideWalkDir.Right ? eArcherSideWalkDir.Left : eArcherSideWalkDir.Right;
		}
		return dirEnum == eArcherSideWalkDir.Right ? eArcherSideWalkDir.Right : eArcherSideWalkDir.Left;
		//return dirEnum == eArcherSideWalkDir.Right ? eArcherMoveDir.Right : eArcherMoveDir.Left;
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

	public void LegIKToGround(AvatarIKGoal ikName)
	{
		//일단 궁수는 얘ㄴㄴㄴ
		if (archer.animCtrl)
		{
			archer.animCtrl.SetIKPositionWeight(ikName, 1);
			archer.animCtrl.SetIKRotationWeight(ikName, 1);

			Vector3 bonePos = archer.animCtrl.GetIKPosition(ikName);
			float distanceGround;
			RaycastHit hitInfo;
			Physics.Raycast(bonePos, Vector3.down, out hitInfo, 100f,LayerMask.GetMask("Environmet"));
			distanceGround = hitInfo.distance;

			Ray leftRay = new Ray(archer.animCtrl.GetIKPosition(ikName) + Vector3.up, Vector3.down);

			if (Physics.Raycast(leftRay, out RaycastHit leftHit, distanceGround + 1f, LayerMask.GetMask("Environmet")))
			{
				Vector3 footPosition = leftHit.point;
				footPosition.y += distanceGround;

				archer.animCtrl.SetIKPosition(ikName, footPosition);
				archer.animCtrl.SetIKRotation(ikName, Quaternion.LookRotation(transform.forward, leftHit.normal));
			}

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

	private void OnDrawGizmosSelected()
	{
		
	}
}
