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

    public float duration;      //지속력              // 
    public float amplitude;     //강도                //
    public float frequency;     //흔들리는 빈도       // SCORE

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

    float Score;
    public float GetScore {get { return Score; }}
    public bool GetStart { get { return isStart; } set { isStart = value; } }
    public bool Conflict { get { return isConflict; } set { isConflict = value; } }
    public float AddValue { get { return addValue; } set { addValue = value; } }
    public float ConflictTime { get { return conflictTime; } set { conflictTime = value; } }
    public float CurrentTime { get { return currentTime; } set { currentTime = value; } }
    public bool Over { get { return isOver; } set { isOver = value; } }




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
        
        Score = 100;
    }

    private void Shake()
    {
        Debug.Log("흔드는 중~~");
        Vector3 randVec = Random.onUnitSphere;
        randVec.x *= Tr_x;
        randVec.y *= Tr_y;
        randVec.z *= Tr_z;
        randVec = new Vector3(randVec.x, randVec.y, randVec.z);

        if(Conflict)
        {
            randVec *= addValue;
            if (currentTime >= conflictTime) Conflict = false;
        }

        Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + randVec;
        Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + randVec;
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
