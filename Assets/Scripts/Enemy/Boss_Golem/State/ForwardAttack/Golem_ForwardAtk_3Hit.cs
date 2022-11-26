//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Golem_ForwardAtk_3Hit : cGolemState
//{
//	public Golem_ForwardAtk_3Hit(int cost) : base(cost)
//	{
//		atkType = eGolemStateAtkType.MiddleAtk;
//	}

//	string animName;
//	public override void EnterState(Enemy script)
//	{
//		base.EnterState(script);

//		golem.animCtrl.applyRootMotion = true;

//		golem.status.curStamina -= stateCost;

//		golem.animCtrl.SetTrigger("tForAtk3");
//	}

//	public override void UpdateState()
//	{
//		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, "ForAtk3"))
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
//		golem.animCtrl.applyRootMotion = false;
//	}
//}
