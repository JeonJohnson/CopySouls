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
    private float roopTimer;

    public ShakeType shakeType;
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
    public float shakeSpeed;
    private float perlinSpeed;
    public float radius;

    public bool isSeedUpdate = false;
    public bool roop = false;

    private Vector3 originPos;
    private Vector3 originRot;
    private Vector3 seed;

    private float startTime;
    private float currentTime;

    // INITIALIZERS: --------------------------------------------------------------------------
    private void Initialize()
    {
        if (!isSeedUpdate) SetSeed();


        originPos = Camera.main.transform.localPosition;
        originRot = Camera.main.transform.localEulerAngles;

        //this.magnitude = 1.0f;
        //this.roughness = 1.0f;
        //this.perlinSpeed = 0.0f;

        this.startTime = Time.time;
        //this.duration = 1.0f;
    }


    public EffectData(float duration, float shakeSpeed, float magnitude,
        bool shakePosition, bool shakeRotation, float radius, AnimationCurve curve, bool SeedUpdate)
    {
        this.Initialize();

        this.shakePosition = shakePosition;
        this.shakeRotation = shakeRotation;
        this.duration = duration;
        this.shakeSpeed = shakeSpeed;
        this.magnitude = magnitude;
        this.radius = radius;
        this.Curve = curve;
        this.isSeedUpdate = SeedUpdate;
    }

    public EffectData(float duration, EffectData cameraShake)
    {
        this.startTime = Time.time;
        this.seed = cameraShake.seed;
        this.perlinSpeed = cameraShake.perlinSpeed;

        this.shakePosition = cameraShake.shakePosition;
        this.shakeRotation = cameraShake.shakeRotation;

        this.duration = duration;
        this.shakeSpeed = cameraShake.shakeSpeed;
        this.magnitude = cameraShake.magnitude;

        this.originPos = cameraShake.originPos;
        this.originRot = cameraShake.originRot;
        this.radius = cameraShake.radius;
    }

    public void Update()
    {
        if(!roop)
        {
            if (isSeedUpdate) SetSeed();
            //Debug.Log(duration);
            if (currentTime > duration)
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
        }
        else
        {
            if(!CameraEffect.instance.Stop)
            {
                currentTime = 1.0f;
                Shake();
            }
            else
            {
                roopTimer = 0.0f;
                currentTime = 0.0f;
                perlinSpeed = 0.0f;
                originPos = Vector3.zero;
                originRot = Vector3.zero;
                CameraEffect.instance.curData = null;
            }
        }

        
        //return amount * this.magnitude * coefficient;
    }

    private void Shake()
    {
        if(shakeType == ShakeType.Smooth)
        {
            Vector3 amount = new Vector3(Mathf.PerlinNoise(this.perlinSpeed, this.seed.x) - 0.5f,
                                         Mathf.PerlinNoise(this.perlinSpeed, this.seed.y) - 0.5f,
                                         Mathf.PerlinNoise(this.perlinSpeed, this.seed.z) - 0.5f);

            this.perlinSpeed += Time.deltaTime * this.shakeSpeed;
            float coefficient = 1.0f;
            Vector3 total = amount * this.magnitude * coefficient;

            //float distance = Vector3.Distance(this.originPos, Camera.main.transform.localPosition);
            //
            //Debug.Log(Mathf.Clamp01(distance / this.radius));
            //
            //coefficient = 1f - Mathf.Clamp01(distance / this.radius);
            //
            //Debug.Log("양 : " +amount + "컨피시언트 : " + coefficient);
            if (!roop)
            {
                if (shakePosition) Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + total * Curve.Evaluate(currentTime);
                if (shakeRotation) Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + total * Curve.Evaluate(currentTime);
            }
            else
            {
                roopTimer += Time.deltaTime;
                if (shakePosition) Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + total * Curve.Evaluate(roopTimer);
                if (shakeRotation) Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + total * Curve.Evaluate(roopTimer);
            }
        }
        else
        {
            float speed =  Time.deltaTime * this.shakeSpeed;

            Vector3 randVec = Random.onUnitSphere * this.magnitude * speed;
            Vector3 randRot = Random.onUnitSphere * this.magnitude * speed;

            if (!roop)
            {
                if (shakePosition) Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + randVec * Curve.Evaluate(currentTime);
                if (shakeRotation) Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + randRot * Curve.Evaluate(currentTime);
            }
            else
            {
                roopTimer += Time.deltaTime;
                if (shakePosition) Camera.main.transform.localPosition = CameraEffect.instance.OriginPos + randVec * Curve.Evaluate(roopTimer);
                if (shakeRotation) Camera.main.transform.localEulerAngles = CameraEffect.instance.OriginRot + randRot * Curve.Evaluate(roopTimer);
            }
        }
    }

    private void SetSeed()
        {
            this.seed = new Vector3(Random.Range(SEED_MIN, SEED_MAX),
                                    Random.Range(SEED_MIN, SEED_MAX),
                                    Random.Range(SEED_MIN, SEED_MAX)
    );
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
    



