using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private static CamShake instance;
    public static CamShake Instance => instance;

    private CameraTest cameraShakeController;

    public float shakeTime;      // 흔들림 지속 시간
    public float shakeIntensity; // 흔들림 세기

    public float shakeposX = 0f; // X축 위치 흔들림
    public float shakeposY = 0f; // Y축 위치 흔들림
    public float shakeposZ = 0f; // Z축 위치 흔들림

    public float shakeposXTime = 0f; // X축 위치 흔들림 시간
    public float shakeposYTime = 0f; // Y축 위치 흔들림 시간
    public float shakeposZTime = 0f; // Z축 위치 흔들림 시간

    public float shakerotX = 0f; // X축 회전 흔들림
    public float shakerotY = 0f; // Y축 회전 흔들림
    public float shakerotZ = 0f; // Z축 회전 흔들림

    public float shakerotXTime = 0f; // X축 회전 흔들림 시간
    public float shakerotYTime = 0f; // Y축 회전 흔들림 시간
    public float shakerotZTime = 0f; // Z축 회전 흔들림 시간

    private void Awake()
    {
        Debug.Log("캠쉐이크 생성");
        cameraShakeController = GetComponent<CameraTest>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            OnShakeCamera(0.1f, 1f);
        }
    }

    public CamShake()
    {
        instance = this;
    }

    // OnShakeCamera(1)       = 1초간 0.1 세기 흔들림
    // OnShakeCamera(0.5f, 1) = 0.5초간 1의 세기 흔들림
    public void OnShakeCamera(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");

        StopCoroutine("ShakeByRotation");
        StartCoroutine("ShakeByRotation");

    }

    public IEnumerator ShakeByPosition()
    {
        
        Vector3 shakeStartPosition = transform.position;

        while(shakeTime > 0.0f)
        {

            float posShakeX = Random.Range(shakeposXTime, shakeposX);
            float posShakeY = Random.Range(shakeposYTime, shakeposY);
            float posShakeZ = Random.Range(shakeposZTime, shakeposZ);

            transform.position = shakeStartPosition + new Vector3(posShakeX, posShakeY, posShakeZ) * shakeIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = shakeStartPosition;
    }

    public IEnumerator ShakeByRotation()
    {
        //cameraShakeController.isOnShake = true;

        Vector3 shakeStartRotation = transform.eulerAngles;

        float shakePower = 10f;

        while(shakeTime > 0.0f)
        {
            float rotShakeX = Random.Range(shakerotXTime, shakerotX);
            float rotShakeY = Random.Range(shakerotYTime, shakerotY);
            float rotShakeZ = Random.Range(shakerotZTime, shakerotZ);

            transform.rotation = Quaternion.Euler(shakeStartRotation + new Vector3(rotShakeX, rotShakeY, rotShakeZ) * shakeIntensity * shakePower);

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.rotation = Quaternion.Euler(shakeStartRotation);

        //cameraShakeController.isOnShake = false;
    }
}
