using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;

//���� ���� ��������

// ���Ӱ���? ������ ��� -> ����
//    public float curActAtkValue = 1.0f;
// ���� -> ������ ó�� ()
// 
// 

public class Spirit : Enemy
{
    //enemy -> player (Att)���̾�
    public LayerMask player_Hitbox;

    public int preHp;
    public Material Material_Disable;
    public Material Material_Standard;
    public float initFOVAngle;

    public eSpiritState curState_e;
    public bool Arrival;

    //public Transform[] PatrolPos;
    public int curPatrol_Index;
    public int prePatrol_Index;

    public bool complete_Equipt;
    public bool complete_Unequipt;
    public bool complete_Atk;
    public bool complete_AttReturn;
    public bool complete_Damaged;
    public bool isEquipt;
    public bool atting;
    public bool existRemainder;
    public bool transWeaponPos;
    public int HitCount;

    //step
    public bool stepWait;
   
    public override void InitializeState()
	{
        fsm = new cState[(int)Enums.eSpiritState.End];

        fsm[(int)Enums.eSpiritState.Idle] = new Spirit_Idle();
        fsm[(int)Enums.eSpiritState.Patrol] = new Spirit_Patrol();
        fsm[(int)Enums.eSpiritState.Equipt] = new Spirit_Equipt();
        fsm[(int)Enums.eSpiritState.Unequipt] = new Spirit_Unequipt();
        fsm[(int)Enums.eSpiritState.Trace] = new Spirit_Trace();
        fsm[(int)Enums.eSpiritState.Atk] = new Spirit_Atk();
        fsm[(int)Enums.eSpiritState.Damaged] = new Spirit_Damaged();
        fsm[(int)Enums.eSpiritState.Death] = new Spirit_Death();

        SetState((int)Enums.eSpiritState.Idle);
	}

	protected override void Awake()
    {
        base.Awake();
        initFOVAngle = GetComponent<FieldOfView>().viewAngle;
    }

    protected override void Start()
    {
        base.Start();
        targetObj = GameObject.Find("Player");
        weapon.Type = eWeaponType.Melee;

        player_Hitbox = 1 << LayerMask.NameToLayer("Player_Hitbox");
    }

    protected override void Update()
    {
        base.Update();
        curState_e = GetCurState<Enums.eSpiritState>();

        if (Input.GetKeyDown(KeyCode.C))
        {
            preHp = status.curHp;
            status.curHp--;
            HitCount++;
        }

        if (status.curHp <= 0)
        {
            status.isDead = true;
            SetState((int)Enums.eSpiritState.Death);
        }
        else if(status.curHp > 0 && !status.isDead)
        {
            if (HitCount > 0 && curState_e != eSpiritState.Damaged)
            {
                SetState((int)Enums.eSpiritState.Damaged);
            }
        }

        //������ ������ ����������
        if (curState_e == eSpiritState.Atk || curState_e == eSpiritState.Trace)
        {
            if (weaponEquipState == eEquipState.None) SetState((int)Enums.eSpiritState.Idle);
        }
    }
    //=============================================================================
    // MaterialChange�Լ�
    //=============================================================================
    public void Material_Change(Material material)
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material = material;
        //GetComponentInChildren<SkinnedMeshRenderer>().materials[0] = material;
    }

    //=============================================================================

    //=============================================================================
    //instance
    //=============================================================================

    public void CreateRemainderWeapon(Transform trans)
    {
        if (!existRemainder)
        {
            existRemainder = true;
            //GameObject obj = Instantiate(remainderWeapon);
            //obj.transform.position = trans.position;
            //obj.transform.rotation = trans.rotation;
        }
        else return;
    }

    //=============================================================================
    //�ִϸ��̼� �̺�Ʈ

    public void Spirit_StepWait() { if (!stepWait) stepWait = true; }
    public void Spirit_StepStart() { if (stepWait) stepWait = false; }
    public void Spirit_Melee_CompleteEquiptment() { complete_Equipt = true; }
    public void Spirit_Melee_CompleteUnequiptment() { complete_Unequipt = true; }
    public void Spirit_Melee_CompleAtk() { complete_Atk = true; }
    //public void Spirit_AttReturn() { if (!complete_AttReturn) complete_AttReturn = true; }
    //public void Spirit_Melee_CompleCombo1() { complete_Combo1 = true; }
    public void Spirit_Damaged() { complete_Damaged = true; }
    public void Spirit_Atting()
    {
        if(curState_e == Enums.eSpiritState.Atk)
        {
            if (!atting) atting = true;
            else atting = false;
        }
    }
    public void Spirit_WeaponTransPos()
    {
        if (curState_e == Enums.eSpiritState.Atk)
        {
            if (!transWeaponPos) transWeaponPos = true;
            else transWeaponPos = false;
        }
    }
    //=============================================================================
}



