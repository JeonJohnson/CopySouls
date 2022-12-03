using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


//public enum eGolemState
//{
//    Think,

//	Idle,
//	Move,
//	Turn,

//    Entrance,
//    //melee Attack
//	MeleeAtk_1Hit,
//    MeleeAtk_2Hit,
//    MeleeAtk_3Hit,
    
//	//Turn,
//	//TurnAtk,
    
//	//Range Attack
//	ForwardAtk_1Hit,
//    ForwardAtk_2Hit,
//    ForwardAtk_3Hit,

//	ThrowRock,
//	//JumpAtk,

//    Hit,
//    Death, //(explode)
//	End
//}




public enum eGolemStateAtkType
{
	None,
	CloseAtk,
	MiddleAtk,
	RangeAtk,
	End
}





public class Golem_ActionTable : MonoBehaviour
{
	Golem golem = null;

	//public cGolemState[] moveFsm;
	//public cGolemState curMoveState = null;
	//public eGolemMoveState curMoveState_e = eGolemMoveState.End;


	//List<cState> statePerCost;
	//	Dictionary<int, List<cGolemState>> statePerCost;

	public float thinkMinTime;
	public float thinkMaxTime;


	public Coroutine decisionCoroutine = null;
	public cGolemState nextState = null;
	
	public bool isWaitForCost;
	public bool isWaitForDist;

	public float curMoveTime;
	public float maxMoveTime;


	bool isLookAt = false;
	[Header("Weapons")]
	public int rockThrowDmg;
	public RockFrag rockScript;
	public int fistDmg;
	public GolemFist[] fistScript;
	public GolemFist[] legScript;

	public int[] hpCriteria;

	#region animationEvents

	public void LookAtEvent(int val)
	{
		isLookAt = Funcs.I2B(val);
	}

	public void PickupRockEvent()
	{
		GameObject rock = ObjectPoolingCenter.Instance.LentalObj("RockFrag");
		if (!rock)
		{
			Debug.LogError("돌덩이 읎는디요");
		}
		rockScript = rock.GetComponent<RockFrag>();
		rockScript.owner = this.gameObject;
		rockScript.Dmg = rockThrowDmg;

		rock.transform.rotation = Random.rotation;
		rockScript.golemRightHandTr = golem.rightHandBoneTr;
		rockScript.golemLeftHandTr = golem.leftHandBoneTr;

		isLookAt = true;
	}
	public void AimRockEvent()
	{
		rockScript.Aiming();
	}
	public void ThrowRockEvent()
	{
		//rockScript.dir = (golem.targetObj.transform.position - rockScript.transform.position).normalized;
		rockScript.dir = (golem.targetHeadTr.position - rockScript.transform.position).normalized;
		rockScript.Throw();
		rockScript = null;
	}

	public void LeftHandCol(int val)
	{
		fistScript[Defines.left].WeaponColliderOnOff(val);
	}
	public void RightHandCol(int val)
	{
		fistScript[Defines.right].WeaponColliderOnOff(val);
	}
	public void BothHandCol(int val)
	{
		LeftHandCol(val);
		RightHandCol(val);
	}

	public void SetAtkType(int type)
	{
		fistScript[Defines.left].atkType = (Enums.eAttackType)type;
		fistScript[Defines.right].atkType = (Enums.eAttackType)type;
	}

	public void LongTimeIdleCheck()
	{
		if (golem.hfsmCtrl.GetCurBaseState != golem.hfsmCtrl.GetBaseState((int)eGolemBaseState.Move))
		{
			golem.hfsmCtrl.SetNextBaseState(golem.hfsmCtrl.GetBaseState((int)eGolemBaseState.Move));
		}
	}


	public void GolemAssembleBeginEvent()
	{
		CameraEffect.instance.PlayShake("Golem_Assemble");
	}
	public void GolemAssembleEndEvent()
	{
		//CameraEffect.instance.PlayShake("Golem_AssembleFin");
	}

	public void GolemScreamShakeEvent()
	{
		CameraEffect.instance.PlayShake("Golem_Scream");
		SoundManager.Instance.PlaySound("Golem_RoarTest", golem.gameObject);
	}

	public void GolemWalkShakeEvent()
	{
		CameraEffect.instance.PlayShake("Golem_Walk");
	}
	#endregion
	//public void OrganizeStatePerCost()
	//{
	//	statePerCost = new Dictionary<int, List<cGolemState>>();

	//	for (int i = 0; i < (int)golem.status.maxStamina; ++i)
	//	{
	//		statePerCost.Add(i, new List<cGolemState>());
	//	}

	//	foreach (cGolemState state in golem.fsm)
	//	{
	//		int cost = state.stateCost;

