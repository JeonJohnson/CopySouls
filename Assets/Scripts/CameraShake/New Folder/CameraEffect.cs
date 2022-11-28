using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public static string EffectDataPrfabFolderPath = "EffectDataPrefabs";

    public static CameraEffect instance = null;

    Vector3 originPos;
    Vector3 originRot;

    [SerializeField]
    private List<EffectData> List_EffectDatas = new List<EffectData>();
    private Dictionary<string, EffectData> Dic_EffectDatas = new Dictionary<string, EffectData>();
    public EffectData curData;

    public Vector3 OriginPos { get { return originPos; } }
    public Vector3 OriginRot { get { return originRot; } }
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    private void Start()
    {
        Load_Dic();

        originPos =  Camera.main.transform.localPosition;
        originRot = Camera.main.transform.localEulerAngles;
    }

    private void Update()
    {
        if(curData) curData.Update();
    }

    private void Load_Dic()
    {
        EffectData[] Datas =  Resources.LoadAll<EffectData>(EffectDataPrfabFolderPath);
        for(int i = 0; i < Datas.Length; i++)
        {
            Dic_EffectDatas.Add(Datas[i].name, Datas[i]);
        }
    }

    //��ø���� ������ ����ũ �Ǵ� ���??
    // 1 ���� ����ũ duration
                        // ���� ���� �� ū ������ ����
                                                        //���� ������ ���̰� ū ���� ���̺��� ��� ū ���� ����
                                                        //ū ������ ���̰� 

    // 2 ���� ����ũ �� 
                        // ���� ���� �� ū���� -> ū �������� ��ü
                        // ū ���� �� ���� ���� -> ���� ���� ����

    public void PlayShake(string dataName)
    {
        if (Dic_EffectDatas.ContainsKey(dataName))
        {
            EffectData data = Dic_EffectDatas[dataName];
            if (curData == null)
            {
                List_EffectDatas.Add(data);
            }

            curData = data;
        }
        else Debug.Log("�ش� �����ʹ� ��ųʸ��� �������� �ʽ��ϴ�.");




        if (curData == null)
        {
            Debug.Log("���� �������� ����ũ �ֽ��ϴ�~ " + dataName + "�� �������� �̰� ����");
            return;
        }

       
    }
}
