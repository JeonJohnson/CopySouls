using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

using UnityEngine.SceneManagement;

public enum eSceneChangeTestIndex
{
    Intro,
    Title,
    InGame,
    Credit,
    End
}
public class GameManager : Manager<GameManager>
{

    [Header("Manager Boxes")]
    public GameObject managerBox;
    public GameObject managerBox_Destory;


    public GameObject objectPoolingManagerPrefab;
    public GameObject soundManagerPrefab;
    public GameObject effectManagerPrefab;



    [Header("Ingame Settings")]
    public float mouseSensivility = 5f;
    public float BgmOffset;
    public float SeOffset;

    [RuntimeInitializeOnLoadMethod]
    private static void GameInitialize()
    {    //게임시작시 호출되는 어트리뷰트
         //static 함수만 호출 가능함. 
         //221127 => 이거 알고보니 Awake 이후에 호출됨....
         
        //여기서는 GameManager만 만들도록 합시다

        GameManager.InstantiateManager(true);
        //InstantiateManagerByPrefabPath(Defines.managerPrfabFolderPath);
        
    }
    public void InstantiateManagerBoxes(out GameObject box, out GameObject destroyBox)
    {
        box = Funcs.CheckGameObjectExist("ManagerBox");
        DontDestroyOnLoad(box);

        destroyBox = Funcs.CheckGameObjectExist("ManagerBox_Destory");
        destroyBox.transform.SetAsFirstSibling();
    }

    public void GeunheeSceneManagersInit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ObjectPoolingCenter.InstantiateManager(false);
        UnitManager.InstantiateManager(false);
        //ObjectPoolingCenter.InstantiateManagerByPrefab(objectPoolingManagerPrefab, managerBox);
    }

    public void JeongminSceneManagersInit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ObjectPoolingCenter.InstantiateManager(false);
        UnitManager.InstantiateManager(false);
        InGameManager.InstantiateManager(false);
        UiManager.InstantiateManager(false);
    }

    public void IntroSceneManagersInit()
    {
        IntroSceneManager.InstantiateManager(false);
    }

    public void TitleSceneManagersInit()
    {

    }

    public void InGameSceneManagersInit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ObjectPoolingCenter.InstantiateManager(false);
        UnitManager.InstantiateManager(false);
        InGameManager.InstantiateManager(false);
        InGameManager.InstantiateManager(false);
        UiManager.InstantiateManager(false);
        EffectManager.InstantiateManager(false);
    }

    public void SceneCheck(int sceneNum)
    {
        switch (sceneNum)
        {
            case (int)eSceneChangeTestIndex.Intro:
                {
                    IntroSceneManagersInit();
                }
                break;                
            case (int)eSceneChangeTestIndex.Title:
                {
                    TitleSceneManagersInit();
                }
                break;
            case (int)eSceneChangeTestIndex.InGame:
                {
                    InGameSceneManagersInit();
                }
                break;
            case (int)eSceneChangeTestIndex.Credit:
                {

                }
                break;
            default:
                break;
        }
    }


    private void Awake()
	{
        DontDestroyOnLoad(this.gameObject);
        InstantiateManagerBoxes(out managerBox, out managerBox_Destory);

        SceneCheck(SceneManager.GetActiveScene().buildIndex);

    }
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEnable()
    {
        base.OnEnable();


    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnSceneChanged(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
		switch (scene.buildIndex)
		{
            case (int)eSceneChangeTestIndex.Intro:
                {
                    Time.timeScale = 1f;
                    IntroSceneManagersInit();
                }
                break;
            case (int)eSceneChangeTestIndex.Title:
                {
                    Time.timeScale = 1f;
                    TitleSceneManagersInit();
                }
				break;
			case (int)eSceneChangeTestIndex.InGame:
                {
                    Time.timeScale = 1f;
                    InGameSceneManagersInit();
                }
				break;
            case (int)eSceneChangeTestIndex.Credit:
                {
                    Time.timeScale = 1f;
                }
                break;
			default:
				break;
		}
	}

    public void PlayerDie()
    {

    }
}
