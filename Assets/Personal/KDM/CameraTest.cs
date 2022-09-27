using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    [SerializeField]
    float mouseSensitivity = 2.0f;

    Transform Transform;

    Vector3 MouseMove;
    Transform cameraParentTransform;
    Transform cameraTransform;
    
    void Awake()
    {
        Transform = transform;
        cameraTransform = Camera.main.transform;
        cameraParentTransform = cameraTransform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        Balance();
        CameraDistanceCtrl();
    }

     void LateUpdate()
    {
        cameraParentTransform.position = Transform.position + Vector3.up * 1.4f;  // 카메라 초기 위치 
        MouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensitivity, Input.GetAxisRaw("Mouse X") * mouseSensitivity, 0); // 마우스 움직임 가감
        if (MouseMove.x < -30) MouseMove.x = -30; // 카메라 최고 높이 제한
        else if (30 < MouseMove.x) MouseMove.x = 30; // 카메라 최저 높이 제한

        cameraParentTransform.localEulerAngles = MouseMove;
    }

    void Balance()
    {
        if (Transform.eulerAngles.x != 0 || Transform.eulerAngles.z != 0) // 모종의 이유로 기울어지면 카메라 바로잡기
            Transform.eulerAngles = new Vector3(0, Transform.eulerAngles.y, 0);
    }

    void CameraDistanceCtrl()
    {
        Camera.main.transform.localPosition += new Vector3(0, 0, Input.GetAxisRaw("Mouse ScrollWheel") * 2.0f); // 휠로 카메라 거리 조절
        if (-1 < Camera.main.transform.localPosition.z)
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, -1);  // 카메라 최대 근접 수치
        else if (Camera.main.transform.localPosition.z < -5)
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, -5);  // 카메라 최대 먼 수치
    }
}
