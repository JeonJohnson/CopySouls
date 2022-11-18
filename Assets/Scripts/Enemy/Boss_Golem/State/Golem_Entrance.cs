using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Entrance : cState
{
	Golem golem = null;
	Golem_ActionTable table = null;
	int stateCost;

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (!golem)
		{
			golem = script as Golem;
			table = golem.actTable;
		}

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
					
				}
				break;
			case eCombatState.Combat:
				{ 
				
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
