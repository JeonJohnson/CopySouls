using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Idle : cState
{

	Archer archer = null;

	Transform testSpineTr;
	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		if (archer == null)
		{ archer = me.GetComponent<Archer>(); }



		

		if (archer.isEquip)
		{
			me.animCtrl.SetTrigger("tIdle"); 
		}
		else 
		{ 
			me.animCtrl.SetTrigger("tIdle_Unequip"); 
		}

		
	}

	public override void UpdateState()
	{
		//archer.EquipWeapon();
	}

	public override void LateUpdateState()
	{
		if (testSpineTr == null)
		{
			testSpineTr = me.animCtrl.GetBoneTransform(HumanBodyBones.Spine);
		}

		Vector3 UpperDir =  me.targetObj.transform.position-testSpineTr.position;
		UpperDir.Normalize();
		//testSpineTr.LookAt(me.targetObj.transform.position);
		testSpineTr.up = -UpperDir;
		//testSpineTr.rotation = testSpineTr.rotation * Quaternion.Euler(-180f, 0f, 11.4f);

	}

	public override void ExitState()
	{
	}


}
