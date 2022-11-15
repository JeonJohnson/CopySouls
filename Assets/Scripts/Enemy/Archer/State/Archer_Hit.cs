using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class Archer_Hit : cState
{
	Archer archer = null;
	
	int rand;
	string animStr;
	public enum eNextState
	{ 
		PreState,
		AttackMelee,
		Run,
		End
	}

	public void RandomNextState()
	{
		//int iRand = Random.Range(0, 4);
		eNextState temp = (eNextState)Random.Range(0, 4);
		switch (temp)
		{
			case eNextState.PreState:
				{ }
				break;
			case eNextState.AttackMelee:
				{ }
				break;
			case eNextState.Run:
				{ }
				break;
			case eNextState.End:
				{ }
				break;
			default:
				break;
		}
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		rand = Random.Range(1, 3);

		me.ResetAllAnimTrigger(Defines.ArcherAnimTriggerStr);

		me.animCtrl.SetTrigger("tHit");
		me.animCtrl.SetInteger("iHit", rand);
		animStr = $"Archer_Hit_0{rand}";

		archer.actTable.MoveWhileAttack(eArcherMoveDir.End);
		archer.animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, 0f);
		//archer.actTable.ArrowReturn();
	}
	public override void UpdateState()
	{

		if (Funcs.IsAnimationAlmostFinish(me.animCtrl, animStr))
		{
			//Combat������Ʈ�� �Ǻ��ؼ� ������ ���鼭 �׳� ���� ���� Ʋ���ָ� �ǰ�
			//�ƴϸ�?
			//���鼭 equip���� �����ָ� �ȴ�~
			if (archer.combatState == eCombatState.Combat | archer.combatState == eCombatState.Alert)
			{
				archer.SetState((int)archer.actTable.RandomAttackState());
			}
			else if(archer.combatState == eCombatState.Idle | archer.weaponEquipState == eEquipState.UnEquip)
			{
				archer.SetState((int)eArcherState.Bow_Equip);
			}
		}

	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
		
	}

	public override void ExitState()
	{

	}

}
