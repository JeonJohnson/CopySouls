using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGolemBaseState
{
	Emotion,
	Move,
	Attack,
	Damaged,
	End
}

public class HFSMCtrl : MonoBehaviour
{
	[HideInInspector]
	public Golem golem;
	[HideInInspector]
	public Golem_ActionTable table;

	[HideInInspector]
	public Golem_BaseState[] baseStates;
	[HideInInspector]
	public Golem_BaseState preBaseState;
	private Golem_BaseState curBaseState;
	public Golem_BaseState GetCurBaseState
	{
		get
		{
			return curBaseState;
		}
	}
	public Golem_BaseState nextBaseState;

	public float thinkMinTime;
	public float thinkMaxTime;
	public float thinkTime;
	public Golem_BaseState GetBaseState(int index)
	{
		if (index >= baseStates.Length)
		{
			return null;
		}

		return baseStates[index];
	}
	
	public void InitBaseState()
	{
		baseStates = new Golem_BaseState[(int)eGolemBaseState.End];

		baseStates[(int)eGolemBaseState.Emotion] = new Base_Emotion(this, "Emotion");
		baseStates[(int)eGolemBaseState.Move] = new Base_Move(this, "Move");
		baseStates[(int)eGolemBaseState.Attack] = new Base_Attack(this, "Attack");
		baseStates[(int)eGolemBaseState.Damaged] = new Base_Damaged(this, "Damaged");

		nextBaseState = baseStates[(int)eGolemBaseState.Emotion];
	}

	public void SetNextBaseState(Golem_BaseState state)
	{
		if (state == null)
		{
			Debug.LogError("Golem BaseState Null Error");
		}
		nextBaseState = state;
	}

	public void SetNextBaseStateByIndex(int index)
	{
		//if (state == null)
		//{
		//	Debug.LogError("Golem BaseState Null Error");
		//}
		nextBaseState = baseStates[index];
	}


	public void SetNextBaseStateWithSubStateIndex(Golem_BaseState state, int index)
	{
		if (state == null)
		{
			Debug.LogError("Golem BaseState Null Error");
		}
		state.nextSubState = state.GetSubState(index);
		nextBaseState = state;
	}
	
	private void ChangeNextBastState()
	{
		if (curBaseState != null)
		{ 
			curBaseState.ExitBaseState();
			preBaseState = curBaseState;
		}
		
		curBaseState = nextBaseState;
		nextBaseState = null;

		curBaseState.EnterBaseState();
	}


	public void Awake()
	{
		golem = GetComponent<Golem>();
		table = GetComponent<Golem_ActionTable>();
	}

	// Start is called before the first frame update
	public void Start()
    {
		InitBaseState();
    }

    // Update is called once per frame
    public void Update()
	{
		if (nextBaseState != null)
		{
			ChangeNextBastState();
		}

		if (curBaseState != null)
		{
			curBaseState.UpdateBaseState();
		}
	}

	public void LateUpdate()
	{
		if (curBaseState != null)
		{
			curBaseState.LateUpdateBaseState();
		}
	}

	public void FixedUpdate()
	{
		if (curBaseState != null)
		{
			curBaseState.FixedUpdateBaseState();
		}
	}


	
}
