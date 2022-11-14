using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Chase : cState
{
	Archer archer = null;

	float randomDist;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);
		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		Debug.Log("Enter Chase State");

		archer.combatState = eCombatState.Alert;

		//���� �帧 
		//���� ������ ����, CheckTargetIsHiding���� ���� ������,
		//���⼭�� �ϴ� �� ���������� ���̴� ��ġ ���� �ɾ�� 
		//or �߰��� ���� �ݶ��̴� ������ ���ư���
		// �׷��� �Ⱥ��̸� �θ��� �Ÿ��ٰ� 
		archer.lastTargetPos = archer.targetObj.transform.position;
		
		archer.lastTargetSpinePos = archer.targetSpineTr.position;
		archer.lastTargetHeadPos = archer.targetHeadTr.position;

		float maxdistance = Vector3.Distance(archer.transform.position, archer.lastTargetPos);
		randomDist = Random.Range(maxdistance*0.1f, maxdistance);
		Debug.Log($"LookAround Distance : {randomDist}");

		//�������̸� Ǯ����ϴµ� ���� �������̸� �� ��� �Ѿ��.

		//archer.animCtrl.SetInteger("iAttackState", 4);
		archer.animCtrl.SetTrigger("tIdle");
		archer.animCtrl.SetBool("bEquip",true);
		archer.curState_e = Enums.eArcherState.Chase;
	}

	public override void UpdateState()
	{
		//if (!archer.CheckTargetIsHiding())
		//{
		//	Debug.Log("�þ߿� ����");
		//	archer.SetState((int)archer.actTable.RandomAttackState());
		//	return;
		//}

		if (archer.CheckTargetInFov())
		{
			Debug.Log("Chase���� �þ߿� ����");
			archer.SetState((int)archer.actTable.RandomAttackState());
			return;
		}
		else
		{
			float dist = Vector3.Distance(archer.transform.position, archer.lastTargetPos);
			//Debug.Log($"chase Dist : {dist}");

			if (dist <= 5f)
			{
				archer.actTable.MoveWhileAttack(eArcherMoveDir.End);
				
				archer.SetState((int)Enums.eArcherState.LookAround);
			}
			else
			{
				archer.actTable.MoveForward(archer.lastTargetPos);
			}
		}

	}
	public override void LateUpdateState()
	{
		base.LateUpdateState();

		archer.LookAtSpecificBone(archer.spineBoneTr, archer.lastTargetSpinePos, Enums.eGizmoDirection.Back, new Vector3(0f, 0f, -90f));
		archer.LookAtSpecificBone(archer.headBoneTr, archer.lastTargetHeadPos, Enums.eGizmoDirection.Foward, new Vector3(0f,0f,0f));
	}
	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void ExitState()
	{

	}
}
