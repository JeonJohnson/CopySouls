using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class Archer_Hit_Hold : cState
{
	Archer archer = null;

	public enum eHoldType
	{ 
		Front = 1,
		Back = 2,
		End
	}

	eHoldType holdType;
	string animName;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);
		
		if (!archer)
		{
			archer = script.GetComponent<Archer>();
		}

		holdType = archer.status.isFrontHold ? eHoldType.Front : eHoldType.Back;
		animName = archer.status.isFrontHold ? "FrontHold" : "BackHold";

		archer.animCtrl.SetTrigger("tHit");
		archer.animCtrl.SetBool("bHit_Hold", true);
		archer.animCtrl.SetBool("bDontGetup", archer.status.isDead);
		archer.animCtrl.SetInteger("iHoldDir", (int)holdType);


		archer.animCtrl.applyRootMotion = true;

		
		archer.navAgent.enabled = false;
		archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 0f);

		

		//archer.actTable.ArrowReturn();
	}

	public override void UpdateState()
	{
		//base.UpdateState();
		if (archer.status.curHp <= 0f)
		{
			if (Funcs.IsAnimationCompletelyFinish(archer.animCtrl, animName,0.95f))
			{
				archer.status.isDead = true;
				archer.ActiveRagdoll();
				archer.DeathReset();
				archer.gameObject.SetActive(false);
			}
		}
		else
		{
			if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "GetUp"))
			{
				if (archer.combatState == eCombatState.Combat | archer.combatState == eCombatState.Alert)
				{
					archer.SetState((int)archer.actTable.RandomAttackState());
				}
				else if (archer.combatState == eCombatState.Idle | archer.weaponEquipState == eEquipState.UnEquip)
				{
					archer.SetState((int)eArcherState.Bow_Equip);
				}
			}
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
		archer.status.isBackHold = false;
		archer.status.isFrontHold = false;

		archer.navAgent.enabled = true;
		archer.animCtrl.SetBool("bHit_Hold", false);
		archer.animCtrl.applyRootMotion = false;
	}
}
