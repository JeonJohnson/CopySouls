using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerLocoMove : MonoBehaviour
{
    public GameObject cam;
    [SerializeField] GameObject playerModel;
    [SerializeField] GameObject playerMovemnetSystem;
    [SerializeField] Animator animator;


    public float playerWalkSpeed = 1f;
    public float playerRunSpeed = 1.5f;

    bool isRun = false;

    [SerializeField] float spinPower = 50f;

    [Range(1, 500)] public float mouseSensitivity = 300;
    float rotSpeed;
    float xRot;
    float yRot;
    float playerSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        CameraRot(); // 이동 보정을 위한 임시카메라 코드
    }

    void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 vec = new Vector2(h, v);
        Vector3 dir = new Vector3(vec.x, 0, vec.y);
        dir.Normalize();
        dir = transform.TransformDirection(dir);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
            playerSpeed = Mathf.Lerp(playerSpeed, playerRunSpeed, Time.deltaTime * 10f);
        }
        else
        {
            isRun = false;
            playerSpeed = Mathf.Lerp(playerSpeed, playerWalkSpeed, Time.deltaTime * 10f);
        }

        transform.position += playerSpeed * dir * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
        playerModel.transform.position = this.transform.position;


        Vector3 camClampedAngle(Vector3 vec)
        {
            Vector3 returnVec = new Vector3(vec.x, 0, vec.z);
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
        animator.SetFloat("MoveSpeed", inputValue);
        print(inputValue);
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
}
