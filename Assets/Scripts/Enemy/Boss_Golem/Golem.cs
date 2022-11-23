using Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{

	public AnimationClip testClip;
	[Header("Target")]
	public Transform targetHeadTr;
	public float angleToTarget;
	public eSideDirection targetWhichSide;

	[Header("Golem's")]
	public GameObject meshObj;
	public GameObject rootObj;
	public Transform headBoneTr;
	public Transform rightHandBoneTr;
	public Transform leftHandBoneTr;
	private Golem_Frag fragScript;
	public Golem_Frag FragScript
	{
		get
		{
			if (!fragScript)
			{
				if (ragdoll)
				{
					fragScript = ragdoll as Golem_Frag;
				}
			}
			return fragScript;
		}
	}
	/// 골램 조각은 ragdoll을 (Golem_Frag로 캐스팅해서 사용하기)
	//as와 is 캐스팅 차이 
	//as는 실패시 null 반환
	//is 는 실패시 false 반환.
	//enemy_ragdoll -> Golem_Frag 다운 캐스팅이라서 확인하고 쓰자!

	public Golem_ActionTable actTable;

	public HFSMCtrl hfsmCtrl;

	[Header("Status Vars")]
	public float rangeAtkRange;
	public float decisionTime;

	


	public void SearchTarget()
	{
		if (!targetObj)
		{
			targetObj = UnitManager.Instance.GetPlayerObj;
			targetScript = UnitManager.Instance.GetPlayerScript;
			targetHeadTr = targetScript.headTr;
		}
	}

	public void SearchMyBone()
	{
		headBoneTr = animCtrl.GetBoneTransform(HumanBodyBones.Head);
		rightHandBoneTr = animCtrl.GetBoneTransform(HumanBodyBones.RightHand);
		leftHandBoneTr = animCtrl.GetBoneTransform(HumanBodyBones.LeftHand);
	}

	public override void DeathReset()
	{
		base.DeathReset();


	}
	public override void ResetEnemy()
	{

	}

	public override void Hit(DamagedStruct dmgStruct)
	{
		base.Hit(dmgStruct);

		if (status.curHp > 0)
		{
			int rand = Random.Range(0, 100);
			if (rand < 30)
			{
				SetState((int)eGolemState.Hit);
			}
		}
		else 
		{
			SetState((int)eGolemState.Death);
		}
	}

	public override void InitializeState()
	{
		//fsm = new cState[(int)eGolemState.End];

		//fsm[(int)eGolemState.Think] = new Golem_Think(0);

		//fsm[(int)eGolemState.Idle] = new Golem_Idle(0);
		//fsm[(int)eGolemState.Move] = new Golem_Move(0);
		//fsm[(int)eGolemState.Turn] = new Golem_Turn(0);

		//fsm[(int)eGolemState.Entrance] = new Golem_Entrance(0);
		
		//fsm[(int)eGolemState.MeleeAtk_1Hit] = new Golem_MeleeAtk_1Hit(1);
		//fsm[(int)eGolemState.MeleeAtk_2Hit] = new Golem_MeleeAtk_2Hit(2);
		//fsm[(int)eGolemState.MeleeAtk_3Hit] = new Golem_MeleeAtk_3Hit(3);

		////fsm[(int)eGolemState.Turn] = new Golem_Turn(0);
		////fsm[(int)eGolemState.TurnAtk] = new Golem_TurnAtk(1);
		
		//fsm[(int)eGolemState.ForwardAtk_1Hit] = new Golem_ForwardAtk_1Hit(3);
		//fsm[(int)eGolemState.ForwardAtk_2Hit] = new Golem_ForwardAtk_2Hit(4);
		//fsm[(int)eGolemState.ForwardAtk_3Hit] = new Golem_ForwardAtk_3Hit(5);

		//fsm[(int)eGolemState.ThrowRock] = new Golem_ThrowRock(5);
		////fsm[(int)eGolemState.JumpAtk] = new Golem_JumpAtk(6);


		//fsm[(int)eGolemState.Hit] = new Golem_Hit(0);
		//fsm[(int)eGolemState.Death] = new Golem_Death(0);

		//SetState((int)eGolemState.Entrance);
	}

	private void SetHFSMCtrl()
	{
		if (!hfsmCtrl) return;
		hfsmCtrl.golem = this;
		hfsmCtrl.table = actTable;
	}

	protected override void Awake()
	{
		base.Awake();
		
		actTable = GetComponent<Golem_ActionTable>();
		hfsmCtrl = GetComponent<HFSMCtrl>();
		//SetHFSMCtrl();

		meshObj.SetActive(false);
		rootObj.SetActive(false);
		ragdoll.gameObject.SetActive(true);

		initPos = transform.position;
		initForward = transform.forward;

		navAgent.stoppingDistance = status.atkRange;
	}

	protected override void Start()
	{
		base.Start();

		SearchMyBone();
		SearchTarget();

		decisionTime = Random.Range(1f, 2f);
	}

	protected override void Update()
	{
		base.Update();

	
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
	}
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
	}

	public void OnDrawGizmos()
	{

		////공격 사정거리
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, status.atkRange);
		////공격 사정거리

		////공격 사정거리
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, rangeAtkRange);
		////공격 사정거리
	}
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();



		//////공격 사정거리
		//Gizmos.color = Color.green;
		//Gizmos.DrawWireSphere(transform.position, throwAtkRange);
		//////공격 사정거리

		//////공격 사정거리
		//Gizmos.color = Color.blue;
		//Gizmos.DrawWireSphere(transform.position, jumpAtkRange);
		//////공격 사정거리


	}


}
