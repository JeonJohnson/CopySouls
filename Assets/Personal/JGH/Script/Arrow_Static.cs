using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Static : MonoBehaviour
{
    public float maxTime;
    public float curTime = 0;

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime >= maxTime)
        {
            curTime = 0f;
            ObjectPoolingCenter.Instance.ReturnObj(this.gameObject,Enums.ePoolingObj.Arrow_Static);
            //옵줵 풀링센터로 ㅂㅂ
        }
    }
}