	//		statePerCost[cost].Add(state);
	//	}
	//}

	public void Awake()
	{
		if (!golem)
		{
			golem = GetComponent<Golem>();
		}

		
	}

	public void Start()
	{
		foreach (GolemFist fist in fistScript)
		{
			fist.owner = gameObject;
			fist.Dmg = fistDmg;
		}
	}

	public void Update()
	{
		CheckAnglToTarget();
		FillStamina();

		if (isLookAt)
		{
			LookAtBody(5f);
		}
	}

	//public void InitMoveFsm()
	//{
	//	moveFsm = new cGolemState[(int)eGolemMoveState.End];

	//	moveFsm[(int)eGolemMoveState.Idle] = new Golem_Idle(0);
	//	moveFsm[(int)eGolemMoveState.Walk] = new Golem_Walk(0);
	//	moveFsm[(int)eGolemMoveState.Turn] = new Golem_Turn(0);

	//	SetMoveState(eGolemMoveState.Walk);
	//	//curMoveState = moveFsm[(int)eGolemMoveState.Walk];
	//}

	//public void SetMoveState(eGolemMoveState state)
	//{
	//	if (curMoveState != null)
	//	{
	//		curMoveState.ExitState();
	//	}

	//	curMoveState = moveFsm[(int)state];
	//	curMoveState.EnterState(golem);
	//	curMoveState_e = state;
	//}

	public bool CheckPlayerClose()
	{
		if (!golem.targetObj)
		{
			return false;
		}

		if (golem.distToTarget <= golem.status.ricognitionRange)
		{
			return true;
		}

		return false;
	}

	public void FillStamina()
	{
		golem.status.curStamina += Time.deltaTime;
		golem.status.curStamina = Mathf.Clamp(golem.status.curStamina, 0f, golem.status.maxStamina);
	}

	public void SelectStateByMovePriority(ref List<cGolemState> list, eGolemMovePriority priority)
	{ 
			


	}

	#region fsm
	public void SortStateByCostPriority(ref List<cGolemState> list, eGolemCostPriority priority)
	{
		switch (priority)
		{
			case eGolemCostPriority.High:
				{
					list = list.OrderByDescending(x => x.stateCost).ToList();
				}
				break;
			case eGolemCostPriority.Rand:
				{
					Funcs.ListShuffle(ref list);
				}
				break;
			case eGolemCostPriority.Low:
				{
					list = list.OrderByDescending(x => x.stateCost).ToList();
					list.Reverse();
				}
				break;
			case eGolemCostPriority.End:
				break;
			default:
				break;
		}
	}

	public void SortStateByCostWait(ref List<cGolemState> list, eGolemCostWait waitState)
	{
		if (waitState == eGolemCostWait.None)
		{
			EraseCondition(ref list, (x => x.stateCost > golem.status.curStamina));
		}
		else
		{ 
			
			
		}
	}

	public void EraseCondition(ref List<cGolemState> refList, System.Predicate<cGolemState> match)
	{
		List<cGolemState> findAllState = refList.FindAll(match);

		if (findAllState == null)
		{
			return;
		}
		for (int i = 0; i < findAllState.Count; ++i)
		{
			refList.Remove(findAllState[i]);
		}
	}
	#endregion

	#region HFSM
	public void SortStateByCostPriority(ref List<Golem_SubState> list, eGolemCostPriority priority)
	{
		switch (priority)
		{
			case eGolemCostPriority.High:
				{
					list = list.OrderByDescending(x => x.stateCost).ToList();
				}
				break;
			case eGolemCostPriority.Rand:
				{
					Funcs.ListShuffle(ref list);
				}
				break;
			case eGolemCostPriority.Low:
				{
					list = list.OrderByDescending(x => x.stateCost).ToList();
					list.Reverse();
				}
				break;
			case eGolemCostPriority.End:
				break;
			default:
				break;
		}
	}

	public void SortStateByCostWait(ref List<Golem_SubState> list, eGolemCostWait waitState)
	{
		if (waitState == eGolemCostWait.None)
		{
			EraseCondition(ref list, (x => x.stateCost > golem.status.curStamina));
		}
		else
		{


		}
	}

	public void EraseCondition(ref List<Golem_SubState> refList, System.Predicate<Golem_SubState> match)
	{
		List<Golem_SubState> findAllState = refList.FindAll(match);

		if (findAllState == null)
		{
			return;
		}
		for (int i = 0; i < findAllState.Count; ++i)
		{
			refList.Remove(findAllState[i]);
		}
	}
	#endregion

