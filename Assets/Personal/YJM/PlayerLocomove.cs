using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerLocomove : MonoBehaviour
{
    public bool isMoveable = true;

    public float walkSpeed = 1f;
    public float runSpeed = 1.5f;
    public float accelSpeed = 80f;
    public float gravity = -5f;

    bool isRun = false;

    [Range(1, 300)] public float mouseSensitivity = 70;

    float rotSpeed;
    float xRot;
    float yRot;
    float curSpeed = 0f;

    [SerializeField] GameObject playerModel;
    [SerializeField] Transform cameraArm;
    [SerializeField] Transform headPos;

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
        CameraRot();
        SetAnimation();
        print(curSpeed);
    }

    bool isMove = false;
    public void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        isMove = moveInput.magnitude != 0;
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, moveDir, Time.deltaTime * 20f);
            playerModel.transform.position = this.transform.position;
            transform.position += moveDir.normalized * Time.deltaTime * curSpeed;
        }

        SprintInput();
    }

    void SprintInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curSpeed += Time.deltaTime * accelSpeed;
        }
        else
        {
            curSpeed -= Time.deltaTime * accelSpeed;
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

    void CameraRot()
    {
        rotSpeed = mouseSensitivity;
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        xRot += h * rotSpeed;
        yRot += v * rotSpeed;

        yRot = Mathf.Clamp(yRot, -90, 90);
        cameraArm.transform.eulerAngles = new Vector3(-yRot, xRot, 0);
    }

    public void ResetValue()
    {
        curSpeed = 0f;
        Player.instance.animator.SetFloat("MoveSpeed", 0f);
    }

    public void PlayerControlCam()
    {
        cameraArm.transform.position = headPos.transform.position;
    }

    public void PlayerPosFix()
    {
        transform.position = playerModel.transform.position;
        playerModel.transform.localPosition = Vector3.zero;
        cameraArm.transform.localPosition = headPos.transform.localPosition;
    }
}
