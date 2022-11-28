using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shake Data", menuName = "Camera Effect Data/Shake Data", order = 1)]
public class EffectData : ScriptableObject
{
    bool calculate;
    float currentTime = 0.0f;
    public bool isFinish;

    public float duration;      //지속력
    public float amplitude;     //강도
    public float frequency;     //흔들리는 빈도

    [Header("Transform Value")]
    [Range(0,10)]
    public float Transform_X;  
    [Range(0,10)]
    public float Transform_Y;
    [Range(0, 10)]
    public float Transform_Z;

    public float Transform_Amplitude;     


    [Header("Rotation Value")]
    [Range(0, 10)]
    public float Rotation_X;
    [Range(0, 10)]
    public float Rotation_Y;
    [Range(0, 10)]
    public float Rotation_Z;

    public float Rotation_Amplitude;


    float Tr_x;
    float Tr_y;
    float Tr_z;



    public EffectData(float _duration,float _amplitude, float _frequency,
        float _Transform_X, float _Transform_Y,float _Transform_Z,float _Transform_Amplitude)
    {
        duration = _duration;
        amplitude = _amplitude;
        frequency = _frequency;
        Transform_X = _Transform_X;
        Transform_Y = _Transform_Y;
        Transform_Z = _Transform_Z;
        Transform_Amplitude = _Transform_Amplitude;
    }

    public void Start()
    {
        Debug.Log("언제 들어오는지 확인하자~~~");
    }


    public void Update()
    {
        if (currentTime < duration)
        {
            if (!calculate) Calculation();
            currentTime += Time.deltaTime;
            Shake();
        }
        else
        {
            Camera.main.transform.localPosition = CameraEffect.instance.OriginPos;
            Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot;
            currentTime = 0;
            CameraEffect.instance.curData = null;
            calculate = false;
        }
    }

    private void Calculation()
    {
        calculate = true;
        Tr_x = Transform_X * Transform_Amplitude;
        Tr_y = Transform_Y * Transform_Amplitude;
        Tr_z = Transform_Z * Transform_Amplitude;
    }

    private void Shake()
    {
        Debug.Log(Tr_x);
        Vector3 randVec = Random.onUnitSphere;
        randVec.x *= Tr_x;
        randVec.y *= Tr_y;
        randVec.z *= Tr_z;

        Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + randVec;
        //Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + randVec;
    }
}
