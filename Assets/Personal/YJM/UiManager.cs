using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : Manager<UiManager>
{
    [SerializeField] GameObject playerStatusUi;
    [SerializeField] GameObject hpBarUi;

    [SerializeField] GameObject hpBarPrefab;

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

    //UI단축키
    public void UI_KeyboardShortcut()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Inventory.inventoryActivated && Inventory.DivisionActivated)
            {
                //분할창 끄기
                Inventory.Instance.Exit_Division_Button();
            }
            else if (Inventory.inventoryActivated && !Inventory.DivisionActivated)
            {
                //인벤토리 창 끄기
                Inventory.Instance.Exit_Inventory_Button();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            //분할 ENTER적용
            Inventory.Instance.Division_Button();
        }
    }



    void TestMakeHpBar()
    {
        for(int i = 0; i < UnitManager.Instance.aliveEnemyList.Count; i++)
        {
            InstantiateHpBar(UnitManager.Instance.aliveEnemyList[i]);
        }
    }
}
