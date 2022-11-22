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

    //UI단축키


    //ui중에 하나라도 켜지면 마우스 활성화
    //
    public void UI_KeyboardShortcut()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Inventory.Instance.TryOpenInventory();
            //플레이어 뭔 제약들 ...
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            //마우스 나오게
            //if(!Inventory.inventoryActivated 
            //    && !DivisionProcess.DivisionActivated 
            //    && !SelectionProcess.SelectionActivated 
            //    && !ThrowingProcess.ThrowingActivated)
            Player.instance.ActivatePlayerInput(false);
            Debug.Log("sgsgfhfdh");

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Inventory.inventoryActivated && DivisionProcess.DivisionActivated)
            {
                //분할창 끄기
                Inventory.Instance.DivisionParent.Button_DivisionCancel();
            }
            else if (Inventory.inventoryActivated && ThrowingProcess.ThrowingActivated)
            {
                //버리기 창 끄기
                Inventory.Instance.ThrowingParent.Button_ThrowCancel();
            }
            else if (Inventory.inventoryActivated && !DivisionProcess.DivisionActivated && !ThrowingProcess.ThrowingActivated)
            {
                //인벤토리 창 끄기
                Inventory.Instance.Button_InventoryExit();
                if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if(DivisionProcess.DivisionActivated)
            {
                //분할 ENTER적용
                Inventory.Instance.DivisionParent.Button_Division();
            }
            else if (ThrowingProcess.ThrowingActivated)
            {
                //버리기 ENTER적용
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
