using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public Transform targetTransform;  // 타겟 위치 (Transform)
    public Transform cameraPivot;      // CameraPivot
    public Transform cameraTransform;  // CameraManager

    public LayerMask collisionLayers;

    public Vector3 targetPosition;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public Vector3 cameraInput;

    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;
    public float ZoomSensitivity = 5;
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;
    public float cameraCollisionRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    public float lookAngle; // 좌우
    public float pivotAngle; // 상하
    public float cameraInputX;
    public float cameraInputY;

    private float defaultPosition;

    //public bool isOnShake { set; get; } // 흔들림 효과 재생 체크

    void Start()
    {

    }

    void Awake()
    {
        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    void Update()
    {
        ZoomCamera();

    }

     void LateUpdate()
     {
        HandleAllCameraMovement();

    }

    public void HandleAllCameraMovement()
    {
        if (Inventory.inventoryActivated) return;
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    public void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;
        cameraInputX = Input.GetAxisRaw("Mouse X");
        cameraInputY = Input.GetAxisRaw("Mouse Y");

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

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }

    void ZoomCamera()
    {
        if (Inventory.inventoryActivated) return;
        defaultPosition += Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity;
        if (defaultPosition > -3.0f) defaultPosition = -3.0f;
        if (defaultPosition < -13.0f) defaultPosition = -13.0f;
    }
}
