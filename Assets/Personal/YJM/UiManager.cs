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

    private void Start()
    {
        
    }

    void TestMakeHpBar()
    {
        for(int i = 0; i < UnitManager.Instance.enemyPrefabList.Count; i++)
        {
            InstantiateHpBar(UnitManager.Instance.enemyPrefabList[i].GetComponent<Enemy>());
        }
    }
}
