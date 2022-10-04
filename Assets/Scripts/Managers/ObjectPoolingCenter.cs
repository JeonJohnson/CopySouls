using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

public class ObjectPoolingCenter : Manager<ObjectPoolingCenter>
{

    public GameObject[] prefabs; //�����յ� ��ƵѰ�
    GameObject[] objBoxes; //�� ������Ʈ ��� ���� �ڽ�
    //�� ���ӿ�����Ʈ, �ν�����â�� ������ ���鲨��
    //�״ϱ� �� ������Ʈ �ֻ��� EmptyGameObject

    Queue<GameObject>[] poolingObjQueue;
    //���⿡ ��Ƶΰ� �ϳ��� ��������


    public void CreateBoxes()
    {
        for (int i = 0; i < (int)ePoolingObj.End; ++i)
        {
            GameObject tempObj = new GameObject(((ePoolingObj)i).ToString());
            tempObj.transform.SetParent(this.gameObject.transform);
            objBoxes[i] = tempObj;
        }
    }

    public void FillObject(ePoolingObj objType, int count = 50)
    {
        if (prefabs[(int)objType] == null)
        {
            Debug.Log("ObjectPoolingCenter Notice!!\n" + Funcs.GetEnumName<ePoolingObj>((int)objType) + "�� prefab�� ����ϴ�");
            return;
        }

        for (int i = 0; i < count; ++i)
        {
            GameObject obj = Instantiate(prefabs[(int)objType], objBoxes[(int)objType].transform);
            obj.SetActive(false);
            poolingObjQueue[(int)objType].Enqueue(obj);
        }
    }
    public GameObject LentalObj(ePoolingObj kind, int count = 1)
    {
        if (poolingObjQueue[(int)kind].Count < count)
        {
            FillObject(kind, count);
            return LentalObj(kind, count);
        }
        else
        {
            GameObject tempObj = poolingObjQueue[(int)kind].Dequeue();
            tempObj.SetActive(true);
            tempObj.transform.SetParent(null);
            return tempObj;
        }
    }

    public GameObject LentalObj(ePoolingObj kind, GameObject parentObj, int count = 1)
    {
        if (parentObj != null)
        {
            GameObject temp = LentalObj(kind, count);
            temp.transform.SetParent(parentObj.transform);
            return temp;
        }
        else
        {
            Debug.Log("Object Pooling Center's Error\n" +
                "LentalObj Func's parentObj is null");

            return LentalObj(kind, count);
        }
    }

    public void ReturnObj(GameObject obj, ePoolingObj kind)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;


        obj.transform.SetParent(objBoxes[(int)kind].transform);

        obj.SetActive(false);

        poolingObjQueue[(int)kind].Enqueue(obj);
    }




    void Awake()
	{
        poolingObjQueue = new Queue<GameObject>[(int)ePoolingObj.End];

        for (int i = 0; i < (int)ePoolingObj.End; ++i)
        {
            poolingObjQueue[i] = new Queue<GameObject>();
        }

        objBoxes = new GameObject[(int)ePoolingObj.End];

        prefabs = new GameObject[(int)ePoolingObj.End];


        CreateBoxes();
        //FindPrefabs();
        FillObject(ePoolingObj.Arrow);
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
