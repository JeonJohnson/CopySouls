using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_cState
{
	public Player me;
	public virtual void EnterState(Player script)
	{
		if (me == null)
		{
			me = script;
		}
	}

	public abstract void UpdateState();

	public abstract void ExitState();
}
