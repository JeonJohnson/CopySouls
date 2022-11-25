//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Golem_JumpAtk : cGolemState
//{
//	enum eState
//	{ 
//		Jump,
//		InAir,
//		Landing,
//		End
//	}

//	eState curState = eState.End;
//	float curTime;
//	public Golem_JumpAtk(int cost) : base(cost)
//	{
//		atkType = eGolemStateAtkType.RangeAtk;
//	}

//	public override void EnterState(Enemy script)
//	{
//		base.EnterState(script);

//		golem.status.curStamina -= stateCost;

//		curTime = 0f;

//		golem.animCtrl.SetTrigger("tJump");
//		golem.navAgent.isStopped = false;
//		//curState = eState.Jump;
//	}

//	public override void UpdateState()
//	{
//		base.UpdateState();

//		curTime += Time.deltaTime;
//		golem.navAgent.SetDestination(golem.targetObj.transform.position);
//		//golem.transform.position = Vector3.Slerp(golem.transform.position, golem.targetObj.transform.position, Time.deltaTime * 5f);
		
//		if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, "JumpAtk"))
//		{
//			golem.SetState((int)eGolemState.Think);
//		}

		
//		////golem.transform.position = Vector3.Lerp()
//		//switch (curState)
//		//{
//		//	case eState.Jump:
//		//		{
//		//			if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, "JumpAtk_Jump"))
//		//			{
//		//				golem.animCtrl.SetTrigger("tInAir");
//		//				curState = eState.InAir;
//		//			}
//		//		}
//		//		break;
//		//	case eState.InAir:
//		//		{
//		//			if (curTime >= 1f)
//		//			{
//		//				golem.animCtrl.SetTrigger("tLanding");
//		//				curState = eState.Landing;
//		//			}
//		//		}
//		//		break;
//		//	case eState.Landing:
//		//		{
//		//			if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, "JumpAtk_Landing"))
//		//			{
//		//				golem.SetState((int)eGolemState.Think);
//		//			}
//		//		}
//		//		break;
//		//	case eState.End:
//		//		break;
//		//	default:
//		//		break;
//		//}



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
//		base.ExitState();
//		golem.navAgent.isStopped = true;
//	}
//}
