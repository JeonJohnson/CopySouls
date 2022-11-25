//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Golem_MeleeAtk_3Hit : cGolemState
//{
//	public Golem_MeleeAtk_3Hit(int cost) : base(cost)
//	{
//		atkType = eGolemStateAtkType.CloseAtk;

		
//	}

//	public override void EnterState(Enemy script)
//	{
//		base.EnterState(script);

//		golem.status.curStamina -= stateCost;

//		golem.animCtrl.SetTrigger("tAtk3");
		

//	}

//	public override void UpdateState()
//	{
//		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, "3Attack"))
//		{
//			golem.SetState((int)eGolemState.Think);
//		}


//	}

//	public override void LateUpdateState()
//	{
//		base.LateUpdateState();
//	}
//	public override void FixedUpdateState()
//	{
//		base.FixedUpdateState();
//	}

//	public override void ExitState()
//	{
//	}
//}
