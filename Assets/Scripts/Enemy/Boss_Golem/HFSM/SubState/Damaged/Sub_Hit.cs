using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eGolemHitState
{ 
	Light,
	Medium,
	Heavy,
	Death,
	End
}

public class Sub_Hit : Golem_SubState
{

	eGolemHitState curState = eGolemHitState.Light;
	float hpRatio;
	public Sub_Hit(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 0;

	}

	public void HitStateCheck()
	{
		float curHp = golem.status.curHp;
		float fullHp = golem.status.maxHp;
		hpRatio = golem.status.curHp / fullHp;
		hpRatio *= 100f;


		switch (curState)
		{
			case eGolemHitState.Light:
			case eGolemHitState.Medium:
			case eGolemHitState.Heavy:
				{
					if (hpRatio <= table.hpCriteria[(int)curState]) //75 ¹Ì¸¸
					{
						golem.animCtrl.SetTrigger("tHit");
						golem.animCtrl.SetInteger("iHit_Num", (int)curState);
						

						curState += 1;
						animName = $"Hit_{(int)curState}";

						golem.animCtrl.applyRootMotion = true;
					}
					else
					{ hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Move)); }
				}
				break;
			case eGolemHitState.Death:
				{
					if (golem.status.curHp <= 0)//Á×À½
					{
						baseState.SetSubState(baseState.GetSubState((int)eGolemDamagedState.Death));
						curState += 1;

					}
					else
					{ hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Move)); }
				}
				break;
			default:
				break;
		}

		
	}

	public override void EnterState()
	{ 
		base.EnterState();

		HitStateCheck();

	}
	public override void UpdateState()
	{
		base.UpdateState();

		if (curState != eGolemHitState.End)
		{
			if (Funcs.IsAnimationAlmostFinish(golem.animCtrl, animName))
			{
				hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Move));
			}
		}
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
		base.ExitState();
		golem.animCtrl.applyRootMotion = false;
	}
}

