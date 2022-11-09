using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Enums;
using Structs;


public enum eMoveDirection
{ 
	Right,
	Left,
}

public class Archer : Enemy
{
    //weapon수정 from용석to근희
    public WoodenShortBow weapon;

	//public Test.eStateTest testEnum = new Test.eStateTest();
	//int[] iTestArr = 
	//{ 
	//	(int)Enums.eArcherState.Idle,
	//	(int)Enums.eArcherState.Bow_Equip,
	//	(int)Enums.eArcherState.Bow_Unequip,
	//	(int)Enums.eArcherState.Walk_Patrol,
	//	(int)Enums.eArcherState.Runaway,
	//	(int)Enums.eArcherState.Attack_Aiming,
	//	(int)Enums.eArcherState.Hit,
	//	(int)Enums.eArcherState.Death,
	//};

	[Header("Fov System")]
	public FovStruct fovStruct;
	public LayerMask fovIgnoreLayer;
	public LayerMask fovCheckLayer;

	[Header("About Target")]
	public Transform targetHeadTr;
	public Transform targetSpineTr;
	public Vector3 fovDir; //head to TargetSpine
	public Vector3 atkDir; //arrowHead to TargetSpine or TargetHead
	

	[Header("Me Bones")]
	public Transform headBoneTr;
	public Transform spineBoneTr;
	public Transform rightIndexFingerBoneTr;

	public float curSpd;
	public float meleeAtkRange;
	public float runRange;


	public eArcherState defaultPattern;
	public eArcherState curState_e;


	////public delegate void Al
	//public delegate void AlertEventHandler();
	//public AlertEventHandler alertStartEvent;
	//public AlertEventHandler alertEndEvent;
	//public delegate void CombatEventHandler();
	//public CombatEventHandler combatStartEvent;
	//public CombatEventHandler combatEndEvent;
	//public delegate void AttackEventHandler();
	//public AttackEventHandler DrawArrowEvent;
	//public AttackEventHandler HookArrowEvent;
	//public AttackEventHandler StartStringPullEvent;
	//public AttackEventHandler EndStringPullEvent;
	//public AttackEventHandler ShootArrowEvent;


	public void TempSettingPlayer()
	{
		if (targetObj == null)
		{
			GameObject tempObj = GameObject.FindGameObjectWithTag("Player");

			if (tempObj != null)
			{
				targetObj = tempObj;
				Player script = tempObj.GetComponent<Player>();
				if (script != null)
				{
					targetHeadTr = script.headTr;
					targetSpineTr = script.spine3Tr;
				}
			}
		}
	}

	public void CalcFovDir(float degreeAngle)
	{
		//22 10 02 fin, 설명해주기

		//시야각도를 이용해서 ㄹㅇ 시야각 구하기

		//f=Forward
		// A   f    B
		// \   |   /
		//  \  |  /
		//   \ | /
		//-----0-------

		//forward와 A사이의 각도 @1
		//=> @1 = Dot(f,A) * aCos;

		//forword와 B 사이의 각도 @2
		//=> @2 = Dot(f,B) * aCos;

		//판별기준은 몬스터와 0의 각도를 구한다음
		//그게 fov/2 보다 작으면 시야각 내에 있는거.

	
		fovDir = (targetSpineTr.position - headBoneTr.position).normalized;
		fovStruct.LookDir = headBoneTr.forward;
		fovStruct.LeftDir = Funcs.DegreeAngle2Dir(transform.eulerAngles.y - (status.fovAngle * 0.5f));
		fovStruct.RightDir = Funcs.DegreeAngle2Dir(transform.eulerAngles.y + (status.fovAngle * 0.5f));
	}


