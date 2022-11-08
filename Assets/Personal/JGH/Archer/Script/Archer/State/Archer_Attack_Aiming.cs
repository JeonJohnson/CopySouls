using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class Archer_Attack_Aiming : cState
{
	public Archer_Attack_Aiming(Enemy script) : base(script)
	{ //c# 상속에서의 생성자 
	  //=> :base() 키워드로 재정의 해줘야 자식의 생성자가 호출됨.
	  //무~~조건 부모 생성자부터 호출되고나서 자식 생성자 호출됨
		if (archer == null)
		{
			archer = me.GetComponent<Archer>();

			//if (archer.StartStringPullEvent == null)
			//{//이거 나중에 cState 생성자 만들거나 Initialize에서 쓸 수 있도록
			//	archer.StartStringPullEvent += PullStart;
			//	archer.EndStringPullEvent += PullEnd;
			//}
		}

		//aimingCoroutine = AimingCoroutine();
		//aimingCoroutine.
	}

	Archer archer = null;

	//여기서도
	//1. 정해진 시간 지나면 쏘는 패턴
	//2. 타겟의 특정 행동 할 때 까지 쏘는패턴?
	//3. 1+2 패턴으로 정해진 시간 동안 쏠예정인데 중간에 특정 행동하면 쏘는거

	//또 하나 더 
	//1. 쏘기 전까지 시간을 정하고 비례해서 드로우 시간 정하기?
	//2. 드로우시간은 무적권 정해져있고 에이밍 시간을 랜덤?

	// aiming 패턴에서 드로우 시간은 고정으로 하고 에이밍 시간을 랜덤으로 가자

	public float curAimingTime = 0f;
	public float maxAimingTime;

	public float stringPullTime;
	public float archerPullAnimSpd;
	public float bowPullAnimSpd;

	public IEnumerator aimingCoroutine = null;


	//public int delayFrame = 0;
	//public bool isHook = false;

	public void PullStart()
	{
		archer.animCtrl.SetFloat("fDrawSpd", archerPullAnimSpd);
	}

	public void PullEnd()
	{
		CoroutineHelper.Instance.StartCoroutine(AimingCoroutine());
	}

	//public void PullEnd()

	public IEnumerator AimingCoroutine()
	{
		//굳이 코루틴으로 할 필요가? 중간에 화살 발사하는 다른 조건도 생길 수 도 있는디
		//	중간에 플레이어가 갑작스럽게 가까이 오거나,
		//뭐 다른 조건들도 생길 수 있음.	
		aimingCoroutine = AimingCoroutine();

		while (curAimingTime >= maxAimingTime)
		{
			curAimingTime += Time.deltaTime;

			//여기 다른 조건 적으면 되지 ㅎㅎ;

			yield return null;
		}

		curAimingTime = 0f;
		archer.animCtrl.SetTrigger("tShootArrow");
		aimingCoroutine = null;
	}

	public void CalcDrawSpd()
	{
		stringPullTime = 4f;

		//시위 당기는건 Draw 애니메이션 20~30프레임 (10프레임, 0.3초)
		//+ Aiming 애니메이션 0~111프레임 (111프레임, 3.7초)
		//약 120프레임, 4초

		//1초만에 할려면 spd 4/1;
		//2초만에 할려면 spd 4/2;
		//3초만에 할려면 spd 4/3;
		//4초 만에 할려면 spd 4/4;

		archerPullAnimSpd = 4f / stringPullTime;

		//Bow 시위 당겨지는 애니메이션 5.5프레임부터 13프레임 (0.25초)
		//0.25초 동안 재생 하려면 spd 1
		//0.5초 동안 하려면 spd 1/2
		//1초 동안 하려면 spd 1/4
		//2초 동안 하려면 spd 1/8
		//4초 동안 하려면 spd 1/16	

		bowPullAnimSpd = 1f / (stringPullTime * 4f);

		
		//archer.bow.pullAnimSpd = bowPullAnimSpd;
			
		//archerPullAnimSpd = 1f / stringPullTime;
		//bowPullAnimSpd = 0.25f / (drawTime*2f);

		//archer.animCtrl.SetFloat("fDrawSpd", archerDrawAnimSpd);
		//archer.bow.drawAnimSpd = bowDrawAnimSpd;
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		//me.isCombat = true;

		CalcDrawSpd();

		maxAimingTime = Random.Range(0.5f, 2f);
		
		me.animCtrl.SetTrigger("tAttack");

		//delayFrame = 0;
	}

	public override void UpdateState()
	{
		//me.transform.rotation = me.LookAtSlow(me.transform, me.targetObj.transform, me.status.lookAtSpd * 2);
  //      archer.ActingLegWhileTurn(me.targetObj.transform.position);

		//if (!archer.CheckTargetIsHidingInFov(me.targetObj))
		//{
		//	me.SetState((int)eArcherState.LookAround);
		//}
		//else
		//{
		//	if (Funcs.IsAnimationCompletelyFinish(me.animCtrl, "Archer_Atk_Shoot"))
		//	{
		//		eArcherState nextState = archer.Think(archer.curState_e);
		//		if (nextState == archer.curState_e)
		//		{
		//			me.RestartCurState();
		//			//이러고 바로 enter들어왔따가 해당 프레임에 바로 Update들어와서
		//			//애니메이션 끝난거 맞으니까 다시 Update 들어와서 활 2번뽑음. ㅋㅋ;
		//		}
		//		else
		//		{
		//			me.SetState((int)nextState);
		//		}

		//	}
		//}
    }

	public override void LateUpdateState()
	{
		base.LateUpdateState();


		me.LookAtSpecificBone(archer.spineBoneTr, me.targetObj.transform, Enums.eGizmoDirection.Back, new Vector3(0f,0f,-90f));
		me.LookAtSpecificBone(archer.headBoneTr, me.targetObj.transform, Enums.eGizmoDirection.Foward);

	}
	public override void ExitState()
	{
		me.animCtrl.SetLayerWeight((int)Enums.eHumanoidAvatarMask.Leg, 0);
		//이것도 나중에 코루틴으로 자연스럽게 돌아가도록.

		//if (archer.arrow != null)
		//{
		//	archer.arrow.ResetForReturn();
		//	ObjectPoolingCenter.Instance.ReturnObj(archer.arrow.gameObject);
		//}

		//archer.bow.ShootArrow();

		////me.isCombat = false;
	}

}
