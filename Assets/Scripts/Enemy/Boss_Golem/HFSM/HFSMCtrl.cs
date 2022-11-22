using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGolemBaseState
{
	Emotion,
	Move,
	Attack,
	Hit,
	End
}

public enum eGolemEmotionState
{
	Entrance,
	Win,
	End,
}

public enum eGolemMoveState
{
	Idle,
	Walk,
	Turn,
	End
}

public enum eGolemAttackState
{
	Melee,
	Forward,
	Range,
	End
}

public enum eGolemHitState
{

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
	public Golem_BaseState curBaseState;
	public Golem_BaseState nextBaseState;

	
	public void InitBaseState()
	{ 
		
	}

	public void SetNextBaseState(Golem_BaseState state)
	{
		if (state == null)
		{
			Debug.LogError("Golem BaseState Null Error");
		}
		nextBaseState = state;
	}

	private void ChangeNextBastState()
	{
		curBaseState.ExitBaseState();

		preBaseState = curBaseState;
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
