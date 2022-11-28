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

    //중첩으로 들어오는 쉐이크 판단 어떻게??
    // 1 순위 쉐이크 duration
                        // 작은 진동 중 큰 진동이 들어옴
                                                        //작은 진동의 길이가 큰 진동 길이보다 길면 큰 진동 씹힘
                                                        //큰 진동의 길이가 

    // 2 순위 쉐이크 값 
                        // 작은 진동 중 큰진동 -> 큰 진동으로 교체
                        // 큰 진동 중 작은 진동 -> 작은 진동 씹힘

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
        else Debug.Log("해당 데이터는 딕셔너리에 존재하지 않습니다.");




        if (curData == null)
        {
            Debug.Log("현재 진행중인 쉐이크 있습니당~ " + dataName + "는 씹힐께요 이건 ㅎㅎ");
            return;
        }

       
    }
}
