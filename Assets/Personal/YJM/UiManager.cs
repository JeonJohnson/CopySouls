using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : Manager<UiManager>
{
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
    }
    void TestMakeHpBar()
    {
        for (int i = 0; i < UnitManager.Instance.aliveEnemyList.Count; i++)
        {
            InstantiateHpBar(UnitManager.Instance.aliveEnemyList[i]);
        }
    }

    //UI단축키
    public void UI_KeyboardShortcut()
    {
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

        if (Input.GetKeyDown(KeyCode.Alpha1)) quickSlot1.QuickSlotUse();
        else if (Input.GetKeyDown(KeyCode.Alpha2)) quickSlot2.QuickSlotEquipt();
        else if (Input.GetKeyDown(KeyCode.Alpha3)) quickSlot3.QuickSlotUse();
        else if (Input.GetKeyDown(KeyCode.Alpha4)) quickSlot4.QuickSlotEquipt();
    }

    
}
