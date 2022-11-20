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

    //UI����Ű
    public void UI_KeyboardShortcut()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Inventory.inventoryActivated && DivisionProcess.DivisionActivated)
            {
                //����â ����
                //Inventory.Instance.Exit_Division_Button();
            }
            //else if (Inventory.inventoryActivated && !Inventory.DivisionActivated)
            //{
            //    //�κ��丮 â ����
            //    Inventory.Instance.Exit_Inventory_Button();
            //}
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            //���� ENTER����
            //Inventory.Instance.Division_Button();
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1)) Inventory.Instance.QuickSlotUse(quickSlot1);
        //else if (Input.GetKeyDown(KeyCode.Alpha2)) Inventory.Instance.QuickSlotUse(quickSlot2);
        //else if (Input.GetKeyDown(KeyCode.Alpha3)) Inventory.Instance.QuickSlotUse(quickSlot3);
        //else if (Input.GetKeyDown(KeyCode.Alpha4)) Inventory.Instance.QuickSlotUse(quickSlot4);
    }



    void TestMakeHpBar()
    {
        for(int i = 0; i < UnitManager.Instance.aliveEnemyList.Count; i++)
        {
            InstantiateHpBar(UnitManager.Instance.aliveEnemyList[i]);
        }
    }
}
