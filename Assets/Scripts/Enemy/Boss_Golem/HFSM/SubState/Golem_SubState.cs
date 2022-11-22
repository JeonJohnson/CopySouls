using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eGolemAtkRangeType
{
	None,
	CloseAtk,
	MiddleAtk,
	RangeAtk,
	End
}

//[System.Serializable]
public class Golem_SubState 
{
	//public string name;

	[HideInInspector]
	public Golem golem = null;
	[HideInInspector]
	public Golem_ActionTable table = null;
	[HideInInspector]
	public HFSMCtrl hfsmCtrl = null;

	[HideInInspector]
	public Golem_BaseState baseState;

	public int stateCost;
	public eGolemAtkRangeType atkRangeType;


	public virtual void EnterState()
	{
	}

	public virtual void UpdateState()
	{
	}
	public virtual void LateUpdateState()
	{
	}
	public virtual void FixedUpdateState()
	{
	}


	public virtual void ExitState()
	{
	}
}