	//public bool CheckNoThinkLongTime()
	//{
	//	//가끔 Think 씹혀서 오랫동안 움직이기만 할 때 체크해서 다시 생각하기 위해서 

	//	if (curMoveTime >= maxMoveTime )	
	//	{
	//		if (decisionCoroutine == null)
	//		{
	//			golem.SetState((int)eGolemState.Think);
	//			curMoveTime = 0f;
	//			return true;
	//		}
	//	}

	//	return false;
	//}

	//public void CheckNextStateCondition()
	//{
	//	if (nextState == null)
	//	{
	//		return;
	//	}

	//	if (isWaitForCost)
	//	{
	//		if (golem.status.curStamina >= nextState.stateCost)
	//		{
	//			golem.SetState(nextState);
	//			isWaitForCost = false;
	//		}
	//	}

	//	if (isWaitForDist)
	//	{
	//		if (nextState.atkType == eGolemStateAtkType.CloseAtk)
	//		{
	//			if (golem.distToTarget <= golem.status.atkRange)
	//			{
	//				golem.SetState(nextState);
	//				isWaitForDist = false;
	//			}
	//		}
	//		else if (nextState.atkType == eGolemStateAtkType.MiddleAtk)
	//		{
	//			if (golem.distToTarget <= golem.rangeAtkRange)
	//			{
	//				golem.SetState(nextState);
	//				isWaitForDist = false;
	//			}
	//		}
	//	}


	//}


	public void AddCondition(ref List<cGolemState> refList, System.Predicate<cGolemState> match)
	{ 
		
	}

	//public void /*cState*/ Decision(ref List<eGolemState)
	//{

	//}

	public eSideDirection TargetOnWhichSide(Vector3 forward, Vector3 dir, Vector3 up, float offset = 0f)
	{
		Vector3 dirCrossForward = Vector3.Cross(forward, dir);
		float dot = Vector3.Dot(dirCrossForward, up);

		if (dot > 0f + offset)
		{
			return eSideDirection.Right;
		}
		else if (dot < 0f + offset)
		{
			return eSideDirection.Left;
		}
		else
		{
			return eSideDirection.Straight;
		}
	}


	public void CheckAnglToTarget()
	{
		golem.angleToTarget = Mathf.Acos(Vector3.Dot(transform.forward, golem.dirToTarget)) * Mathf.Rad2Deg;
		golem.targetWhichSide = TargetOnWhichSide(transform.forward, golem.dirToTarget, transform.up);
	}

	public void LookAtHead()
	{
		//일단 회전 각도 부터 체크하고 일정 각도 이하일때만 돌아가도록.	
		if (golem.angleToTarget < 90f)
		{
			//golem.LookAtSpecificBone(golem.headBoneTr, golem.targetHeadTr, Enums.eGizmoDirection.Down	);
		}
	}

	public void LookAtBody(float spd)
	{
		Vector3 tempDir = golem.dirToTarget;
		tempDir.y = 0;

		Quaternion angle = Quaternion.LookRotation(tempDir);

		transform.rotation = Quaternion.Lerp(golem.transform.rotation, angle, Time.deltaTime * spd);
	}



	//public void Move()
	//{
	//	if (golem.distToTarget <= golem.status.atkRange + 0.5f)
	//	{ //이러면 회전만
	//		if (golem.angleToTarget >=45f)
	//		{
	//			//golem.SetState((int)eGolemState.Turn);

	//			//golem.animCtrl.applyRootMotion = true;
	//			LookAtBody(transform, golem.targetObj.transform, 2f);
	//			if (!golem.animCtrl.GetCurrentAnimatorStateInfo(0).IsName(rotateAnimName))
	//			{
	//				golem.animCtrl.SetTrigger("tRotate");

	//				switch (golem.targetWhichSide)
	//				{
	//					case eSideDirection.Left:
	//						{
	//							rotateAnimName = "Turn_Left";
	//							golem.animCtrl.SetInteger("iRot", -1);
	//						}
	//						break;
	//					case eSideDirection.Right:
	//						{
	//							rotateAnimName = "Turn_Right";
	//							golem.animCtrl.SetInteger("iRot", 1);
	//						}
	//						break;
	//					default:
	//						break;
	//				}
	//			}
	//		}
	//		else
	//		{
	//			golem.animCtrl.SetTrigger("tIdle");
	//		} 
	//	}
	//	else if (golem.distToTarget > golem.status.atkRange + 0.5f)
	//	{
	//		//if (!golem.animCtrl.GetCurrentAnimatorStateInfo(0).IsName(rotateAnimName))
	//		//{
	//			golem.animCtrl.applyRootMotion = false;

	//		//}
	//	}
	
	//}
}
