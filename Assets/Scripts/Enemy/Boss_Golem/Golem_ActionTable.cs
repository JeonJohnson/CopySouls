using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGolemState
{
    Think,
    Entrance,
    MeleeAtk_1Hit,
    MeleeAtk_2Hit,
    MeleeAtk_3Hit,
    TurnAtk,
    ForwardAtk_1Hit,
    ForwardAtk_2Hit,
    ForwardAtk_3Hit,
    Hit,
    Death, //(explode)
    End
}


public class Golem_ActionTable : MonoBehaviour
{
	Golem golem = null;


	public void Awake()
	{
		if (!golem)
		{
			golem = GetComponent<Golem>();
		}
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




}
