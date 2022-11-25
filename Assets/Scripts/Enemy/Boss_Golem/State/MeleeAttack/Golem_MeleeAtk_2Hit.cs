//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Golem_MeleeAtk_2Hit : cGolemState
//{
//	string animName;
//	public Golem_MeleeAtk_2Hit(int cost) : base(cost)
//	{
//		atkType = eGolemStateAtkType.CloseAtk;
//	}

//	public override void EnterState(Enemy script)
//	{
//		base.EnterState(script);

//		golem.status.curStamina -= stateCost;

//		golem.animCtrl.SetTrigger("tAtk2");
//		int iRand = Random.Range(1, 3);
//		animName = $"2Attack_{iRand}";
//		golem.animCtrl.SetInteger("iAtk2_Num", iRand);
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
//	}
//}
