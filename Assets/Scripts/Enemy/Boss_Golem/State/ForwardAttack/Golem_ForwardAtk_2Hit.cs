//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Golem_ForwardAtk_2Hit : cGolemState
//{
//	public Golem_ForwardAtk_2Hit(int cost) : base(cost)
//	{
//		atkType = eGolemStateAtkType.MiddleAtk;
//	}

//	string animName;
//	public override void EnterState(Enemy script)
//	{
//		base.EnterState(script);

//		golem.animCtrl.applyRootMotion = true;

//		golem.status.curStamina -= stateCost;
//		int rand = Random.Range(1, 3);
//		animName = $"ForAtk2_{rand}";

//		golem.animCtrl.SetTrigger("tForAtk2");
//		golem.animCtrl.SetInteger("iForAtk2", rand);
//	}

//	public override void UpdateState()
//	{
//		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, animName))
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
