using Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
	[Header("Golem's")]
	public GameObject meshObj;
	public GameObject rootObj;
	public Transform headBoneTr;
	public Golem_Frag fragScript;
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

	}


	protected override void Awake()
	{
		base.Awake();
		actTable = GetComponent<Golem_ActionTable>();
		
	}

	protected override void Start()
	{
		base.Start();
		
		meshObj.SetActive(false);
		rootObj.SetActive(false);
		ragdoll.gameObject.SetActive(true);
	}

	protected override void Update()
	{
		base.Update();

		status.curStamina += Time.deltaTime;
		status.curStamina = Mathf.Clamp(status.curStamina, 0f, status.maxStamina);

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
