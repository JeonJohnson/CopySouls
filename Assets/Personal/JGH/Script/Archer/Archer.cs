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

	public GameObject arrowPrefab;
	
	public Arrow arrow;
	public eArcherState curState_e;



	Transform testTr;

	//public List<GameObject> patrolPos
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

	//public void EquipWeapon()
	//{
	//	if (isCombat)
	//	{
	//		SetState((int)eArcherState.Bow_Equip);
	//	}
	//	else
	//	{
	//		SetState((int)eArcherState.Bow_Unequip);
	//	}
	//}

	public void EquippedBow()
	{
		SetState((int)eArcherState.Bow_Equip);
	}

	public void UnequippedBow()
	{
		SetState((int)eArcherState.Bow_Unequip);
	}

    public void MoveLegWhileTurn()
    {
		Vector3 tempDir = (targetObj.transform.position - transform.position).normalized;
		tempDir.y = 0;
		float offsetAngle = Vector3.Angle(transform.forward, tempDir);
        Debug.Log(offsetAngle);
        if (offsetAngle > 5f)
        {
            Debug.Log("돌아가야함");
            //animCtrl.GetCurrentAnimatorStateInfo(1)
            animCtrl.SetLayerWeight(1, 1);
        }
        else
        {
            animCtrl.SetLayerWeight(1, offsetAngle);
        }
    }


	public void DrawArrow()
	{
		arrow = ObjectPoolingCenter.Instance.LentalObj(ePoolingObj.Arrow).GetComponent<Arrow>();
		arrow.hand = rightHand;
		arrow.master = gameObject;
		arrow.target = targetObj;
		//arrow.transform.position = bowString.transform.position;
	}
    public void ShotArrow()
	{


		StartCoroutine(arrow.GetComponent<Arrow>().AliveCouroutine());

	}

	//public bool TargetCheck()
	//{
	//	if (distToTarget <= status.ricognitionRange)
	//	{
	//		return true;
	//	}
	//	return false;
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

		testTr = animCtrl.GetBoneTransform(HumanBodyBones.Spine);

		bowStringOriginPos = new Vector3(0f, -0.01f, 0f);
		weapon.SetActive(false);

		alertStartEvent += EquippedBow;
		alertEndEvent += UnequippedBow;
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



		curState_e = (eArcherState)curState_i;
	}

    protected override void LateUpdate()
    {
        base.LateUpdate();

	//ChestDir = mainCameraTr.position + mainCameraTr.forward * 50f;
	//playerChestTr.LookAt(ChestDir); //상체를 카메라 보는방향으로 보기
	//playerChestTr.rotation = playerChestTr.rotation * Quaternion.Euler(ChestOffset);

	//https://dallcom-forever2620.tistory.com/6 -> 애니메이션 적용중일때 본 가져오는거? 
	}
}
