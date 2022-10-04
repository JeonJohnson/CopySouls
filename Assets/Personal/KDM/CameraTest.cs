using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public Transform targetTransform;  // 타겟 위치 (Transform)
    public Transform cameraPivot;      
    public Transform cameraTransform;  // CameraManager

    public LayerMask collisionLayers;

    public Vector3 targetPosition;

    private Vector3 cameraFollowVelcocity = Vector3.zero;
    private Vector3 cameraVectorPosition;


    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;
    public float cameraCollisionRadius = 2;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    //public float wheelSpeed = 10;
    //public float distance; // 앞뒤
    public float lookAngle; // 좌우
    public float pivotAngle; // 상하
    public Vector3 cameraInput;
    public float cameraInputX;
    public float cameraInputY;
    public float cameraInputZ;

    private float defaultPosition;

    void Start()
    {

    }

    void Awake()
    {
        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
        cameraInputZ = cameraInput.z;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    void Update()
    {
        CameraDistanceCtrl();
    }

     void LateUpdate()
     {
        HandleAllCameraMovement();
     }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelcocity, cameraFollowSpeed);

        transform.position = targetPosition;

    }

    public void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;
        cameraInputX = Input.GetAxisRaw("Mouse X");
        cameraInputY = Input.GetAxisRaw("Mouse Y");
        //cameraInputZ = Input.GetAxisRaw("Mouset ScrollWheel");

        lookAngle = lookAngle + (cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;


    }

    void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = targetPosition - (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
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
