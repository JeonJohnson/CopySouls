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
	}
}
