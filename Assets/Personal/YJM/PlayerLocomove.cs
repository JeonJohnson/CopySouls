using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class PlayerLocomove : MonoBehaviour
{
    public bool isMoveable = true;
    public bool isCameraLock = false;
    public Transform targetEnemy;

    [Header("Stats")]
    [SerializeField]
    float movementSpeed = 1f;
    [SerializeField]
    float runningSpeed = 2f;
    [SerializeField]
    float rotationSpeed = 10f;
    [SerializeField]
    float fallingSpeed = 45f;
    public float moveAmount = 0f;
    float addedSpeed = 0f;
    [SerializeField]
    float accelSpeed = 1f;

    [SerializeField] public bool isRun = false;

    [Range(1, 300)] public float mouseSensitivity = 70;

    CharacterController cc;

    [SerializeField] GameObject playerModel;
    public Transform cameraArm;
    public CameraTest cameraManager;

    #region singletone
    /// <singletone>    
    static public PlayerLocomove instance = null;
    /// <singletone>

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        //LinkCamera();
        cc = GetComponent<CharacterController>();
    }

    #endregion

    void Update()
    {
        SetAnimation();
        KeyInput();
        GiveGravity();
    }

    private void FixedUpdate()
    {
    }


    float yVelocity = 0f;
    void GiveGravity()
    {
        if (cc.isGrounded == false)
        {
            yVelocity += fallingSpeed * Time.deltaTime;
            Vector3 gravityVec = new Vector3(0f, -yVelocity, 0f);
            cc.Move(gravityVec * Time.deltaTime);
        }
        else
        {
            yVelocity = 0f;
        }
    }

    public void LinkCamera()
    {
        cameraManager = GameObject.Find("CamerManager").GetComponent<CameraTest>();
        if(cameraManager == null)
        {
            Debug.LogWarning("Can't Find CameraManager!!");
        }
        cameraArm = cameraManager.transform.Find("CameraPivot").transform;
        //cameraManager.targetTransform = Player.instance.playerModel.transform;
    }

    [HideInInspector]public bool isMove = false;
	public Vector3 moveDir;
    Vector3 normalVector;
    public void Move()
    {
        SprintInput();
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        isMove = moveInput.magnitude != 0;
        moveAmount = Mathf.Clamp01(Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y));
        if (isMove)
        {
            moveDir = cameraArm.forward * moveInput.y;
            moveDir += cameraArm.right * moveInput.x;
            moveDir.Normalize();

            moveDir = new Vector3(moveDir.x, 0f, moveDir.z);

            float speed = movementSpeed + addedSpeed;
            moveDir *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDir, normalVector);
            cc.Move(projectedVelocity * Time.deltaTime);
        }
        else
        {
            moveAmount = 0f;
            Player.instance.SetState(Enums.ePlayerState.Idle);
        }
        SetPlayerTrInputHold();
        if(isCameraLock)
        {
            
        }
        else
        {

        }
    }

    void HandleRotation(float delta, Vector2 moveInput)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = moveAmount;

        targetDir = cameraArm.forward * moveInput.y;
        targetDir += cameraArm.right * moveInput.x;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
            targetDir = transform.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * delta);

        transform.rotation = targetRotation;
    }

    void SprintInput()
    {
        if (Input.GetKey(KeyCode.C))
        {
            addedSpeed += Time.deltaTime * accelSpeed;
            isRun = true;
        }
        else
        {
            addedSpeed -= Time.deltaTime * accelSpeed;
            isRun = false;
        }
        addedSpeed = Mathf.Clamp(addedSpeed, 0, runningSpeed);
    }

    void SetAnimation()
    {
        if (isMove == true)
        {
            Player.instance.animator.applyRootMotion = false;
            Player.instance.animator.SetFloat("MoveSpeed", (movementSpeed + addedSpeed) / (movementSpeed + runningSpeed));
        }
        else
        {
            Player.instance.animator.applyRootMotion = true;
            Player.instance.animator.SetFloat("MoveSpeed", 0f);
        }
    }

    void KeyInput()
    {
        if (Inventory.inventoryActivated) return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isCameraLock == false)
            {
                print("앍");
                SearchTarget();
                isCameraLock = true;
            }
            else
            {
                isCameraLock = false;
            }
        }

        if (isCameraLock == true)
        {
            CameraLock();
        }
        else
        {
            //CameraRot();
            //cameraManager.RotateCamera();
        }
    }

    void SearchTarget()
    {
        float distance = float.MaxValue;
        for(int i =0; i < UnitManager.Instance.aliveEnemyList.Count; i++)
        {
            if(Vector3.Distance(UnitManager.Instance.aliveEnemyList[i].transform.position, this.transform.position) < distance)
            {
                distance = Vector3.Distance(UnitManager.Instance.aliveEnemyList[i].transform.position, this.transform.position);
                targetEnemy = UnitManager.Instance.aliveEnemyList[i].transform;
                print(UnitManager.Instance.aliveEnemyList[i]);
            }
        }
    }

    void CameraLock()
    {
        if (targetEnemy != null)
        {
            isCameraLock = false;
            return;
        }
        Vector3 lookDir = targetEnemy.transform.position - cameraArm.transform.position;
        lookDir.y = 0f;

        cameraArm.transform.forward = Vector3.Slerp(cameraArm.transform.forward, lookDir.normalized, Time.deltaTime * 10f);
        cameraManager.lookAngle = cameraArm.transform.eulerAngles.y;
        cameraManager.pivotAngle = cameraArm.transform.localEulerAngles.z;
    }

    public void ResetValue()
    {
        moveAmount = 0f;
        isMove = false;
        Player.instance.animator.SetFloat("MoveSpeed", 0f);
        Player.instance.animator.applyRootMotion = true;
    }

    public void SetPlayerTrImt()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
        moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
        playerModel.transform.forward = moveDir;
    }

    public void SetPlayerTrSlow(bool isCamLocked = false, float timer = 0.2f)
    {
            StartCoroutine(SetPlayerTrSlowCoro(isCamLocked, timer));
    }

    IEnumerator SetPlayerTrSlowCoro(bool isCamLocked = false, float _timer = 0.2f)
    {
        while (_timer >= 0f)
        {
            if (isCamLocked == false)
            {
                _timer -= Time.deltaTime;
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
                moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
                if(moveDir ==Vector3.zero)
                {
                    moveDir = playerModel.transform.forward;
                }
                playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, moveDir, Time.deltaTime * 20f);
                yield return null;
            }
            else
            {
                _timer -= Time.deltaTime;
                Vector3 lookDir = targetEnemy.transform.position - playerModel.transform.position;
                lookDir.y = 0f;
                //playerModel.transform.forward = lookDir.normalized;
                playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, lookDir, Time.deltaTime * 20f);
                yield return null;
            }
        }
    }

    void ChangeModelTrInput()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (isCameraLock == true)
        {
            Vector3 lookDir = targetEnemy.transform.position - this.transform.position;
            playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, lookDir, Time.deltaTime * 20f);
        }
        else if (isCameraLock == false)
        {
            HandleRotation(Time.deltaTime, moveInput);
        }
    }

    float inputTimer = 0.2f;
    Coroutine SetPlayerTrInputCoro;
    bool isInput = false;

    void SetPlayerTrInputHold()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (inputTimer >= 0f && !isCameraLock)
        {
            SetPlayerTrInputCoro = StartCoroutine(SetPlayerTr());
        }
        else
        {
            StopCoroutine(SetPlayerTrInputCoro);
            isInput = false;
            ChangeModelTrInput();
        }

        if (moveInput != Vector2.zero)
        {
            inputTimer -= Time.deltaTime;
        }
        else
        {
            inputTimer = 0.2f;
        }
    }

    IEnumerator SetPlayerTr()
    {
        float _timer = 0.15f;
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
        while (_timer >= 0f)
        {
                _timer -= Time.deltaTime;
                moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
                playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, moveDir, Time.deltaTime * 20f);
                yield return null;
        }
        isInput = false;
    }
}
