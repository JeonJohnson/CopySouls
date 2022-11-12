using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Static : MonoBehaviour, IPoolingObject
{
    [HideInInspector]   public float curTime = 0;
    public float maxTime;

	void IPoolingObject.ResetForReturn()
	{
            
	}


	void Update()
    {
        curTime += Time.deltaTime;

        if (curTime >= maxTime)
        {
            curTime = 0f;
            
            ObjectPoolingCenter.Instance.ReturnObj(this.gameObject);
        }
    }
}
