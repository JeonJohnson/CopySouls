using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Shake Data", menuName = "Camera Effect Data/Shake Data", order = 1)]
public class EffectData : ScriptableObject
{
    //public const float COEF_SHAKE_POSITION = 01.0f;
    //public const float COEF_SHAKE_ROTATION = 25.0f;

    private const float SEED_MIN = 0.0f;
    private const float SEED_MAX = 1000.0f;

    public AnimationCurve Curve = AnimationCurve.EaseInOut(
            0.0f,
            1.0f,
            1.0f,
            0.0f
        );
    // PROPERTIES: ----------------------------------------------------------------------------
    public bool shakePosition = true;
    public bool shakeRotation = true;
    public float duration;
    public float magnitude;
    public float roughness;
    public float perlinSpeed;
    public float radius;
    


    private Vector3 originPos;
    private Vector3 originRot;
    private Vector3 seed;
    
    private float startTime;
    public float currentTime;

    // INITIALIZERS: --------------------------------------------------------------------------
    private void Initialize()
    {
        originPos =  Camera.main.transform.localPosition;
        originRot = Camera.main.transform.localEulerAngles;

        //this.magnitude = 1.0f;
        //this.roughness = 1.0f;
        //this.perlinSpeed = 0.0f;
        
        this.seed = new Vector3(
            Random.Range(SEED_MIN, SEED_MAX),
            Random.Range(SEED_MIN, SEED_MAX),
            Random.Range(SEED_MIN, SEED_MAX)
        );

        this.startTime = Time.time;
        //this.duration = 1.0f;
    }
    public EffectData(float duration, float roughness, float magnitude,
        bool shakePosition, bool shakeRotation,float radius, AnimationCurve curve)
    {
        this.Initialize();

        this.shakePosition = shakePosition;
        this.shakeRotation = shakeRotation;
        this.duration = duration;
        this.roughness = roughness;
        this.magnitude = magnitude;
        this.radius = radius;
        this.Curve = curve;
    }
    public EffectData(float duration, EffectData cameraShake)
    {
        this.startTime = Time.time;
        this.seed = cameraShake.seed;
        this.perlinSpeed = cameraShake.perlinSpeed;

        this.shakePosition = cameraShake.shakePosition;
        this.shakeRotation = cameraShake.shakeRotation;

        this.duration = duration;
        this.roughness = cameraShake.roughness;
        this.magnitude = cameraShake.magnitude;

        this.originPos = cameraShake.originPos;
        this.originRot = cameraShake.originRot;
        this.radius = cameraShake.radius;
    }

    public void Update()
    {
        Debug.Log(duration);
        if(currentTime >= duration)
        {
            currentTime = 0.0f;
            perlinSpeed = 0.0f;
            originPos = Vector3.zero;
            originRot = Vector3.zero;
            CameraEffect.instance.curData = null;
        }
        else
        {
            currentTime += Time.deltaTime;
            Shake();
        }
        //return amount * this.magnitude * coefficient;
    }

    private void Shake()
    {
        Vector3 amount = new Vector3(
            Mathf.PerlinNoise(this.perlinSpeed, this.seed.x) - 0.5f,
            Mathf.PerlinNoise(this.perlinSpeed, this.seed.y) - 0.5f,
            Mathf.PerlinNoise(this.perlinSpeed, this.seed.z) - 0.5f
        );

        this.perlinSpeed += Time.deltaTime * this.roughness;

        float coefficient = 1.0f;

        //float distance = Vector3.Distance(this.originPos, Camera.main.transform.localPosition);
        //
        //Debug.Log(Mathf.Clamp01(distance / this.radius));
        //
        //coefficient = 1f - Mathf.Clamp01(distance / this.radius);
        //
        //Debug.Log("양 : " +amount + "컨피시언트 : " + coefficient);
        
        if(shakePosition) Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + amount * this.magnitude * coefficient;
        if(shakeRotation) Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + amount * this.magnitude * coefficient;
        //Camera.main.transform.localPosition += amount * this.magnitude * coefficient;
        //Camera.main.transform.localPosition += amount * this.magnitude * coefficient;
    }

    //public float GetEasing()
    //{
    //    //return EASING.Evaluate(this.GetNormalizedProgress());
    //}

    public bool IsComplete()
    {
        return this.GetNormalizedProgress() >= 1.0f;
    }

    public Vector3 GetInfluencePosition()
    {
        return (this.shakePosition ? Vector3.one : Vector3.zero);
    }

    public Vector3 GetInfluenceRotation()
    {
        return (this.shakeRotation ? Vector3.one : Vector3.zero);
    }

    // PRIVATE METHODS: -----------------------------------------------------------------------

    private float GetNormalizedProgress()
    {
        return Mathf.Clamp01((Time.time - this.startTime) / this.duration);
    }
}
//bool isOver;
//bool isConflict;
//bool isStart;
//bool calculate;
//float addValue;
//float conflictTime = 0.0f;
//
//float currentTime = 0.0f;


