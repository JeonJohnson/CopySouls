using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBowState
{ 
    Idle,
    //Hook, //ȭ�� ������ �ɾ��� ��
    Pull, //Ȱ ���� ���� ��
    //Full, //���� �� ����� ��
    Shoot, //Ȱ ���� ������ ��
    End
}
	
public class WoodenShortBow : Weapon, IPoolingObject
{
    public Arrow arrow;

    public eBowState state = eBowState.Idle;

    public Animator animCtlr;
    public float pullingAnimSpd;
    
    public Transform leverTr;
    public Transform stringTr;

    

    public void ResetForReturn()
    {
        ((IPoolingObject)arrow).ResetForReturn();

    }


    protected override void weaponInitialize()
    {
        animCtlr = GetComponent<Animator>();
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

		switch (state)
		{
			case eBowState.Idle:
				break;
			case eBowState.Pull:
				break;
			case eBowState.Shoot:
				break;
			case eBowState.End:
				break;
			default:
				break;
		}
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
