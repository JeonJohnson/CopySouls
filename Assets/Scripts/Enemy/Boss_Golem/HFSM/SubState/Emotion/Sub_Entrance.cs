using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Entrance : Golem_SubState
{
	public Sub_Entrance(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		golem.combatState = eCombatState.Idle;
	}

	public override void EnterState()
	{
		base.EnterState();
	}

	public override void UpdateState()
	{
		base.UpdateState();

		if (!golem.targetObj)
		{
			return;
		}

		switch (golem.combatState)
		{
			case eCombatState.Idle:
				{
					if (golem.distToTarget <= golem.status.ricognitionRange)
					{

						golem.actTable.GolemAssembleBeginEvent();

						golem.animCtrl.enabled = false;
						golem.combatState = eCombatState.Alert;
						golem.FragScript.Assemble();

                        //golem.hpBar_Boss = UiManager.Instance.InstantiateBossHpBar(golem);
                        golem.hpBar_Boss.gameObject.SetActive(true);
						InGameManager.Instance.BossCombatStart();
					}
				}
				break;
			case eCombatState.Alert:
				{
					if (Funcs.IsAnimationCompletelyFinish(golem.FragScript.animCtrl, "Assemble"))
					{

						golem.actTable.GolemAssembleEndEvent();

						golem.combatState = eCombatState.Combat;

						golem.meshObj.SetActive(true);
						golem.rootObj.SetActive(true);
						golem.ragdoll.gameObject.SetActive(false);
						golem.animCtrl.enabled = true;
						golem.animCtrl.SetTrigger("tRoar");

                    }
				}
				break;
			case eCombatState.Combat:
				{
					if (Funcs.IsAnimationCompletelyFinish(golem.animCtrl, "Roar"))
					{
						hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Move));
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
	}
}
