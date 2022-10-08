using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;
using Structs;


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

	public virtual void LateUpdateState()
	{ }

	public abstract void ExitState();

}
