using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerLocomove : MonoBehaviour
{
    public bool isMoveable = true;

    public GameObject cam;
    public float walkSpeed = 1f;
    public float runSpeed = 1.5f;
    public float accelSpeed = 80f;

    bool isRun = false;

    [Range(1, 500)] public float mouseSensitivity = 300;

    float rotSpeed;
    float xRot;
    float yRot;
    float curSpeed = 0f;

    GameObject playerModel;

    #region singletone and InitializeState
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
        playerModel = Player.instance.playerModel;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRot(); // 이동 보정을 위한 임시카메라 코드
    }

    public void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 vec = new Vector2(h, v);
        Vector3 dir = new Vector3(vec.x, 0, vec.y);
        dir.Normalize();
        dir = transform.TransformDirection(dir);

        //playerSpeed = Mathf.Lerp(playerSpeed, playerrunSpeed, Time.deltaTime * 10f);

        curSpeed -= Time.deltaTime * accelSpeed / 2;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
            curSpeed = Mathf.Clamp(curSpeed, 0f, runSpeed);
            curSpeed += vec.magnitude * Time.deltaTime * accelSpeed;
        }
        else
        {
            isRun = false;
            if (curSpeed > walkSpeed)
            {
                
            }
            else
            {
                curSpeed = Mathf.Clamp(curSpeed, 0f, walkSpeed);
                curSpeed += vec.magnitude * Time.deltaTime * accelSpeed;
            }
        }

        transform.position += curSpeed * dir * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
        Player.instance.playerModel.transform.position = this.transform.position;

        //플레이어모델 자유이동 보정
        Vector3 camClampedAngle(Vector3 _vec)
        {
            Vector3 returnVec = new Vector3(_vec.x, 0, _vec.z);
            return returnVec;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, camClampedAngle(cam.transform.right), Time.deltaTime * 20f);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, camClampedAngle(-cam.transform.right), Time.deltaTime * 20f);
        }

        if (Input.GetKey(KeyCode.W))
        {
            playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, camClampedAngle(cam.transform.forward), Time.deltaTime * 20f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerModel.transform.forward = Vector3.Slerp(playerModel.transform.forward, camClampedAngle(-cam.transform.forward), Time.deltaTime * 20f);
        }
        SetAnimation();

        if (Mathf.Round(vec.magnitude) <=0)
        {
            Player.instance.SetState(Enums.ePlayerState.Idle);
        }
    }

    void SetAnimation()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float inputValue;

        //print(vec.magnitude);
        if (isRun == true)
        {
            inputValue = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
        }
        else
        {
            inputValue = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v)) / 2;
        }
        Player.instance.animator.SetFloat("MoveSpeed", curSpeed / runSpeed);
    }

    void CameraRot()
    {
        rotSpeed = mouseSensitivity;
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        xRot += h * rotSpeed;
        yRot += v * rotSpeed;

        yRot = Mathf.Clamp(yRot, -90, 90);
        cam.transform.position = this.transform.localPosition;
        cam.transform.eulerAngles = new Vector3(-yRot, xRot, 0);
    }

    public void ResetValue()
    {
        curSpeed = 0f;
        Player.instance.animator.SetFloat("MoveSpeed", 0f);
    }

    public void PlayerControlCam()
    {
        Player.instance.playerMovemnetSystem.transform.position = Player.instance.playerModel.transform.position;
        cam.transform.position = Player.instance.playerModel.transform.position;
    }
}
