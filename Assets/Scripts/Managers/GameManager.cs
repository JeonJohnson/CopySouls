﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{

    public List<GameObject> managerPrefabs;

    [RuntimeInitializeOnLoadMethod]
    private static void GameInitialize()
    {    //게임시작시 호출되는 어트리뷰트
         //static 함수만 호출 가능함.
        
        InstantiateManagerByPrefab("ManagerPrefabs/");
        //게임 시작하면 이 함수 호출하면서 GameManager의 prefab찾아서 생성.


        //그 이후에
        //우리가 각 씬별 테스트 할때
        //각 씬 별로 다르게 생성 순서, 생성 유무를 정하기
    }

    private void Awake()
	{
		
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}