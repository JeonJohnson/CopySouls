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


public class Golem_ActionTable : MonoBehaviour
{
	Golem golem = null;

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
		OrganizeStatePerCost();
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

	public void /*cState*/ Decision()
	{
		if (golem.distToTarget <= golem.status.atkRange)
		{
			//가까우면 근접 공격 우선
			//
		}
		else
		{ 
		
		}
	}


}
