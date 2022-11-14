using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Ragdoll : Enemy_Ragdoll
{
	public Rigidbody[] propRds;
	

    public Transform spineTr;
    public Vector3 preSpinePos;
    private int stopCount;
	
	//private bool isRagdollStop;

    private IEnumerator EnumCheckIsGoatStop()
    {
		preSpinePos = spineTr.position;

		while (true)
        {
			float differ = (spineTr.position - preSpinePos).magnitude;
			if (differ < 0.01f)
			{
				stopCount++;
				if (stopCount > 10)
				{
					break;
				}
			}
			else
			{
				stopCount = 0;
			}

			preSpinePos = spineTr.position;
			yield return new WaitForSeconds(0.01f);
		}

		for (int i = 0; i < propRds.Length; ++i)
		{
			if (!propRds[i])
			{
				continue;
			}
			propRds[i].GetComponent<Collider>().isTrigger = false;
			propRds[i].useGravity = true;
			propRds[i].isKinematic = false;
		}

    }

	public override void OnEnable()
	{
		base.OnEnable();

		for (int i = 0; i < propRds.Length; ++i)
		{
			if (!propRds[i])
			{
				continue;
			}
			propRds[i].GetComponent<Collider>().isTrigger = true;
			propRds[i].useGravity = false;
			propRds[i].isKinematic = true;
		}

		StartCoroutine(EnumCheckIsGoatStop());
	}

	

}
