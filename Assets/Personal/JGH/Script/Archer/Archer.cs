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
	//	//Idle - 활 장착 / 장착 안했을 때 두가지
	//	//Bow_Equip - 아이들에서 전투태세 시작되면
	//	//Bow_Unequip - 전투상태에서 돌아가면
	//	//Walk_Patrol - 전투전 돌아다니는거
	//	//Walk_Careful - 무기장착은 하고 있지만 그냥 간보는거(위치 이동)
	//	//Walk_Aiming - 조준하면서 간보는거
	//	//Run - 거리벌릴려고 ㅌㅌ하는거
	//	//Attack_Rapid - 여러발 빨리 쏘기
	//	//Attack_Aiming - 한발 신중히 쏘기
	//	//Attack_Melee - 가까이 왔을때
	//	//Hit - 쳐맞는거
	//	//Death - 죽는거
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