	public bool CheckTargetInFov()
	{
		//시야각안에 플레이어 들어왔는지 아닌지 만 판단.

		Collider[] hitObjs = Physics.OverlapSphere(transform.position, status.ricognitionRange);

		if (hitObjs.Length == 0)
		{
			return false;
		}

		foreach (Collider col in hitObjs)
		{
			if (col.transform.root.gameObject != targetObj)
			{
				continue;
			}

			//float angleToTarget = Mathf.Acos(Vector3.Dot(transform.forward, dirToTarget)) * Mathf.Rad2Deg;
			//float angleToTarget = Mathf.Acos(Vector3.Dot(targetHeadTr.forward, fovDir)) * Mathf.Rad2Deg;
			float angleToTarget = Mathf.Acos(Vector3.Dot(transform.forward, fovDir)) * Mathf.Rad2Deg;
			//왜 head의 forward랑은 안돼는거냐?
			//Debug.Log(angleToTarget);

			//내적해주고 나온 라디안 각도를 역코사인걸어주고 오일러각도로 변환.
			//if (angleToTarget <= (status.fovAngle * 0.5f) //타겟이 시야각 안에 있고
			//	&& !Physics.Raycast(transform.position, fovDir, status.ricognitionRange, fovIgnoreLayer))
			if (angleToTarget <= (status.fovAngle * 0.5f) //타겟이 시야각 안에 있고
				&& !Physics.Raycast(headBoneTr.position, fovDir, status.ricognitionRange, fovIgnoreLayer))
			//Environment이거나 Enemy인 애만 인식을 하는 Ray에 잡히지 않을 때!
			//=> 즉 시야각 안에있는 오브젝트가 Environment || Enemy가 아닐 때
			{
				Debug.Log($"{angleToTarget}도로 시야각 안에 들어옴");
				return true;
			}
		}

		return false;
	}

	public bool CheckTargetIsHiding(GameObject tempTarget)
	{
		Vector3 dir = (tempTarget.transform.position - transform.position).normalized;
		float angleToTarget = Mathf.Acos(Vector3.Dot(fovStruct.LookDir, dir)) * Mathf.Rad2Deg;

		if (angleToTarget <= (status.fovAngle * 0.5f)) //시야각 안에 있는 경우
		{
			RaycastHit hitEnvironmentInfo;

			if (Physics.Raycast(transform.position, dir, LayerMask.GetMask("Player")))
			{
				int temp = LayerMask.GetMask("Environment");
				if (Physics.Raycast(transform.position, dir, out hitEnvironmentInfo, float.MaxValue, temp))
				{
					float dist = Vector3.Distance(hitEnvironmentInfo.point, transform.position);

					if (distToTarget > dist)
					{
						//같은 dir 쏴서 지형이 가까이 있으면, 플레이어는 가려진거겠지
						return false;
					}
					return true;
				}
				else
				{
					return true;
				}
			}
		}
		return false;
	}




	public override void InitializeState()
	{
		fsm = new cState[(int)eArcherState.End];

		fsm[(int)eArcherState.Idle] = new Archer_Idle();
		
		fsm[(int)eArcherState.Bow_Equip] = new Archer_BowEquip();
		fsm[(int)eArcherState.Bow_Unequip] = new Archer_BowUnequip();
		
		fsm[(int)eArcherState.Walk_Patrol] = new Archer_Walk_Patrol();
		fsm[(int)eArcherState.Walk_Careful] = new Archer_Walk_Careful();
		fsm[(int)eArcherState.Walk_Aiming] = new Archer_Walk_Aiming();

		fsm[(int)eArcherState.LookAround] = new Archer_LookAround();

		fsm[(int)eArcherState.Runaway] = new Archer_Runaway();

		fsm[(int)eArcherState.Attack_Rapid] = new Archer_Attack_Rapid();
		fsm[(int)eArcherState.Attack_Aiming] = new Archer_Attack_Aiming(this);
		fsm[(int)eArcherState.Attack_Melee] = new Archer_Attack_Melee();

		fsm[(int)eArcherState.Hit] = new Archer_Hit();

		fsm[(int)eArcherState.Death] = new Archer_Death();


		SetState((int)defaultPattern);
	}

	public void SettingBonesTransform()
	{
		headBoneTr = animCtrl.GetBoneTransform(HumanBodyBones.Head);
		spineBoneTr = animCtrl.GetBoneTransform(HumanBodyBones.Spine);
		rightIndexFingerBoneTr = animCtrl.GetBoneTransform(HumanBodyBones.RightIndexDistal);
	}

