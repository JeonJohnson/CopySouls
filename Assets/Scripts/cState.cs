using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class cState
{
	public Enemy me;

	public virtual void EnterState(Enemy script)
	{
		if (me == null)
		{
			me = script;
		}
	}

	public abstract void UpdateState();

	public abstract void ExitState();

}
