using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class Archer_Return : cState
{
	Archer archer = null;

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		archer.combatState = eCombatState.Idle;
		archer.animCtrl.SetTrigger("tEquip");
		archer.animCtrl.SetBool("bEquip", false);

		archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 0f);

		switch (archer.defaultPattern)
		{
			case Enums.eArcherState.Idle:
				{
					
				}
				break;
			case Enums.eArcherState.Patrol:
				break;
		}

	}

	public override void UpdateState()
	{
		if (Funcs.IsAnimationAlmostFinish(archer.animCtrl, "Archer_Unequip"))
		{
			archer.weapon.gameObject.SetActive(false);
			archer.weaponEquipState = eEquipState.UnEquip;
			archer.animCtrl.SetTrigger("tWalk");
		}
		else
		{
			if (archer.CheckTargetInFov())
			{
				archer.SetState((int)eArcherState.Bow_Equip);
			}
		}



		switch (archer.defaultPattern)
		{
			case Enums.eArcherState.Idle:
				{
					archer.navAgent.SetDestination(archer.initPos);
					

					float dist = Vector3.Distance(archer.transform.position, archer.initPos);

					if (dist <= 0.15f)
					{
						archer.SetState((int)Enums.eArcherState.Idle);
					}
				}
				break;
			case Enums.eArcherState.Patrol:
				{ 
				

				}
				break;
		}
	}

	public override void ExitState()
	{
		
	}

}
