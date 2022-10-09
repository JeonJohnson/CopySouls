using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Attack_Aiming : cState
{
	Archer archer = null;

	Transform testSpineTr;
	Transform headTr;

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }

		me.isCombat = true;

		me.animCtrl.SetTrigger("tAttack");

		
		
	}

	public override void UpdateState()
	{
		me.transform.rotation = me.LookAtSlow(me.transform, me.targetObj.transform, me.status.lookAtSpd * 2);
        //archer.MoveLegWhileTurn();

		//archer.bowString.transform.position = archer.rightHand.transform.position;


		if (Funcs.IsAnimationCompletelyFinish(me.animCtrl, "Archer_Atk_Shot"))
		{
			
		}
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();

		if (testSpineTr == null)
		{		
			testSpineTr = me.animCtrl.GetBoneTransform(HumanBodyBones.Spine);
			headTr = me.animCtrl.GetBoneTransform(HumanBodyBones.Head);
		}



		//Vector3 UpperDir = me.targetObj.transform.position - testSpineTr.position;
		//UpperDir.Normalize();
		////testSpineTr.LookAt(me.targetObj.transform.position);
		//testSpineTr.forward = -UpperDir;
		////testSpineTr.rotation = testSpineTr.rotation * Quaternion.Euler(new Vector3(359.621338f, 187.739853f, 197.663208f));
		//testSpineTr.rotation = testSpineTr.rotation * Quaternion.Euler(0f,0,-90f);

		me.LookAtSpecificBone(archer.spineBoneTr, me.targetObj.transform, Enums.eGizmoDirection.Back, new Vector3(0f,0f,-90f));

		//headTr.forward = me.targetObj.transform.position - headTr.position;
		me.LookAtSpecificBone(archer.headBoneTr, me.targetObj.transform, Enums.eGizmoDirection.Foward);
	}
	public override void ExitState()
	{
		//archer.bowString.transform.localPosition = archer.bowStringOriginPos;
	}

}
