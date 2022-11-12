using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Static : MonoBehaviour, IPoolingObject
{

    public float aliveTime;

    public IEnumerator AliveCoroutine()
    {
        yield return new WaitForSeconds(aliveTime);

        ResetForReturn();
        ObjectPoolingCenter.Instance.ReturnObj(this.gameObject);
    }

    public void ResetForReturn()
	{
            
	}


	public void OnEnable()
	{
		StartCoroutine(AliveCoroutine());
		//	AliveCoroutine()

	}
	//void Update()
	//   {
	//       curTime += Time.deltaTime;

	//       if (curTime >= maxTime)
	//       {
	//           curTime = 0f;

	//           ObjectPoolingCenter.Instance.ReturnObj(this.gameObject);
	//       }
	//   }
}
