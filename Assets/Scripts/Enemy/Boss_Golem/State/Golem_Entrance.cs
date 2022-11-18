using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Entrance : cGolemState
{
	public Golem_Entrance(int cost) : base(cost)
	{
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

	}

	public override void UpdateState()
	{
		switch (golem.combatState)
		{
			case eCombatState.Idle:
				{
					if (golem.distToTarget <= golem.status.ricognitionRange)
					{
						golem.combatState = eCombatState.Alert;
						golem.FragScript.Assemble();
					}
				}
				break;
			case eCombatState.Alert:
				{
					if (Funcs.IsAnimationCompletelyFinish(golem.FragScript.animCtrl, "Assemble"))
					{
						golem.combatState = eCombatState.Combat;

						golem.meshObj.SetActive(true);
						golem.rootObj.SetActive(true);
						golem.ragdoll.gameObject.SetActive(false);

						golem.animCtrl.SetTrigger("tRoar");
					}
				}
				break;
			case eCombatState.Combat:
				{
					if (Funcs.IsAnimationCompletelyFinish(golem.animCtrl, "Roar"))
					{
						golem.SetState((int)eGolemState.Think);
					}
				}
				break;
			case eCombatState.End:
				{ 
				
				}
				break;

			default:
				break;
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
	}
}
