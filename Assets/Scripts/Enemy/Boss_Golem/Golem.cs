using Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
	[Header("Target")]
	public Transform targetHeadTr;

	[Header("Golem's")]
	public GameObject meshObj;
	public GameObject rootObj;
	public Transform headBoneTr;
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
	}

	public override void InitializeState()
	{
		fsm = new cState[(int)eGolemState.End];

		fsm[(int)eGolemState.Think] = new Golem_Think();

		fsm[(int)eGolemState.Entrance] = new Golem_Entrance();
		
		fsm[(int)eGolemState.MeleeAtk_1Hit] = new Golem_MeleeAtk_1Hit();
		fsm[(int)eGolemState.MeleeAtk_2Hit] = new Golem_MeleeAtk_2Hit();
		fsm[(int)eGolemState.MeleeAtk_3Hit] = new Golem_MeleeAtk_3Hit();

		fsm[(int)eGolemState.TurnAtk] = new Golem_TurnAtk();
		
		fsm[(int)eGolemState.ForwardAtk_1Hit] = new Golem_ForwardAtk_1Hit();
		fsm[(int)eGolemState.ForwardAtk_2Hit] = new Golem_ForwardAtk_2Hit();
		fsm[(int)eGolemState.ForwardAtk_3Hit] = new Golem_ForwardAtk_3Hit();

		fsm[(int)eGolemState.Hit] = new Golem_Hit();
		fsm[(int)eGolemState.Death] = new Golem_Death();

		SetState((int)eGolemState.Entrance);
	}


	protected override void Awake()
	{
		base.Awake();
		
		actTable = GetComponent<Golem_ActionTable>();

		meshObj.SetActive(false);
		rootObj.SetActive(false);
		ragdoll.gameObject.SetActive(true);

		initPos = transform.position;
		initForward = transform.forward;
	}

	protected override void Start()
	{
		base.Start();

		SearchMyBone();
		SearchTarget();
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

	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
	}
}
