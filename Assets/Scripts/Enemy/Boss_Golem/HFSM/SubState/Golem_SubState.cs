using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Animations;
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
	public string stateName;
	public string animName;
	
	[HideInInspector]
	public Golem golem = null;
	[HideInInspector]
	public Golem_ActionTable table = null;
	[HideInInspector]
	public HFSMCtrl hfsmCtrl = null;

	[HideInInspector]
	public Golem_BaseState baseState;

	public int stateCost;
	public eGolemAtkRangeType atkRangeType = eGolemAtkRangeType.None;

	public Golem_SubState(Golem_BaseState _baseState, string name)
	{
		baseState = _baseState;
		golem = _baseState.golem;
		table = _baseState.table;
		hfsmCtrl = _baseState.hfsmCtrl;

		stateName = name;
	}

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
