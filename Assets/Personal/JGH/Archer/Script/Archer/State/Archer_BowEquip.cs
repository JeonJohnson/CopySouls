using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class Archer_BowEquip : cState
{
	Archer archer = null;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		archer.combatState = eCombatState.Alert;

		archer.animCtrl.SetTrigger("tEquip");
		archer.animCtrl.SetBool("bEquip", true);
	}

	public override void UpdateState()
	{
		if (archer.weaponEquipState == eEquipState.Equip)
		{


			//archer.SetState((int)archer.actTable.RandomAttackState());

			archer.SetState((int)Enums.eArcherState.Attack_Precision);

			//if (archer.distToTarget <= archer.status.atkRange)
			//{
			//	archer.SetState((int)Enums.eArcherState.Attack_Rushed);
			//}
			//else
			//{
			//	archer.SetState((int)Enums.eArcherState.Attack_Precision);
			//}
		}
	}

    public override void LateUpdateState()
    {
        base.LateUpdateState();

		archer.actTable.LookTargetRotate();
    }

    public override void ExitState()
	{
		
	}

}
