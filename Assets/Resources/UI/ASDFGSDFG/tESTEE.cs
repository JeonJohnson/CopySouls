using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tESTEE : MonoBehaviour
{
    public CameraShakeEvent data;
    CameraShake st;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if ����
        CameraShake.Instance.AddShakeEvent(data);
    }
}
