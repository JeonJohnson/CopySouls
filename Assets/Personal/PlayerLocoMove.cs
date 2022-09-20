using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerLocoMove : MonoBehaviour
{
    public GameObject cam;
    [SerializeField] GameObject playerModel;
    [SerializeField] GameObject playerMovemnetSystem;

    public float playerSpeed = 1f;

    [Range(1, 500)] public float mouseSensitivity = 300;
    float rotSpeed;
    float xRot;
    float yRot;

    // Start is called before the first frame update
    void Start()
    {
        corYValue = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Floor(Input.GetAxisRaw("Horizontal") * 100) != 0 || Mathf.Floor(Input.GetAxisRaw("Vertical") * 100) != 0)
        {
            PlayerMove();
        }
        PlayerRot(); // 이동 보정을 위한 임시카메라 코드
    }

    float corYValue;
    void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 vec = new Vector2(h, v);
        Vector3 dir = new Vector3(vec.x, 0, vec.y);
        dir.Normalize();
        dir = transform.TransformDirection(dir);
        transform.position += playerSpeed * dir * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);


        playerModel.transform.position = this.transform.position;
        playerModel.transform.eulerAngles = transform.eulerAngles;
    }


    void PlayerRot()
    {
        rotSpeed = mouseSensitivity;
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        xRot += h * rotSpeed;
        yRot += v * rotSpeed;

        yRot = Mathf.Clamp(yRot, -90, 90);
        cam.transform.position = this.transform.position;
        cam.transform.eulerAngles = new Vector3(-yRot, xRot, 0);
    }
}
