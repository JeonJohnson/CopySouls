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

    public Dictionary<eEnemyName, List<Enemy>> aliveEnemyDic; //얘는 잘 안쓸꺼 같지만 살아있는 애만
    public List<Enemy> allEnemyList = new List<Enemy>(); //ㄹㅇ 맵상의 죽었든 살았든 모든 Enemy
    public List<Enemy> aliveEnemyList = new List<Enemy>(); //현재 살아있는 Enemy만

    public Enemy boss_Golem;
    //// <EnemyVar>


    //// <EnemyFuncs>
    public GameObject SpawnEnemy(eEnemyName name, Vector3 pos, Vector3 rot, int count = 1)
    {

        


        return null;
    }

    public void ResetAllEnemies()
	{
		foreach (Enemy enemy in allEnemyList)
		{
			enemy.ResetEnemy();
            
            GameObject newObj = null;
            Enemy newEnemy = null;

            if (enemy.status.name_e != eEnemyName.Golem)
            {
     
                switch (enemy.status.name_e)
                {
                    case eEnemyName.Spirit:
                        {
                            newObj = ObjectPoolingCenter.Instance.LentalObj("Skeleton_Spirit");
                        }
                        break;
                    case eEnemyName.Archer:
                        {
                            newObj = ObjectPoolingCenter.Instance.LentalObj("Skeleton_Archer");
                        }
                        break;
                    default:
                        break;
                }

                if (newObj != null)
                {
                    newEnemy = newObj.GetComponent<Enemy>();
                }
                else
                {
                    Debug.LogError("EnemyObj is null at ResetAllenemy in UnitManager.");
                    return;
                }

                //newEnemy.navAgent.enabled = false;
                //newEnemy.gameObject.transform.position = enemy.initPos;
                //newEnemy.gameObject.transform.forward = enemy.initForward;
                //newEnemy.navAgent.enabled = true;

                //newEnemy.SetInitTr(enemy.initPos, enemy.initForward);
                //newObj.SetActive(true);

                if (enemy.hpBar)
                {
                    enemy.hpBar.DestorySceneReset();
                }

            }
            else
            {
				//골렘용 초기화
				//boss hpbar

				if (enemy.combatState != eCombatState.Idle)
				{
					newObj = ObjectPoolingCenter.Instance.LentalObj("Golem");

                    if (newObj != null)
                    {
                        newEnemy = newObj.GetComponent<Golem>();
                        ((Golem)enemy).hpBar_Boss.DestorySceneReset();
                    }
                    else
                    {
                        Debug.LogError("Golem is null at ResetAllenemy in UnitManager.");
                        return;
                    }
                }
				else
				{
					continue;
				}
			}
            newEnemy.navAgent.enabled = false;
            newEnemy.gameObject.transform.position = enemy.initPos;
            newEnemy.gameObject.transform.forward = enemy.initForward;
            newEnemy.navAgent.enabled = true;

            newEnemy.SetInitTr(enemy.initPos, enemy.initForward);
            newObj.SetActive(true);

            ObjectPoolingCenter.Instance.AddTrashBin(enemy.gameObject);
        }

		ClearEnemyList();
        SearchEnemy();
    }

    private void ClearEnemyList()
    {
        allEnemyList.Clear();
        aliveEnemyList.Clear();
        foreach (var pair in aliveEnemyDic)
        {
            pair.Value.Clear();
        }
    }

    private void SearchEnemy()
    {

        GameObject[] allEnemyGoList = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < allEnemyGoList.Length; i++)
        {
            Enemy enemy = allEnemyGoList[i].GetComponent<Enemy>();

            if (enemy != null && enemy.gameObject.activeSelf &&!allEnemyList.Contains(enemy) )
            {
                if (enemy.status.name_e == eEnemyName.Golem)
                {
                    boss_Golem = enemy;
                }

                SetEnemiesRagdoll(ref enemy);

                allEnemyList.Add(enemy);
                aliveEnemyList.Add(enemy);

                List<Enemy> dicList;
                if (aliveEnemyDic.TryGetValue(enemy.status.name_e, out dicList))
                {
                    dicList.Add(enemy);
                }
            }
        }
    }

    public void EraseDeathEnemy(Enemy script)
    {
        if (aliveEnemyList.Count == 0 |
            !aliveEnemyList.Contains(script))
        {
            Debug.Log("이 에너미는 리스트에 없는디요?");
            return;
        }

        if (script.status.isDead)
        {
            aliveEnemyList.Remove(script);

            List<Enemy> dicList;
            if (aliveEnemyDic.TryGetValue(script.status.name_e, out dicList))
            {
                dicList.Remove(script);
            }
        }

    }

    private void SetEnemiesRagdoll(ref Enemy script)
    {
        if (script.status.name_e == eEnemyName.Golem)
        {
            return;
        }

        if (!script.ragdoll)
        {
            GameObject ragdollObj = Instantiate(enemyRagdollPrefabList[(int)script.status.name_e], ragdollBox);
            script.ragdoll = ragdollObj.GetComponent<Enemy_Ragdoll>();
            ragdollObj.SetActive(false);
        }
    }

    private void SetHpBarEnemy(Enemy enemyScript)
    { 
    
    
    }
        

	//// <EnemyFuncs>





	#region TestEnemy
	public GameObject testEmptyEnemyPrefab;
    public GameObject SpawnTestEnemy(Vector3 pos)
    {
        GameObject testObj = Instantiate(testEmptyEnemyPrefab, pos, Quaternion.identity);

        Enemy tempScript = testObj.GetComponent<Enemy>();

        allEnemyList.Add(tempScript);

        return testObj;
    }
	#endregion



	private void Awake()
    {
        aliveEnemyDic = new Dictionary<eEnemyName, List<Enemy>>();

        for (int i = 0; i < (int)eEnemyName.End; ++i)
        {
            aliveEnemyDic.Add((eEnemyName)i, new List<Enemy>());
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            ResetAllEnemies();
        }
    }
}
