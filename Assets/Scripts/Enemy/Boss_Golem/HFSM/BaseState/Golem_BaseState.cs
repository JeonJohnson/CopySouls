using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Golem_BaseState
{
	public string stateName;

	[HideInInspector]
	public Golem golem = null;
	[HideInInspector]
	public Golem_ActionTable table = null;
	[HideInInspector]
	public HFSMCtrl hfsmCtrl = null;

	[HideInInspector]
	public Golem_SubState[] subStates;
	[HideInInspector]
	public Golem_SubState preSubState;
	public Golem_SubState curSubState;
	public Golem_SubState nextSubState;

	[HideInInspector]
	public Golem_SubState referSubState;

	public Golem_BaseState(HFSMCtrl script, string name)
	{
		hfsmCtrl = script;
		golem = script.golem;
		table = script.table;
		stateName = name;
	}

	public virtual void InitBaseState()
	{ }

	public virtual void SetSubState(Golem_SubState subState)
	{
		if (subState == null)
		{
			Debug.LogError("GolemSubState Null Error");
		}

		nextSubState = subState;
	}

	protected virtual void ChangeNextState()
	{
		curSubState.ExitState();

		preSubState = curSubState;
		curSubState = nextSubState;
		nextSubState = null;

		curSubState.EnterState();
	}

	public virtual void EnterBaseState()
	{

	}

	public virtual void UpdateBaseState()
	{
		if (nextSubState != null)
		{
			ChangeNextState();
		}

		if (curSubState != null)
		{
			curSubState.UpdateState();
		}
	}
	public virtual void LateUpdateBaseState()
	{
		if (curSubState != null)
		{
			curSubState.LateUpdateState();
		}
	}

	public virtual void FixedUpdateBaseState()
	{
		if (curSubState != null)
		{
			curSubState.FixedUpdateState();
		}
	}

	public virtual void ExitBaseState()
	{

	}
}
