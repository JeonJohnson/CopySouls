using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShakeEvent
{
    float duration;
    float timeRemaining;

    CameraShakeEvent data;

    Vector3 noiseOffset;
    public Vector3 noise;

    public CameraShakeEvent.Target target
    {
        get { return data.target; }
    }

    //积己磊
    public ShakeEvent(CameraShakeEvent data)
    {
        this.data = data;

        duration = data.duration;
        timeRemaining = duration;

        float rand = 32.0f;

        noiseOffset.x = Random.Range(0.0f, rand);
        noiseOffset.y = Random.Range(0.0f, rand);
        noiseOffset.z = Random.Range(0.0f, rand);
    }

    public void Update()
    {
        float deltaTime = Time.deltaTime;

        timeRemaining -= deltaTime;

        float noiseOffsetDelta = deltaTime * data.frequency;

        noiseOffset.x += noiseOffsetDelta;
        noiseOffset.y += noiseOffsetDelta;
        noiseOffset.z += noiseOffsetDelta;

        noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
        noise.y = Mathf.PerlinNoise(noiseOffset.y, 1.0f);
        noise.z = Mathf.PerlinNoise(noiseOffset.z, 2.0f);

        noise -= Vector3.one * 0.5f;

        noise *= data.amplitude;

        float agePercent = 1.0f - (timeRemaining / duration);
        noise *= data.blendOverLifetime.Evaluate(agePercent);
    }
    public bool IsAlive()
    {
        return timeRemaining > 0.0f;
    }
}

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance = null;

    List<ShakeEvent> shakeEvents = new List<ShakeEvent>();

    private void Awake()
    {
        if(Instance == null)
        {
            Debug.Log("墨皋扼溅捞农 教臂沛 积己");
            Instance = this;
        }
    }

    void LateUpdate()
    {
        Vector3 positionOffset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 rotationOffset = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

        for (int i = shakeEvents.Count - 1; i != -1; i--)
        {
            ShakeEvent se = shakeEvents[i];
            se.Update();

            if (se.target == CameraShakeEvent.Target.Position)
            {
                positionOffset += se.noise;
            }
            else
            {
                rotationOffset += se.noise;
            }

            if (!se.IsAlive())
            {
                shakeEvents.RemoveAt(i);
            }
        }

        transform.localPosition = positionOffset;
        transform.localEulerAngles = rotationOffset;
    }

    public void AddShakeEvent(float amplitude, float frequency, float duration,
        AnimationCurve blendOverLifetime, CameraShakeEvent.Target target)
    {
        CameraShakeEvent data = CameraShakeEvent.CreateInstance<CameraShakeEvent>();
        data.Init(amplitude, frequency, duration, blendOverLifetime, target);

        AddShakeEvent(data);
    }

    public void AddShakeEvent(CameraShakeEvent data)
    {
        Camera.main.GetComponentInParent<CameraShake>().AddShakeEvent(data);
        shakeEvents.Add(new ShakeEvent(data));
    }
}

//public void AddShakeEvent(GameObject obj)
//{

//    CameraShakeEvent data = CameraShakeEvent.CreateInstance<CameraShakeEvent>();
//    Camera.main.GetComponentInParent<CameraShake>().AddShakeEvent(data);
//    if (data != null)
//    {
//        shakeEvents.Add(new ShakeEvent(data));
//    }

//    //CameraShakeEvent temp = obj.GetComponent<CameraShakeEvent>();

//    //if (temp != null)
//    //{
//    //    shakeEvents.Add(new ShakeEvent(temp.data));
//    //}

//    //ShakeOnKey temp = obj.GetComponent<ShakeOnKey>();

//    //if (temp != null)
//    //{ shakeEvents.Add(new ShakeEvent(temp.data)); }

//    //shakeEvents.Add(new ShakeEvent(go.GetComponent<ShakeEvent>()));
//}