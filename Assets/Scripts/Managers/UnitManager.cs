using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using Enums;

public class UnitManager : Manager<UnitManager>
{
    //오브젝트 풀링에서 빌려오기?
    //아니면 유닛들은 여기서 관리하기?

    //// <Player>
    public GameObject playerObj;



    //// <Player>



    //// <EnemyVar>
    public int enemyAllCount;
    public List<GameObject> enemyPrefabList;
    public Dictionary<eEnemyName, List<Enemy>> enemyDic;



    //// <EnemyVar>


    //// <EnemyFuncs>
    public GameObject SpawnEnemy(eEnemyName name, Vector3 pos, Vector3 rot, int count = 1)
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
