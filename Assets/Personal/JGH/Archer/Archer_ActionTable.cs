using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;
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

	public IEnumerator LegLayerWeightCoroutine(int amount)
	{

		yield return null;

	}

	public void LookTargetRotate()
	{//Use at LateUpdate!!!

		//움직임이 없을 경우
		//=> 얼굴이랑 같이 천천히 LookAt으로 돌아감 (딱히 발 모션없음)
		//=> 대신 90라던가 각도가 엄청 차이나면 회전 애니메이션 재생.
		//움직임이 있는 경우(걷기, 뛰기 등)
		//=> 발 애니메이션이 있기에 그냥 NavMeshAgent의 rotate로 해도 딱히 안 이상함.
		//=> 대신 이것도 각도 차이가 많이 나면 회전 애니메이션 재생함.

		float angleToTarget = Mathf.Acos(Vector3.Dot(transform.forward, archer.dirToTarget)) * Mathf.Rad2Deg;
		
		if (archer.navAgent.velocity != Vector3.zero)
		{ //움직임이 있는 경우 
			
			//1. 얼굴만 돌리기
			archer.LookAtSpecificBone(archer.headBoneTr, archer.targetHeadTr, eGizmoDirection.Foward);

			//2. 얼굴이랑 몸이랑 각도가 너무 차이 날 경우

		}
		else 
		{ //움직임이 없는 경우
			if (angleToTarget < 90f)
			{
				//차이가 90도 안일 경우
				
				//얼굴 그쪽으로 바라보면서 몸 돌리기 + 제자리 발 애니메이션
				archer.LookAtSpecificBone(archer.headBoneTr, archer.targetHeadTr, eGizmoDirection.Foward);
				archer.LookAtSlow(archer.transform, archer.targetObj.transform, 1f);


				LegRotateInPlace(angleToTarget);
			}
			else
			{
				//차이가 90도 보다 클 경우

				//animator apply root motion 키고 애니메이션 재생
				eSideDirection dirNum = archer.TargetOnWhichSide(transform.forward,archer.dirToTarget,transform.up);
				if (dirNum == eSideDirection.Left)
				{
					
				}
				else if (dirNum == eSideDirection.Right)
				{ 
				
				}

			}

		}
	}

	private void LegRotateInPlace(float angle)
	{
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
		archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, angle / 90f);
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
