using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shake Data", menuName = "Camera Effect Data/Shake Data", order = 1)]
public class EffectData : ScriptableObject
{
    bool isOver;
    bool isConflict;
    bool isStart;
    bool calculate;
    float addValue;
    float conflictTime = 0.0f;

    float currentTime = 0.0f;


    [Header("Value")]
    public float duration;      //지속력              //
    [Range(0, 10)]
    public float amplitude;     //강도                //

    public bool Quick;
    [Range(0, 5)]
    public int frequency;     //흔들리는 빈도       // SCORE

    [Header("Transform Value")]
    [Range(0, 10)]
    public float Transform_X;
    [Range(0, 10)]
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

    float Rot_x;
    float Rot_y;
    float Rot_z;

    float Score;
    public float GetScore {get { return Score; }}
    public bool GetStart { get { return isStart; } set { isStart = value; } }
    public bool Conflict { get { return isConflict; } set { isConflict = value; } }
    public float AddValue { get { return addValue; } set { addValue = value; } }
    public float ConflictTime { get { return conflictTime; } set { conflictTime = value; } }
    public float CurrentTime { get { return currentTime; } set { currentTime = value; } }
    public bool Over { get { return isOver; } set { isOver = value; } }




    public EffectData(float _duration,float _amplitude, int _frequency,
        float _Transform_X, float _Transform_Y,float _Transform_Z,float _Transform_Amplitude,
        float _Rotation_X, float _Rotation_Y, float _Rotation_Z, float _Rotation_Amplitude)
    {
        duration = _duration;
        amplitude = _amplitude;
        frequency = _frequency;
        Transform_X = _Transform_X;
        Transform_Y = _Transform_Y;
        Transform_Z = _Transform_Z;
        Transform_Amplitude = _Transform_Amplitude;
        Rotation_X = _Rotation_X;
        Rotation_Y = _Rotation_Y;
        Rotation_Z = _Rotation_Z;
        Rotation_Amplitude = _Rotation_Amplitude;
    }

    public void Start()
    {
        Debug.Log("언제 들어오는지 확인하자~~~");
    }
    public void Update()
    {
        if(isStart)
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
                CameraEffect.instance.curData = null;
                calculate = false;
                isStart = false;
                addValue = 1;
                conflictTime = 0.0f;
                currentTime = 0.0f;
            }
        }
    }

    private void Calculation()
    {
        calculate = true;
        Tr_x = Transform_X * Transform_Amplitude;
        Tr_y = Transform_Y * Transform_Amplitude;
        Tr_z = Transform_Z * Transform_Amplitude;

        Rot_x = Rotation_X * Rotation_Amplitude;
        Rot_y = Rotation_Y * Rotation_Amplitude;
        Rot_z = Rotation_Z * Rotation_Amplitude;

        Score = Transform_X;
    }

    private void Shake()
    {
        Debug.Log("흔드는 중~~");
        if (Quick)
        {
            Vector3 vector;
            for (int i = 0; i < frequency; i++)
            {
                Vector3 randVec = Random.onUnitSphere * amplitude;
                randVec.x *= Tr_x;
                randVec.y *= Tr_y;
                randVec.z *= Tr_z;
                randVec = new Vector3(randVec.x, randVec.y, randVec.z);

                Vector3 randRot = Random.onUnitSphere * amplitude;
                randRot.x *= Rot_x;
                randRot.y *= Rot_y;
                randRot.z *= Rot_z;

                if (Conflict)
                {
                    randVec *= addValue;
                    if (currentTime >= conflictTime) Conflict = false;
                }

                Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + randVec;
                Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + randRot;
            }
        }
        else
        {
            Vector3 randVec = Random.onUnitSphere * amplitude;
            randVec.x *= Tr_x;
            randVec.y *= Tr_y;
            randVec.z *= Tr_z;
            randVec = new Vector3(randVec.x, randVec.y, randVec.z);

            Vector3 randRot = Random.onUnitSphere * amplitude;
            randRot.x *= Rot_x;
            randRot.y *= Rot_y;
            randRot.z *= Rot_z;

            if (Conflict)
            {
                randVec *= addValue;
                if (currentTime >= conflictTime) Conflict = false;
            }

            Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + randVec;
            Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + randRot;
        }
    }

    //private void HandleCameraCollisions()
    //{
    //    float targetPosition = defaultPosition;
    //    RaycastHit hit;
    //    Vector3 direction = cameraTransform.position - cameraPivot.position;
    //    direction.Normalize();
    //
    //    if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
    //    {
    //        float distance = Vector3.Distance(cameraPivot.position, hit.point);
    //        targetPosition = -(distance - cameraCollisionOffset);
    //    }
    //
    //    if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
    //    {
    //        targetPosition = targetPosition - minimumCollisionOffset;
    //    }
    //
    //    cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
    //    cameraTransform.localPosition = cameraVectorPosition;
    //}


}
