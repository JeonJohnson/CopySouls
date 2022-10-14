using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Hit : cState
{
	Archer archer = null;
	int rand;
	//string animName = "Archer_Hit";

	public enum eNextState
	{ 
		PreState,
		AttackMelee,
		Run,
		End
	}

	public void RandomNextState()
	{
		//int iRand = Random.Range(0, 4);
		eNextState temp = (eNextState)Random.Range(0, 4);
		switch (temp)
		{
			case eNextState.PreState:
				{ }
				break;
			case eNextState.AttackMelee:
				{ }
				break;
			case eNextState.Run:
				{ }
				break;
			case eNextState.End:
				{ }
				break;
			default:
				break;
		}


	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		rand = Random.Range(1, 3);

		me.animCtrl.SetInteger("iHit", rand);

		//animName = "Archer_Hit" + $"_0{rand}";

	}
	public override void UpdateState()
	{
		if(Funcs.IsAnimationAlmostFinish(me.animCtrl,$"Archer_Hit_0{rand}"))
		{
			me.SetState(me.preState);
		}
	}

	public override void ExitState()
	{

	}

}
