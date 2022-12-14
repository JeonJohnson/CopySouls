using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : Manager<CameraTest>
{
    public Transform targetTransform;  // TargetTransform(Transform)
    public Transform cameraPivot;      // CameraPivot
    public Transform cameraTransform;  // CameraManager

    public LayerMask collisionLayers;

    public Vector3 targetPosition;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public Vector3 cameraInput;

    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed;
    //public float cameraPivotSpeed = 2;
    public float ZoomSensitivity = 5;
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;
    public float cameraCollisionRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    public float lookAngle; // left,right
    public float pivotAngle; // up,down
    public float cameraInputX;
    public float cameraInputY;

    private float defaultPosition;

    public Vector3 CameraVectorPosition { get{ return cameraVectorPosition; } }

    void Awake()
    {

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;

    }

    void Start()
    {
        cameraLookSpeed = GameManager.Instance.mouseSensivility;
    }

    void Update()
    {
        ZoomCamera();
    }

     void LateUpdate()
     {
        HandleAllCameraMovement();
        if (Input.GetKeyDown(KeyCode.B))
        {
            CameraEffect.instance.PlayRollAttEffect();
        }
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        if (!Player.instance.status.isInputtable) return;
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
        pivotAngle = pivotAngle - (cameraInputY * cameraLookSpeed);
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

    public void ZoomCamera()
    {
        if (Inventory.inventoryActivated) return;
        defaultPosition += Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity;
        if (defaultPosition > -1.0f) defaultPosition = -1.0f;
        if (defaultPosition < -16.0f) defaultPosition = -16.0f;
    }
}
