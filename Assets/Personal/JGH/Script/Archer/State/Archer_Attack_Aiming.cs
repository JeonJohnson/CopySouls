using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Attack_Aiming : cState
{
	Archer archer = null;

	//여기서도
	//1. 정해진 시간 지나면 쏘는 패턴
	//2. 타겟의 특정 행동 할 때 까지 쏘는패턴?
	//3. 1+2 패턴으로 정해진 시간 동안 쏠예정인데 중간에 특정 행동하면 쏘는거

	//또 하나 더 
	//1. 쏘기 전까지 시간을 정하고 비례해서 드로우 시간 정하기?
	//2. 드로우시간은 무적권 정해져있고 에이밍 시간을 랜덤?

	// aiming 패턴에서 드로우 시간은 고정으로 하고 에이밍 시간을 랜덤으로 가자

	public float aimingTime;

	public float drawTime;
	public float archerDrawAnimSpd;
	public float bowDrawAnimSpd;

	public bool isHook = false;

	public void PullStart()
	{
		archer.bow.animCtrl.SetFloat("fPullSpd", bowDrawAnimSpd);
		archer.bow.animCtrl.SetTrigger("tPull");

		isHook = true;
	}

	public void CalcDrawSpd()
	{
		drawTime = 5f;
		//유닛 드로우 애니메이션 기본 1초
		//보우 드로우 애니메이션 기본 0.4초
		//기본 시간 기준 드로우 애니메이션 속도 0.25
		archerDrawAnimSpd = 1f / drawTime;
		bowDrawAnimSpd = 0.25f / drawTime;
		
		archer.animCtrl.SetFloat("fDrawSpd", archerDrawAnimSpd);

		Debug.Log(bowDrawAnimSpd);
		aimingTime = Random.Range(0.5f, 2f);
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		if (archer.PullStartEvent == null)
		{//이거 나중에 cState 생성자 만들거나 Initialize에서 쓸 수 있도록
			archer.PullStartEvent += PullStart;
		}

		me.isCombat = true;

		me.animCtrl.SetTrigger("tAttack");

		CalcDrawSpd();
		
		
	}

	public override void UpdateState()
	{
		me.transform.rotation = me.LookAtSlow(me.transform, me.targetObj.transform, me.status.lookAtSpd * 2);
        archer.ActingLegWhileTurn();

		//archer.bowString.transform.position = archer.rightHand.transform.position;


		if (Funcs.IsAnimationCompletelyFinish(me.animCtrl, "Archer_Atk_Shot"))
		{
			
		}
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();

		//Vector3 UpperDir = me.targetObj.transform.position - testSpineTr.position;
		//UpperDir.Normalize();
		////testSpineTr.LookAt(me.targetObj.transform.position);
		//testSpineTr.forward = -UpperDir;
		////testSpineTr.rotation = testSpineTr.rotation * Quaternion.Euler(new Vector3(359.621338f, 187.739853f, 197.663208f));
		//testSpineTr.rotation = testSpineTr.rotation * Quaternion.Euler(0f,0,-90f);

		me.LookAtSpecificBone(archer.spineBoneTr, me.targetObj.transform, Enums.eGizmoDirection.Back, new Vector3(0f,0f,-90f));

		//headTr.forward = me.targetObj.transform.position - headTr.position;
		me.LookAtSpecificBone(archer.headBoneTr, me.targetObj.transform, Enums.eGizmoDirection.Foward);

		if (isHook)
		{ archer.bow.stringTr.position = archer.rightIndexFingerBoneTr.position; }
	}
	public override void ExitState()
	{
		me.animCtrl.SetLayerWeight((int)Enums.eHumanoidAvatarMask.Leg, 0);
		//이것도 나중에 코루틴으로 자연스럽게 돌아가도록.

		//archer.bowString.transform.localPosition = archer.bowStringOriginPos;
	}

}