//[Header("Value")]
//public float duration;      //지속력              //
//[Range(0, 10)]
//public float amplitude;     //강도                //
//
//public bool Quick;
//[Range(0, 5)]
//public int frequency;     //흔들리는 빈도       // SCORE
//
//[Header("Transform Value")]
//[Range(0, 10)]
//public float Transform_X;
//[Range(0, 10)]
//public float Transform_Y;
//[Range(0, 10)]
//public float Transform_Z;
//
//public float Transform_Amplitude;
//
//
//[Header("Rotation Value")]
//[Range(0, 10)]
//public float Rotation_X;
//[Range(0, 10)]
//public float Rotation_Y;
//[Range(0, 10)]
//public float Rotation_Z;
//
//public float Rotation_Amplitude;
//
//float Tr_x;
//float Tr_y;
//float Tr_z;
//
//float Rot_x;
//float Rot_y;
//float Rot_z;
//
//float Score;
//
//[SerializeField]
//private AnimationCurve curve;

//public float GetScore {get { return Score; }}
//public bool GetStart { get { return isStart; } set { isStart = value; } }
//public bool Conflict { get { return isConflict; } set { isConflict = value; } }
//public float AddValue { get { return addValue; } set { addValue = value; } }
//public float ConflictTime { get { return conflictTime; } set { conflictTime = value; } }
//public float CurrentTime { get { return currentTime; } set { currentTime = value; } }
//public bool Over { get { return isOver; } set { isOver = value; } }




//public EffectData(float _duration,float _amplitude, int _frequency,
//    float _Transform_X, float _Transform_Y,float _Transform_Z,float _Transform_Amplitude,
//    float _Rotation_X, float _Rotation_Y, float _Rotation_Z, float _Rotation_Amplitude)
//{
//    duration = _duration;
//    amplitude = _amplitude;
//    frequency = _frequency;
//    Transform_X = _Transform_X;
//    Transform_Y = _Transform_Y;
//    Transform_Z = _Transform_Z;
//    Transform_Amplitude = _Transform_Amplitude;
//    Rotation_X = _Rotation_X;
//    Rotation_Y = _Rotation_Y;
//    Rotation_Z = _Rotation_Z;
//    Rotation_Amplitude = _Rotation_Amplitude;
//}

//public void Start()
//    {
//        Debug.Log("언제 들어오는지 확인하자~~~");
//    }
//   public void Update()
//  {


        //if(isStart)
        //{
        //    if (currentTime < duration)
        //    {
        //        if (!calculate) Calculation();
        //        currentTime += Time.deltaTime;
        //        Shake();
        //    }
        //    else
        //    {
        //        Camera.main.transform.localPosition = CameraEffect.instance.OriginPos;
        //        Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot;
        //        CameraEffect.instance.curData = null;
        //        calculate = false;
        //        isStart = false;
        //        addValue = 1;
        //        conflictTime = 0.0f;
        //        currentTime = 0.0f;
        //    }
        //}
//    }

    //private void Calculation()
    //{
    //    calculate = true;
    //    Tr_x = Transform_X * Transform_Amplitude;
    //    Tr_y = Transform_Y * Transform_Amplitude;
    //    Tr_z = Transform_Z * Transform_Amplitude;
    //
    //    Rot_x = Rotation_X * Rotation_Amplitude;
    //    Rot_y = Rotation_Y * Rotation_Amplitude;
    //    Rot_z = Rotation_Z * Rotation_Amplitude;
    //
    //    Score = Transform_X;
    //}

    //private void Shake()
    //{
        //Debug.Log(curve.Evaluate(currentTime));

        //if (Quick)
        //{
        //    Vector3 vector;
        //    for (int i = 0; i < frequency; i++)
        //    {
        //        Vector3 randVec = Random.onUnitSphere * amplitude;
        //        randVec.x *= Tr_x;
        //        randVec.y *= Tr_y;
        //        randVec.z *= Tr_z;
        //        randVec = new Vector3(randVec.x, randVec.y, randVec.z);
        //
        //        Vector3 randRot = Random.onUnitSphere * amplitude;
        //        randRot.x *= Rot_x;
        //        randRot.y *= Rot_y;
        //        randRot.z *= Rot_z;
        //
        //        if (Conflict)
        //        {
        //            randVec *= addValue;
        //            if (currentTime >= conflictTime) Conflict = false;
        //        }
        //
        //        Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + randVec;
        //        Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + randRot;
        //    }
        //}
        //Vector3 randVec = Random.onUnitSphere * amplitude;
        //
        //randVec.x *= Tr_x;
        //randVec.y *= Tr_y;
        //randVec.z *= Tr_z;
        //randVec = new Vector3(randVec.x, randVec.y, randVec.z);
        //
        //Vector3 randRot = Random.onUnitSphere * amplitude;
        //randRot.x *= Rot_x;
        //randRot.y *= Rot_y;
        //randRot.z *= Rot_z;
        //
        //if (Conflict)
        //{
        //    randVec *= addValue;
        //    if (currentTime >= conflictTime) Conflict = false;
        //}
        //
        //Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + randVec;
        //Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + randRot;
    //}

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
    



