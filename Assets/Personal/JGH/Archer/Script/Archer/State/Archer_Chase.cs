using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Chase : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);
		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }
		
		//추적 흐름 

		//여기 들어오는 조건, CheckTargetIsHiding에서 없어 졌을때,
		//여기서는 일단 그 마지막으로 보이던 위치 

	}

	public override void UpdateState()
	{
		
	}
	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
	}
	public override void ExitState()
	{
		throw new System.NotImplementedException();
	}
}
