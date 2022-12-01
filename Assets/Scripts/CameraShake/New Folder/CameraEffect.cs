using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraEffect : MonoBehaviour
{
    public static string EffectDataPrfabFolderPath = "EffectDataPrefabs";

    public static CameraEffect instance = null;

    Vector3 originPos;
    Vector3 originRot;

    private bool curDataStop;

    [SerializeField]
    private List<EffectData> List_EffectDatas = new List<EffectData>();
    private Dictionary<string, EffectData> Dic_EffectDatas = new Dictionary<string, EffectData>();
    public EffectData curData;

    public Vector3 OriginPos { get { return originPos; } }
    public Vector3 OriginRot { get { return originRot; } }
    public bool Stop { get { return curDataStop; } set { curDataStop = value; } }

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
            EffectData data = new EffectData(Datas[i].duration,Datas[i].roughness, Datas[i].magnitude, Datas[i].shakePosition, Datas[i].shakeRotation, Datas[i].radius, Datas[i].Curve, Datas[i].isSeedUpdate);
            Dic_EffectDatas.Add(Datas[i].name, Datas[i]);
        }
    }

    public void PlayShake(string dataName)
    {
        if (Dic_EffectDatas.ContainsKey(dataName))
        {
            EffectData data = Dic_EffectDatas[dataName];
            //List_EffectDatas.Add(data);

            curData = data;//SelectCurData(data);
            Debug.Log(curData.name + "을 쉐이크 할꺼임");
        }
        else Debug.Log("해당 데이터는 딕셔너리에 존재하지 않습니다.");
    }

    //기본적으로 이미 실행한 것들은 모두 수행하네
    //그럼 끝날때 인자 하나 받아서 끝났다는 체크 해야겠네

    //private EffectData SelectCurData(EffectData data)
    //{
    //    //if (List_EffectDatas.Capacity == 0) return null;
    //    if (curData == null)
    //    {
    //        if (!data.GetStart) data.GetStart = true;
    //        return data;
    //    }
    //    if (curData == data)
    //    {
    //        //실행중 똑같은거 드오면 드온놈 실행
    //        Debug.Log("똑같은 쉐이크 시간연장!!!!!!");
    //        curData.GetStart = false;
    //        if (!data.GetStart) data.GetStart = true;
    //        return data;
    //    }
    //
    //    if (curData.duration == data.duration)
    //    {
    //        if (curData.GetScore >= data.GetScore)
    //        {
    //            if (!curData.GetStart) curData.GetStart = true;
    //            return curData;
    //        }
    //        else
    //        {
    //            if (!data.GetStart) data.GetStart = true;
    //            return data;
    //        }
    //    }
    //    else if (curData.duration > data.duration)
    //    {
    //        if (curData.GetScore > data.GetScore)
    //        {
    //            //data 씹힘
    //            Debug.Log("금방 누른 쉐이크 씹힘!!!!!!");
    //            if (!curData.GetStart) curData.GetStart = true;
    //            return curData;
    //        }
    //        else
    //        {
    //            //<중첩되는 시간동안 강도 높여줌>
    //            Debug.Log("지금 잠깐 강도 올라감!!!!!!");
    //
    //            curData.Conflict = true;        //이 시점부터
    //            curData.AddValue = 2f;  //이만큼의 벡터를 더해줄꺼임
    //            curData.ConflictTime = curData.CurrentTime + data.duration;
    //            return curData;
    //        }
    //    }
    //    else if (curData.duration < data.duration)
    //    {
    //        if (curData.GetScore > data.GetScore)
    //        {
    //            //중첩시간동안 씹히다가
    //            //중첩시간 끝나면 다시 본래의 강도로 돌아옴
    //
    //            if (!curData.GetStart)
    //            {
    //                Debug.Log("씹히다가 원래 지금 드온 강도로 변환!!!!!!");
    //
    //                data.CurrentTime = data.duration - (curData.duration - curData.CurrentTime);
    //                if (!data.GetStart) data.GetStart = true;
    //                return data;
    //            }
    //        }
    //        else
    //        {
    //            //중첩되는 시간동안 강도 높여줌
    //            //중첩시간 끝나면 다시 본래의 강도로 돌아옴
    //
    //            curData.Conflict = true;        //이 시점부터
    //            curData.AddValue = 2f;  //이만큼의 벡터를 더해줄꺼임
    //            if (!curData.GetStart)
    //            {
    //                Debug.Log("강도 높아졌다가 지금 드온 강도로 변환!!!!!!");
    //
    //                data.CurrentTime = data.duration - (curData.duration - curData.CurrentTime);
    //                return data;
    //            }
    //        }
    //    }
    //    
    //    return null;
    //}
}
