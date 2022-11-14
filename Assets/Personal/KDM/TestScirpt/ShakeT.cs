using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeT : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.O))
        {
            CameraShake.Instance.AddShakeEvent(data);
        }
    }
}