	public void Initializebow()
	{

		if (weapon != null)
		{
			weapon.gameObject.SetActive(false);
		}
		//if (bow == null)
		//{ 
		//	//bow = Instantiate(bowPrefab,)
		//}

		//bow.rightIndexFingerTr = rightIndexFingerBoneTr;

		//HookArrowEvent += bow.HookArrow;
		//StartStringPullEvent += bow.StartStringPull;
		//ShootArrowEvent += bow.ShootArrow;
	}

	//public void EquippedBow()
	//{
	//	SetState((int)eArcherState.Bow_Equip);
	//}

	//public void UnequippedBow()
	//{
	//	SetState((int)eArcherState.Bow_Unequip);
	//}

	public void RandomAttack()
	{
		int atkRandom = UnityEngine.Random.Range(0, 1);

		if (atkRandom == 0)
		{
			SetState((int)Enums.eArcherState.Attack_Aiming);
		}
		else
		{
			SetState((int)Enums.eArcherState.Attack_Rapid);
		}

	}

    public int ActingLegWhileTurn(Vector3 pos)
    {
		//코루틴보다 일단 걍 함수로 필요할때 호출하기
		
		Vector3 tempDir = (pos - transform.position).normalized;
		tempDir.y = 0;
		//이렇게 되면 오르막, 내리막 길에는 어떻게 되지...? 

		float offsetAngle = Vector3.Angle(transform.forward, tempDir);
		//Vector3.Angle은 절대값을 반환함.
		//원시적으로 외적, 내적으로 좌,우 판단해야할듯

		//1. 포워드와 direction 외적해서 법선 구하기
		Vector3 DirCrossFoward = Vector3.Cross(tempDir,transform.forward);

		//2. 나온 법선이랑 Up벡터 내적하기
		float dot = Vector3.Dot(DirCrossFoward, Vector3.up);

		//3. 나온값은 Cos@ 값임. 이게 음수면 오른쪽, 양수면 왼쪽
		// cos0 = 1 / cos90 = 0 / cos180 = -1
		//왜 그렇냐?
		//Cross는 결과값이 벡터(방향과 크기)로 나오는데
		//이것이 비교값(현재는 월드UP)과 같은 방향이면 (cos 0 = 1
		//다른 방향이면 (cos 180 = -1
		//임.
		//근데 Cross(외적)은 내적과 다르게 교환법칙이 성립안하는,
		//즉 순서에 따라 결과값이 다르므로! 이렇게 구별이 가능하다
		//왼쪽이면 90도 미만이니까(같은 up방향) 양수,
		//오른쪽이면 90도 초과이니까 음수.

		if (dot > 0.1f)
		{//오른쪽
			Debug.Log("Right");
			animCtrl.SetBool("bTurn_R", true);
			animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, offsetAngle);
			return 1;
		}
		else if(dot < -0.1f)
		{//왼쪽
			Debug.Log("Left");
			animCtrl.SetBool("bTurn_R", false);
			animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, offsetAngle);
			return -1;
		}
		else
		{ //가운데
			Debug.Log("Middle");
			animCtrl.SetLayerWeight((int)eHumanoidAvatarMask.Leg, offsetAngle);
			return 0;
		}

		//        Debug.Log(offsetAngle);

	

		//221009 이거분명 나중에 길 경사 깔리면 문제생김
		//ㅋㅋ 2차원으로만 생각하고 짠거라서 ㅋㅋ
		//그때 일단 tempDir의 up을 몬스터의 up과 같이 만들고
		//왼쪽,오른쪽 판별할때도 World up 말고 local up 써보던지
		//일단 몰라~ㅋㅋ 문제생길때 고쳐~

	}



    public eArcherState Think(eArcherState curState)
    {

		eArcherState returnState = eArcherState.End;

        switch (curState)
        {
   //         case eArcherState.Attack_Rapid:
   //         case eArcherState.Attack_Aiming:
   //             {
			//		if (distToTarget >= status.ricognitionRange
			//			|| !CheckTargetInFovAndRange())
			//		{//인지범위 밖으로 나갔거나 시야각 밖으로 나갔을 경우
			//			returnState = eArcherState.LookAround;
			//		}
			//		else
			//		{
			//			returnState = eArcherState.Attack_Aiming;
			//		}
   //             }
			//	break;
   //         case eArcherState.Attack_Melee:
   //             break;

			//case eArcherState.Hit:
			//	{
			//		if (distToTarget <= meleeAtkRange)
			//		{
			//			float random = UnityEngine.Random.Range(0f, 100f);
			//			//한 30프로 확률로다가 근접공격
			//			//30프로는 이전 상태
			//			//40프로는 도망가기 ㅌㅌㅌ

			//			if (random <= 30f)
			//			{
			//				//returnState = eArcherState.Attack_Melee; 
			//				returnState = eArcherState.Runaway;
			//			}
			//			else if (random > 30f && random <= 60f)
			//			{
			//				returnState = (eArcherState)preState_i;
			//			}
			//			else
			//			{
			//				returnState = eArcherState.Runaway;
			//			}
			//		}
			//		else
			//		{
			//			float random = UnityEngine.Random.Range(0, 2);

			//			if (random == 0)
			//			{
			//				returnState = (eArcherState)preState_i;
			//			}
			//			else
			//			{
			//				returnState = eArcherState.Runaway;
			//			}
			//		}
			//	}
			//	break;
   //         default:
   //             break;
        }

		return returnState;
    }

    #region Events

    public void DrawArrow()
	{
		//arrow = ObjectPoolingCenter.Instance.LentalObj("Arrow_Dynamic").GetComponent<Arrow>();

		//arrow.archer = this;
		//arrow.rightIndexFingerBoneTr = rightIndexFingerBoneTr;
		//arrow.target = targetObj;
		//arrow.bowLeverTr = bow.bowLeverTr;


		//bow.arrow = arrow;

		//HookArrowEvent += arrow.Hooking;
		//ShootArrowEvent += arrow.Shoot;
	}

	public void HookArrow()
	{
		//if (HookArrowEvent != null)
		//{
		//	HookArrowEvent();
		//}
	}

	public void StartStringPull()
	{
		//if (StartStringPullEvent != null)
		//{
		//	StartStringPullEvent();
		//}
	}

	public void EndStringPull()
	{
		//if (EndStringPullEvent != null)
		//{
		//	EndStringPullEvent();
		//}
	}

    public void ShootArrow()
	{
		//if (ShootArrowEvent != null)
		//{
		//	ShootArrowEvent();
		//}

		//arrow = null;
		////StartCoroutine(arrow.GetComponent<Arrow>().AliveCouroutine());
	}


	#endregion

	public override void Hit(DamagedStruct dmgStruct)
    {
        base.Hit(dmgStruct);

		if (status.curHp <= 0f)
		{
			SetState((int)eArcherState.Death); 
		}
		else
		{
			if (!status.isSuperArmor)
			{
				SetState((int)eArcherState.Hit);
			}
		}
	}

	//public override void Death()
	//{

	//}

	protected override void Awake()
	{
		base.Awake();
		
	}

	protected override void Start()
	{
		base.Start();
		
		SettingBonesTransform();
		Initializebow();

		TempSettingPlayer();
		//weapon.SetActive(false);
	}

	protected override void Update()
	{
		base.Update();
        CalcFovDir(status.fovAngle);

		//for (int i = (int)KeyCode.Alpha0; i < iTestArr.Length+48; ++i)
		//{
		//	if (Input.GetKeyDown((KeyCode)i))
		//	{
		//		int iTest = i - 48;
		//		SetState(iTestArr[iTest]);
		//	}
		//}


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

	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		
		if (headBoneTr != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(headBoneTr.position, fovStruct.LookDir * status.ricognitionRange);
			Gizmos.DrawRay(headBoneTr.position, fovStruct.LeftDir * status.ricognitionRange);
			Gizmos.DrawRay(headBoneTr.position, fovStruct.RightDir * status.ricognitionRange);

			Gizmos.color = Color.red;
			Gizmos.DrawRay(headBoneTr.position, fovDir * status.ricognitionRange);
		}

		//if (headBoneTr != null)
		//{ Gizmos.DrawFrustum(headBoneTr.position, status.fovAngle, status.ricognitionRange, 0.1f, 1f); }




	}

}
