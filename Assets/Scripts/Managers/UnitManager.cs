using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class UnitManager : Manager<UnitManager>
{
    //// <Player>
    public GameObject playerObj;



    //// <Player>



    //// <EnemyVar>
    public int enemyAllCount;
    public Dictionary<eEnemyName, List<Enemy>> enemyDic;



    //// <EnemyVar>


    //// <EnemyFuncs>
    public GameObject SpawnEnemy(eEnemyName name, Vector3 pos, Vector3 rot, int count)
    {


        return null;
    }
    //// <EnemyFuncs>



    private void Awake()
    {
        enemyDic = new Dictionary<eEnemyName, List<Enemy>>();

        for (int i = 0; i < (int)eEnemyName.End; ++i)
        {
            enemyDic.Add((eEnemyName)i, new List<Enemy>());
        }



    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
