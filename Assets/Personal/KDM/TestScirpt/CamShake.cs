using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private static CamShake instance;
    public static CamShake Instance => instance;

    private CameraTest cameraShakeController;

    public float shakeTime;      // ��鸲 ���� �ð�
    public float shakeIntensity; // ��鸲 ����

    public float shakeposX = 0f; // X�� ��ġ ��鸲
    public float shakeposY = 0f; // Y�� ��ġ ��鸲
    public float shakeposZ = 0f; // Z�� ��ġ ��鸲

    public float shakeposXTime = 0f; // X�� ��ġ ��鸲 �ð�
    public float shakeposYTime = 0f; // Y�� ��ġ ��鸲 �ð�
    public float shakeposZTime = 0f; // Z�� ��ġ ��鸲 �ð�

    public float shakerotX = 0f; // X�� ȸ�� ��鸲
    public float shakerotY = 0f; // Y�� ȸ�� ��鸲
    public float shakerotZ = 0f; // Z�� ȸ�� ��鸲

    public float shakerotXTime = 0f; // X�� ȸ�� ��鸲 �ð�
    public float shakerotYTime = 0f; // Y�� ȸ�� ��鸲 �ð�
    public float shakerotZTime = 0f; // Z�� ȸ�� ��鸲 �ð�

    private void Awake()
    {
        Debug.Log("ķ����ũ ����");
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

    // OnShakeCamera(1)       = 1�ʰ� 0.1 ���� ��鸲
    // OnShakeCamera(0.5f, 1) = 0.5�ʰ� 1�� ���� ��鸲
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
