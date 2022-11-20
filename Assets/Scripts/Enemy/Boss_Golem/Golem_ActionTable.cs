using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


public enum eGolemState
{
    Think,

	Idle,
	Walk,
	Turn,

    Entrance,
    //melee Attack
	MeleeAtk_1Hit,
    MeleeAtk_2Hit,
    MeleeAtk_3Hit,
    
	//Turn,
	//TurnAtk,
    
	//Range Attack
	ForwardAtk_1Hit,
    ForwardAtk_2Hit,
    ForwardAtk_3Hit,

	ThrowRock,
	JumpAtk,

    Hit,
    Death, //(explode)
	End
}

public enum eGolemMoveState
{ 
	Idle,
	Walk,
	Turn,
	//Rotate_Left,
	//Rotate_Right,
	End
}

public enum eGolemStateAtkType
{ 
	None,
	CloseAtk,
	MiddleAtk,
	RangeAtk,
	End
}


public enum eGolemMovePriority
{ 
	None,
	Move,
	End
}

public enum eGolemCostPriority
{ 
	High,
	Rand,
	Low,
	End
}

public enum eGolemCostWait
{ 
	Wait,
	None,
	End
}


public class Golem_ActionTable : MonoBehaviour
{
	Golem golem = null;

	//public cGolemState[] moveFsm;
	//public cGolemState curMoveState = null;
	//public eGolemMoveState curMoveState_e = eGolemMoveState.End;


	//List<cState> statePerCost;
	Dictionary<int, List<cGolemState>> statePerCost;

	public Coroutine decisionCoroutine = null;

	//string rotateAnimName;

	bool isLookAt = false;

	#region animationEvents

	public void LookAtEvent(int val)
	{
		isLookAt = Funcs.I2B(val);
	}

	public void PickupRockEvent()
	{ 
	
	}
	public void ThrowRockEvent()
	{ 
	
	
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
		//InitMoveFsm();
		//OrganizeStatePerCost();
	}

	public void Update()
	{
		CheckAnglToTarget();
		FillStamina();

		if (isLookAt)
		{
			LookAtBody(4f);
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
	}

	public void EraseCondition(ref List<cGolemState> refList, System.Predicate<cGolemState> match)
	{
		List<cGolemState> findAllState = refList.FindAll(match);

		for (int i = 0; i < findAllState.Count; ++i)
		{
			refList.Remove(findAllState[i]);
		}
	}

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
