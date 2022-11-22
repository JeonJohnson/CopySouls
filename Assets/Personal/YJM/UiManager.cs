using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : Manager<UiManager>
{
    public static bool UIActivated = false;

    [SerializeField] GameObject playerStatusUi;
    [SerializeField] GameObject hpBarUi;

    [SerializeField] GameObject hpBarPrefab;

    [SerializeField] public QuickSlot quickSlot1;
    [SerializeField] public QuickSlot quickSlot2;
    [SerializeField] public QuickSlot quickSlot3;
    [SerializeField] public QuickSlot quickSlot4;


    public void InstantiateHpBar(Enemy target)
    {
        GameObject hpBarGo = Instantiate(hpBarPrefab,hpBarUi.transform);
        HpBar hpBar = hpBarGo.GetComponent<HpBar>();
        hpBar.target = target;
    }

    private void Awake()
    {
        TestMakeHpBar();
    }

    private void Start()
    {

    }
    private void Update()
    {
        UI_KeyboardShortcut();
        if(UIActivated) Player.instance.ActivatePlayerInput(false);
        else Player.instance.ActivatePlayerInput(true);
    }
    void TestMakeHpBar()
    {
        for (int i = 0; i < UnitManager.Instance.aliveEnemyList.Count; i++)
        {
            InstantiateHpBar(UnitManager.Instance.aliveEnemyList[i]);
        }
    }

    //UI����Ű
    //�κ� ���â �����ų� alt������ ���콺 Ȱ��ȭ
    //�κ��̳� ���â�� �����ų� alt�ѹ� �� ������ ���콺 ��Ȱ��ȭ

    public void UI_KeyboardShortcut()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (!UIActivated) UIActivated = true;
            else UIActivated = false;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Inventory.Instance.InventoryComand();
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            Inventory.Instance.TryOpenInventory();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            EquipmentWindow.Instance.TryOpenEquiptment();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Inventory.inventoryActivated && DivisionProcess.DivisionActivated)
            {
                //����â ����
                Inventory.Instance.DivisionParent.Button_DivisionCancel();
            }
            else if (Inventory.inventoryActivated && ThrowingProcess.ThrowingActivated)
            {
                //������ â ����
                Inventory.Instance.ThrowingParent.Button_ThrowCancel();
            }
            else if (Inventory.inventoryActivated && !DivisionProcess.DivisionActivated && !ThrowingProcess.ThrowingActivated)
            {
                //�κ��丮 â ����
                Inventory.Instance.Button_InventoryExit();
                if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            }
            else if(EquipmentWindow.EquipmentActivated)
            {
                EquipmentWindow.Instance.TryOpenEquiptment();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if(DivisionProcess.DivisionActivated)
            {
                //���� ENTER����
                Inventory.Instance.DivisionParent.Button_Division();
            }
            else if (ThrowingProcess.ThrowingActivated)
            {
                //������ ENTER����
                Inventory.Instance.ThrowingParent.Button_Throw();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            quickSlot1.QuickSlotUse();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            quickSlot2.QuickSlotEquipt();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            quickSlot3.QuickSlotUse();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            quickSlot4.QuickSlotEquipt();
        }
    }

    
}
