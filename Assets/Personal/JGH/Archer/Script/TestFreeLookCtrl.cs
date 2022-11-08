using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFreeLookCtrl : MonoBehaviour
{

    public float mouseSpd;
    private Vector3 camRotate;
    public GameObject cam;

    public float moveSpd;
    Rigidbody rd;

    public Transform headTr;
    public Transform spineTr;

    private Vector2 MouseInput()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        return new Vector2(x, y);
    }

    public void CamMove(Vector2 input, float spd)
    {
        Vector3 mouseDir = new Vector3(-input.y, input.x, 0f);
        camRotate += mouseDir * spd * Time.deltaTime;

        camRotate.x = Mathf.Clamp(camRotate.x, -90f, 90f);

        transform.eulerAngles = camRotate;
    }



    public void PlayerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(x, 0, z);

        dir = Camera.main.transform.TransformDirection(dir);

        transform.position += dir.normalized * Time.deltaTime * moveSpd;
        
    }

	public void Awake()
	{

        Cursor.lockState = CursorLockMode.Locked;
	}

	// Start is called before the first frame update
	void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

	public void LateUpdate()
	{
        CamMove(MouseInput(), mouseSpd);
        
    }
}
