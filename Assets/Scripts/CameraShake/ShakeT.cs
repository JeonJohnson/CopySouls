using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeT : MonoBehaviour
{
<<<<<<< Updated upstream:Assets/Scripts/CameraShake/ShakeT.cs
    public CameraShakeEvent data;
    //CameraShake st;

=======
    public CameraShakeEvent dataP;
    public CameraShakeEvent dataR;
>>>>>>> Stashed changes:Assets/Personal/KDM/TestScirpt/ShakeT.cs
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CameraShake.Instance.AddShakeEvent(dataP);
            CameraShake.Instance.AddShakeEvent(dataR);
        }
    }
}
