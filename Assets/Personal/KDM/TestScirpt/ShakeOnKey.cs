using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeOnKey : MonoBehaviour
{
    public CameraShake st;
    public CameraShakeEvent data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.GetComponentInParent<CameraShake>().AddShakeEvent(data);
        }
    }
}
