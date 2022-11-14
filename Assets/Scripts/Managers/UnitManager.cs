﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using Enums;

public class UnitManager : Manager<UnitManager>
{
    //오브젝트 풀링에서 빌려오기?
    //아니면 유닛들은 여기서 관리하기?

    //// <Player>
    [Header("Player Vars")]
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject cameraPrefab;
    private Player playerScript;
    private PlayerActionTable playerActTable;

	public GameObject GetPlayerObj
    {
        get
        {
            if (playerScript == null)
            {
                CreatePlayer();
            }
            return playerScript.gameObject;
        }
    }

    public Player GetPlayerScript
    {
        get
        {
            if (playerScript == null)
            {
                CreatePlayer();
            }
            return playerScript;
        }
    }

    public PlayerActionTable GetPlayerActTable
    {
        get
        {
            if (playerScript == null)
            {
                CreatePlayer();
            }
            return playerActTable;
        }
    }

    private void CreatePlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        GameObject cameraObj = GameObject.Find("CameraManager");

        if (playerObj == null && playerPrefab)
        {
            //난중에 여기서 카메라들도 세팅해주셈
            playerObj = Instantiate(playerPrefab);
            
        }

        if (cameraObj == null && cameraPrefab)
        {
            cameraObj = Instantiate(cameraPrefab);
        }

        playerScript = playerObj.GetComponent<Player>();
        cameraObj.GetComponent<CameraTest>().targetTransform = playerScript.playerModel.transform;
        playerActTable = playerScript.playerAt;
        playerScript.playerLocomove.cameraArm = cameraObj.transform.GetChild(0).gameObject.transform;
        playerScript.playerLocomove.cameraManager = cameraObj.GetComponent<CameraTest>();
    }
    //// <Player>



    //// <EnemyVar>
    [Header("Enemy Vars")]
    public int enemyAllCount;
    
   //public List<GameObject> enemyPrefabList;
    public List<GameObject> enemyRagdollPrefabList;
    public Transform ragdollBox;

    public Dictionary<eEnemyName, List<Enemy>> enemyDic;
    public List<Enemy> allEnemyList = new List<Enemy>();




  
    //// <EnemyVar>


    //// <EnemyFuncs>
    public GameObject SpawnEnemy(eEnemyName name, Vector3 pos, Vector3 rot, int count = 1)
    {

        


        return null;
    }
    private void SearchEnemy()
    {
        print("모든 적군 써치");

        GameObject[] allEnemyGoList = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < allEnemyGoList.Length; i++)
        {
            Enemy enemy = allEnemyGoList[i].GetComponent<Enemy>();

            SetEnemiesRagdoll(ref enemy);

            if (enemy != null && enemy.gameObject.activeSelf &&!allEnemyList.Contains(enemy) )
            {
                allEnemyList.Add(allEnemyGoList[i].GetComponent<Enemy>());
            }
        }
    }

    private void SetEnemiesRagdoll(ref Enemy script)
    {
        if (!script.ragdoll)
        {
            GameObject ragdollObj = Instantiate(enemyRagdollPrefabList[(int)script.status.name_e], ragdollBox);
            script.ragdoll = ragdollObj.GetComponent<Enemy_Ragdoll>();
            ragdollObj.SetActive(false);
        }
    }
    //// <EnemyFuncs>
    
    /// <TestEnemy>
    public GameObject testEmptyEnemyPrefab;
    public GameObject SpawnTestEnemy(Vector3 pos)
    {
        GameObject testObj = Instantiate(testEmptyEnemyPrefab, pos, Quaternion.identity);

        Enemy tempScript = testObj.GetComponent<Enemy>();

        allEnemyList.Add(tempScript);

        return testObj;
    }
    /// </TestEnemy>



    private void Awake()
    {
        enemyDic = new Dictionary<eEnemyName, List<Enemy>>();

        for (int i = 0; i < (int)eEnemyName.End; ++i)
        {
            enemyDic.Add((eEnemyName)i, new List<Enemy>());
        }
        ragdollBox = Funcs.CheckGameObjectExist("RagdollBox").transform;

        

        CreatePlayer();
        SearchEnemy();

    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
