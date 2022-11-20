using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


public enum eGolemState
{
    Think,

    Entrance,
    //melee Attack
	MeleeAtk_1Hit,
    MeleeAtk_2Hit,
    MeleeAtk_3Hit,
    
	Turn,
	TurnAtk,
    
	//Range Attack
	ForwardAtk_1Hit,
    ForwardAtk_2Hit,
    //ForwardAtk_3Hit,
	ThrowRock,
	JumpAtk,

    Hit,
    Death, //(explode)
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


public enum eGolemMoveState
{ 
	Move,
	Rotate_Left,
	Rotate_Right,
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

	public List<cState> moveFsm;

	//List<cState> statePerCost;
	Dictionary<int, List<cGolemState>> statePerCost;

	public void OrganizeStatePerCost()
	{
		statePerCost = new Dictionary<int, List<cGolemState>>();

		for (int i = 0; i < (int)golem.status.maxStamina; ++i)
		{
			statePerCost.Add(i, new List<cGolemState>());
		}

		foreach (cGolemState state in golem.fsm)
		{
			int cost = state.stateCost;

			statePerCost[cost].Add(state);
		}
	}

	public void Awake()
	{
		if (!golem)
		{
			golem = GetComponent<Golem>();
		}
	}

	public void Start()
	{
		//OrganizeStatePerCost();
	}

	public void Update()
	{
		CheckAnglToTarget();
		FillStamina();
	}

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

	public void /*cState*/ Decision()
	{
		bool isMove = Funcs.I2B(Random.Range(0, 2));


		if (golem.distToTarget <= golem.status.atkRange)
		{
			//가까우면 근접 공격 우선
			//
		}
		else
		{ 
		
		}
	}

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

	public void LookAtTargetHead()
	{
		//일단 회전 각도 부터 체크하고 일정 각도 이하일때만 돌아가도록.	
		if (golem.angleToTarget < 90f)
		{
			//golem.LookAtSpecificBone(golem.headBoneTr, golem.targetHeadTr, Enums.eGizmoDirection.Down	);
		}
	}



	public void Move()
	{
		if (golem.distToTarget <= golem.status.atkRange + 1f)
		{ //이러면 회전만
			if (golem.angleToTarget >= 90f)
			{
				golem.animCtrl.applyRootMotion = true;
				if (!golem.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("turn90Right"))
				{
					golem.animCtrl.SetTrigger("tRotate");
					golem.animCtrl.SetInteger("iRotDir", 1);
				}
			}
		}
		else
		{
			if (!golem.animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
			{
				golem.animCtrl.SetTrigger("tMove");
			}
			golem.navAgent.SetDestination(golem.targetObj.transform.position);

		}
	}
}
