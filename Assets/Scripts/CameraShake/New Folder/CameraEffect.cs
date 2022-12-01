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
            Debug.Log(curData.name + "�� ����ũ �Ҳ���");
        }
        else Debug.Log("�ش� �����ʹ� ��ųʸ��� �������� �ʽ��ϴ�.");
    }

    //�⺻������ �̹� ������ �͵��� ��� �����ϳ�
    //�׷� ������ ���� �ϳ� �޾Ƽ� �����ٴ� üũ �ؾ߰ڳ�

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
    //        //������ �Ȱ����� ����� ��³� ����
    //        Debug.Log("�Ȱ��� ����ũ �ð�����!!!!!!");
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
    //            //data ����
    //            Debug.Log("�ݹ� ���� ����ũ ����!!!!!!");
    //            if (!curData.GetStart) curData.GetStart = true;
    //            return curData;
    //        }
    //        else
    //        {
    //            //<��ø�Ǵ� �ð����� ���� ������>
    //            Debug.Log("���� ��� ���� �ö�!!!!!!");
    //
    //            curData.Conflict = true;        //�� ��������
    //            curData.AddValue = 2f;  //�̸�ŭ�� ���͸� �����ٲ���
    //            curData.ConflictTime = curData.CurrentTime + data.duration;
    //            return curData;
    //        }
    //    }
    //    else if (curData.duration < data.duration)
    //    {
    //        if (curData.GetScore > data.GetScore)
    //        {
    //            //��ø�ð����� �����ٰ�
    //            //��ø�ð� ������ �ٽ� ������ ������ ���ƿ�
    //
    //            if (!curData.GetStart)
    //            {
    //                Debug.Log("�����ٰ� ���� ���� ��� ������ ��ȯ!!!!!!");
    //
    //                data.CurrentTime = data.duration - (curData.duration - curData.CurrentTime);
    //                if (!data.GetStart) data.GetStart = true;
    //                return data;
    //            }
    //        }
    //        else
    //        {
    //            //��ø�Ǵ� �ð����� ���� ������
    //            //��ø�ð� ������ �ٽ� ������ ������ ���ƿ�
    //
    //            curData.Conflict = true;        //�� ��������
    //            curData.AddValue = 2f;  //�̸�ŭ�� ���͸� �����ٲ���
    //            if (!curData.GetStart)
    //            {
    //                Debug.Log("���� �������ٰ� ���� ��� ������ ��ȯ!!!!!!");
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
