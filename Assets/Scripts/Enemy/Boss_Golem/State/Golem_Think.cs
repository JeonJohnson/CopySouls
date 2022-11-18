using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Think : cGolemState
{
	public Golem_Think(int cost) : base(cost)
	{ 
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);


	}

	public override void UpdateState()
	{
		{
			golem.status.curStamina += Time.deltaTime;
			golem.status.curStamina = Mathf.Clamp(golem.status.curStamina, 0f, golem.status.maxStamina);
		}

		





	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
	}
	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void ExitState()
	{
	}
}
