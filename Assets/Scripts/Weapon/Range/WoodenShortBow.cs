using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBowState
{ 
    Idle, 
    Pull, //Ȱ ���� ���� ���� ��
    Hook, //���� �� ����� ��
    Return, //Ȱ ���� ������ ��
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
