using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    Rigidbody rigid;
    Vector3 moveVec;
    public float x;
    public float z;
    public float speed;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();   
    }
    void Start()
    {
        
    }

    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        moveVec = new Vector3(x, 0, z);
        transform.position += (moveVec.normalized * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
    }
}
