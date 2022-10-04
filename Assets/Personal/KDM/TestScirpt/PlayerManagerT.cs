using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerT : MonoBehaviour
{
    CameraTest0 cameraTest0;

    private void Awake()
    {
        cameraTest0 = FindObjectOfType<CameraTest0>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        cameraTest0.HandleAllCameraMovement();
    }
}
