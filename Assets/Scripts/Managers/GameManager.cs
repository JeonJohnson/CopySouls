using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{
    [Header("Manager Boxes")]
    public GameObject managerBox;
    public GameObject managerBox_Destory;


    public GameObject objectPoolingManagerPrefab;
    public GameObject soundManagerPrefab;

    [RuntimeInitializeOnLoadMethod]
    private static void GameInitialize()
    {    //게임시작시 호출되는 어트리뷰트
         //static 함수만 호출 가능함.
         
        //여기서는 GameManager만 만들도록 합시다

        GameManager.InstantiateManager();
        //InstantiateManagerByPrefabPath(Defines.managerPrfabFolderPath);
        
    }
    public void InstantiateManagerBoxes(out GameObject box, out GameObject destroyBox)
    {
        box = Funcs.CheckGameObjectExist("ManagerBox");
        DontDestroyOnLoad(box);

        destroyBox = Funcs.CheckGameObjectExist("ManagerBox_Destory");
        destroyBox.transform.SetAsFirstSibling();
    }

    public void GeunHeeSceneManagersCreate()
    {
        ObjectPoolingCenter.InstantiateManager(false); ;
        UnitManager.InstantiateManager(false);
        //ObjectPoolingCenter.InstantiateManagerByPrefab(objectPoolingManagerPrefab, managerBox);
    }


    private void Awake()
	{
        DontDestroyOnLoad(this.gameObject);
        InstantiateManagerBoxes(out managerBox, out managerBox_Destory);

        //여기서 씬체크한뒤에 원하는 방식대로 하면됨.
        GeunHeeSceneManagersCreate();
        Cursor.lockState = CursorLockMode.Locked;

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

    public override void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {

    }
}
