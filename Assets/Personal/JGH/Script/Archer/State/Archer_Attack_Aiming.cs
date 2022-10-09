using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Attack_Aiming : cState
{
	Archer archer = null;

	//���⼭��
	//1. ������ �ð� ������ ��� ����
	//2. Ÿ���� Ư�� �ൿ �� �� ���� �������?
	//3. 1+2 �������� ������ �ð� ���� �����ε� �߰��� Ư�� �ൿ�ϸ� ��°�

	//�� �ϳ� �� 
	//1. ��� ������ �ð��� ���ϰ� ����ؼ� ��ο� �ð� ���ϱ�?
	//2. ��ο�ð��� ������ �������ְ� ���̹� �ð��� ����?

	// aiming ���Ͽ��� ��ο� �ð��� �������� �ϰ� ���̹� �ð��� �������� ����

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
		//���� ��ο� �ִϸ��̼� ���� 1��
		//�׷��� ���� ��ο� �ִϸ��̼� ���� 0.4��
		//���� ������� *25
		archerDrawAnimSpd = 1 / drawTime;
		archer.animCtrl.SetFloat("fDrawSpd", archerDrawAnimSpd);
		bowDrawAnimSpd = archerDrawAnimSpd * 0.025f;
		Debug.Log(bowDrawAnimSpd);
		aimingTime = Random.Range(0.5f, 2f);
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		if (archer.PullStartEvent == null)
		{//�̰� ���߿� cState ������ ����ų� Initialize���� �� �� �ֵ���
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

		//archer.bowString.transform.localPosition = archer.bowStringOriginPos;
	}

}
