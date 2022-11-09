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

    protected override void weaponInitialize()
    {
    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
