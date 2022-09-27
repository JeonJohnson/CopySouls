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
        cameraParentTransform.position = Transform.position + Vector3.up * 1.4f;  // ī�޶� �ʱ� ��ġ 
        MouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensitivity, Input.GetAxisRaw("Mouse X") * mouseSensitivity, 0); // ���콺 ������ ����
        if (MouseMove.x < -30) MouseMove.x = -30; // ī�޶� �ְ� ���� ����
        else if (30 < MouseMove.x) MouseMove.x = 30; // ī�޶� ���� ���� ����

        cameraParentTransform.localEulerAngles = MouseMove;
    }

    void Balance()
    {
        if (Transform.eulerAngles.x != 0 || Transform.eulerAngles.z != 0) // ������ ������ �������� ī�޶� �ٷ����
            Transform.eulerAngles = new Vector3(0, Transform.eulerAngles.y, 0);
    }

    void CameraDistanceCtrl()
    {
        Camera.main.transform.localPosition += new Vector3(0, 0, Input.GetAxisRaw("Mouse ScrollWheel") * 2.0f); // �ٷ� ī�޶� �Ÿ� ����
        if (-1 < Camera.main.transform.localPosition.z)
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, -1);  // ī�޶� �ִ� ���� ��ġ
        else if (Camera.main.transform.localPosition.z < -5)
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, -5);  // ī�޶� �ִ� �� ��ġ
    }
}
