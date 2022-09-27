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

    /*
    float cameraDist = 0f; // 리그로부터 카메라 거리
    float cameraWidth = 0f; // y
    float cameraHeight = -5f; // x

    Vector3 dir;
    */
    void Start()
    {
       /* cameraDist = Mathf.Sqrt(cameraWidth * cameraHeight + cameraDist);

        dir = new Vector3(0, cameraHeight, cameraWidth).normalized;
        */
    }

    void Awake()
    {
        Transform = transform;
        cameraTransform = Camera.main.transform;
        cameraParentTransform = cameraTransform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
        CameraBalance();
        CameraDistanceCtrl();
        //WallDetect();
    }

     void LateUpdate()
     {
        CameraMove();
    }

    void CameraMove()
    {
        cameraParentTransform.position = Transform.position + Vector3.up * 1.4f;  // 카메라 초기 위치 
        MouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensitivity, Input.GetAxisRaw("Mouse X") * mouseSensitivity, 0); // 마우스 움직임 감지
        if (MouseMove.x < -30) MouseMove.x = -30; // 카메라 최고 높이 제한
        else if (30 < MouseMove.x) MouseMove.x = 30; // 카메라 최저 높이 제한

        cameraParentTransform.localEulerAngles = MouseMove;
        /*
        Quaternion cameraRotation = cameraParentTransform.rotation;
        cameraRotation.x = cameraRotation.z = 0;
        Transform.rotation = Quaternion.Slerp(Transform.rotation, cameraRotation, 10.0f * Time.deltaTime);
        */
    }
    void CameraBalance()
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

    /*void WallDetect()
    {
        Vector3 rayTarget = transform.up * MouseMove.x + transform.forward * MouseMove.y;
        RaycastHit hitinfo;
        Physics.Raycast(transform.position, rayTarget, out hitinfo, cameraDist);

        if(hitinfo.point != Vector3.zero)
        {
            cameraParentTransform.position = hitinfo.point;
        }
        else
        {
            cameraParentTransform.localPosition = Vector3.zero;

            cameraParentTransform.Translate(dir * cameraDist);
        }
    }*/
}
