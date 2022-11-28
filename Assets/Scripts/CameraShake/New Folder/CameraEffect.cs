using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public static CameraEffect instance = null;

    Vector3 originPos;
    Vector3 originRot;
    bool isShake = false;
    float currentTime = 0.0f;

    public float duration = 1.0f;
    public float amplitude = 1.0f;
    public float frequency = 1.0f;

    public bool DoShake
    {
        get { return isShake; }
        set { isShake = value; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    private void Start()
    {
        originPos =  Camera.main.transform.localPosition;
        originRot = Camera.main.transform.localEulerAngles;
    }

    private void Update()
    {
        if (isShake == true)
        {
            currentTime += Time.deltaTime;
            if (currentTime < duration)
            {
                Vector3 randVec = Random.onUnitSphere;
                //r.z = 0;
                Camera.main.transform.localPosition = originPos + randVec;
                Camera.main.transform.localEulerAngles = originRot + randVec;
            }
            else
            {
                Camera.main.transform.localPosition = originPos;
                Camera.main.transform.localEulerAngles = originRot;
                currentTime = 0.0f;
                isShake = false;
            }
        }
    }
}
