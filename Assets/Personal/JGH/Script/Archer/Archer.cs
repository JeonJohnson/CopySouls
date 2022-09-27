using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Structs;

namespace Test
{
	public enum eStateTest
	{ 
		Idle = 48,
		Equip,
		Unequip,
		Walk_Patrol,
		Run = 54,
		Attack_Aiming,
		Hit = 58,
		Death = 59,
		End
	}

}

public class Archer : Enemy
{
	//public Test.eStateTest testEnum = new Test.eStateTest();
	int[] iTestArr = 
	{ 
		(int)Enums.eArcherState.Idle,
		(int)Enums.eArcherState.Bow_Equip,
		(int)Enums.eArcherState.Bow_Unequip,
		(int)Enums.eArcherState.Walk_Patrol,
		(int)Enums.eArcherState.Runaway,
		(int)Enums.eArcherState.Attack_Aiming,
		(int)Enums.eArcherState.Hit,
		(int)Enums.eArcherState.Death,

	};
	
	public bool isEquip;

	public GameObject rightHand;
	public GameObject bowString;
	public Vector3 bowStringOriginPos;

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

    public override void Hit(DamagedStruct dmgStruct)
    {
        base.Hit(dmgStruct);
    }

    //public override void Death()
    //{

    //}


    protected override void Awake()
	{
		base.Awake();


		bowStringOriginPos = new Vector3(0f, -0.01f, 0f);
		weapon.SetActive(false);
	}

	protected override void Start()
	{
		base.Start();

	}

	protected override void Update()
	{
		base.Update();

		for (int i = (int)KeyCode.Alpha0; i < iTestArr.Length+48; ++i)
		{
			if (Input.GetKeyDown((KeyCode)i))
			{
				int iTest = i - 48;
				SetState(iTestArr[iTest]);
			}
		}
	}
}
