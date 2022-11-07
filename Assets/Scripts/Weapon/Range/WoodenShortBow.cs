using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBowState
{ 
    Idle, 
    Pull, //활 시위 당기는 중일 때
    Hook, //시위 다 당겼을 때
    Return, //활 시위 놓았을 때
    End
}
	
public class WoodenShortBow : Weapon
{
    public Arrow arrow;

    public eBowState state;

    public Transform leverTr;
    public Transform stringTr;

    public Animator animCtlr;
    public float pullingAnimSpd;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
