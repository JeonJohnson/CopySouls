using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Archer : Enemy
{
	public bool isEquip;



	public override void InitializeState()
	{
		fsm = new cState[(int)eArcherState.End];

		fsm[(int)eArcherState.Idle] = new Archer_Idle();
		
		fsm[(int)eArcherState.Bow_Equip] = new Archer_BowEquip();
		fsm[(int)eArcherState.Bow_Unequip] = new Archer_BowUnequip();
		
		fsm[(int)eArcherState.Walk_Patrol] = new Archer_Walk_Patrol();
		fsm[(int)eArcherState.Walk_Careful] = new Archer_Walk_Careful();
		fsm[(int)eArcherState.Walk_Aiming] = new Archer_Walk_Aiming();
		
		fsm[(int)eArcherState.Runaway] = new Archer_Runaway();

		fsm[(int)eArcherState.Attack_Rapid] = new Archer_Attack_Rapid();
		fsm[(int)eArcherState.Attack_Aiming] = new Archer_Attack_Aiming();
		fsm[(int)eArcherState.Attack_Melee] = new Archer_Attack_Melee();

		fsm[(int)eArcherState.Hit] = new Archer_Hit();

		fsm[(int)eArcherState.Death] = new Archer_Death();


		SetState((int)eArcherState.Idle);
	}

	//public override void InitializeState()
	//{
	//	fsm = new cState[(int)eArcherState.End];
	//	//Idle - Ȱ ���� / ���� ������ �� �ΰ���
	//	//Bow_Equip - ���̵鿡�� �����¼� ���۵Ǹ�
	//	//Bow_Unequip - �������¿��� ���ư���
	//	//Walk_Patrol - ������ ���ƴٴϴ°�
	//	//Walk_Careful - ���������� �ϰ� ������ �׳� �����°�(��ġ �̵�)
	//	//Walk_Aiming - �����ϸ鼭 �����°�
	//	//Run - �Ÿ��������� �����ϴ°�
	//	//Attack_Rapid - ������ ���� ���
	//	//Attack_Aiming - �ѹ� ������ ���
	//	//Attack_Melee - ������ ������
	//	//Hit - �ĸ´°�
	//	//Death - �״°�
	//}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		//���� ���� 0�� 48 ~ 9�� 57
		
		//Idle ~ MeleeAttack ����
		for (int i = 48; i < (int)KeyCode.Alpha9; ++i)
		{
			if (Input.GetKeyDown((KeyCode)i))
			{
				SetState(i - 48);
			}
		}


	}
}
