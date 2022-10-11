using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerLocomove : MonoBehaviour
{
    public bool isMoveable = true;
    public bool isCameraLock = false;
    public Transform targetEnemy;

    public float walkSpeed = 1f;
    public float runSpeed = 1.5f;
    public float accelSpeed = 80f;
    public float gravity = -5f;

    [SerializeField] public bool isRun = false;

    [Range(1, 300)] public float mouseSensitivity = 70;

    public float curSpeed = 0f;

    [SerializeField] GameObject playerModel;
    [SerializeField] Transform cameraArm;
    //[SerializeField] Transform headPos;
    [SerializeField] CameraTest cameraManager;

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
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
        KeyInput();
    }

    [HideInInspector]public bool isMove = false;
    Vector3 moveDir;

    public void Move()
    {
        SprintInput();
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        isMove = moveInput.magnitude != 0;
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
            moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            transform.position += moveDir.normalized * Time.deltaTime * curSpeed;
        }
        else
        {
            ResetValue();
            Player.instance.SetState(Enums.ePlayerState.Idle);
        }

        playerModel.transform.position = this.transform.position;
        SetPlayerTrInputHold();
    }

    void ChangeModelTrInput()
    {
        if (isCameraLock == true)
        {
            Vector3 lookDir = targetEnemy.transform.position - playerModel.transform.position;
            lookDir.y = 0f;
            playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, lookDir.normalized, Time.deltaTime * 10f);
        }
        else if (isCameraLock == false)
        {
            Vector3 lookDir = moveDir;
            playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, lookDir, Time.deltaTime * 20f);
        }
    }

    void SprintInput()
    {
        if (Input.GetKey(KeyCode.C))
        {
            curSpeed += Time.deltaTime * accelSpeed;
            isRun = true;
        }
        else
        {
            curSpeed -= Time.deltaTime * accelSpeed;
            isRun = false;
        }
        curSpeed = Mathf.Clamp(curSpeed, walkSpeed, runSpeed);
    }

    void SetAnimation()
    {
        if (isMove == true)
        {
            Player.instance.animator.SetFloat("MoveSpeed", curSpeed / runSpeed);
        }
        else
        {
            Player.instance.animator.SetFloat("MoveSpeed", 0);
        }
    }

    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isCameraLock == false && targetEnemy != null)
            {
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
            cameraManager.RotateCamera();
        }
    }

    void CameraLock()
    {
        Vector3 lookDir = targetEnemy.transform.position - cameraArm.transform.position;
        lookDir.y = 0f;

        cameraArm.transform.forward = Vector3.Slerp(cameraArm.transform.forward, lookDir.normalized, Time.deltaTime * 10f);
        cameraManager.lookAngle = cameraArm.transform.eulerAngles.y;
        cameraManager.pivotAngle = cameraArm.transform.localEulerAngles.z;
    }

    public void ResetValue()
    {
        curSpeed = 0f;
        Player.instance.animator.SetFloat("MoveSpeed", 0f);
    }


    public void PlayerPosFix()
    {
        transform.position = playerModel.transform.position;
        playerModel.transform.localPosition = Vector3.zero;
        //cameraArm.transform.localPosition = headPos.transform.localPosition;
    }

    public void SetPlayerTrImt()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
        moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
        playerModel.transform.forward = moveDir;
    }

    public void SetPlayerTrSlow(bool isCamLocked = false)
    {
            StartCoroutine(SetPlayerTrSlowCoro(isCamLocked));
    }

    IEnumerator SetPlayerTrSlowCoro(bool isCamLocked = false)
    {
        float _timer = 0.2f;
        while (_timer >= 0f)
        {
            if (isCamLocked == false)
            {
                _timer -= Time.deltaTime;
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
                moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
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

    float inputTimer = 0.2f;
    Coroutine SetPlayerTrInputCoro;
    bool isInput = false;

    void SetPlayerTrInputHold()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (inputTimer >= 0f)
        {
            if (isInput == false)
            {
                SetPlayerTrInputCoro = StartCoroutine(SetPlayerTr());
                isInput = true;
            }
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
