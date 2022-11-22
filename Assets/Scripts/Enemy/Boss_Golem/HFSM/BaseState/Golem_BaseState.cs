using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Golem_BaseState
{
	public string stateName;

	public Golem golem = null;
	public Golem_ActionTable table = null;
	public HFSMCtrl hfsmCtrl = null;

	public Golem_SubState[] subStates;
	public Golem_SubState preSubState = null;
	public Golem_SubState curSubState = null;
	public Golem_SubState nextSubState = null;

	public Golem_SubState referSubState = null;

	public Golem_BaseState(HFSMCtrl script, string name)
	{
		hfsmCtrl = script;
		golem = script.golem;
		table = script.table;
		stateName = name;

		InitBaseState();
	}

	public virtual void InitBaseState()
	{ }

	public Golem_SubState GetSubState(int index)
	{
		if (index >= subStates.Length)
		{
			return null;
		}

		return subStates[index];
	}

	public void SetSubState(Golem_SubState subState)
	{

		if (subState == null)
		{
			Debug.LogError("GolemSubState Null Error");
		}

		nextSubState = subState;
	}

	protected void ChangeNextState()
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
		if (curSubState != null)
		{
			curSubState.ExitState();
			preSubState = curSubState;
			curSubState = null;
		}
	}
}
