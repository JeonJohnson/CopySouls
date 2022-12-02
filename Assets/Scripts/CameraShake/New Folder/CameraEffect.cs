using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShakeType
{
    Smooth,
    Rough
}

public class CameraEffect : MonoBehaviour
{
    public static string EffectDataPrfabFolderPath = "EffectDataPrefabs";

    public static CameraEffect instance = null;

    public CameraTest ct;

    Vector3 originPos;
    Vector3 originRot;

    private bool curDataStop;

    public bool zoomStart;

    [SerializeField]
    private List<EffectData> List_EffectDatas = new List<EffectData>();
    private Dictionary<string, EffectData> Dic_EffectDatas = new Dictionary<string, EffectData>();
    public EffectData curData;
    public Zoom curZoom;

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
        if (curZoom != null)
        {
            if (!curZoom.isFinish)
            {
                if (!zoomStart) zoomStart = true;
                curZoom.Update();
            }
            else
            {
                zoomStart = false;
                curZoom.isFinish = false;
                curZoom = null;
            }
        }
    }
    private void LateUpdate()
    {
        if(curData) curData.Update();
    }

    private void Load_Dic()
    {
        EffectData[] Datas =  Resources.LoadAll<EffectData>(EffectDataPrfabFolderPath);
        for(int i = 0; i < Datas.Length; i++)
        {
            EffectData data = new EffectData(Datas[i].duration,Datas[i].shakeSpeed, Datas[i].magnitude, Datas[i].shakePosition,
                                             Datas[i].shakeRotation, Datas[i].Curve, Datas[i].isSeedUpdate);
            Dic_EffectDatas.Add(Datas[i].name, Datas[i]);
        }
    }

    public void PlayShake(string dataName)
    {
        if (Dic_EffectDatas.ContainsKey(dataName))
        {
            EffectData data = Dic_EffectDatas[dataName];
            //List_EffectDatas.Add(data);
            data.Start = true;
            curData = data;//SelectCurData(data);
        }
        else Debug.Log("�ش� �����ʹ� ��ųʸ��� �������� �ʽ��ϴ�.");
    }

    public void PlayZoom(ZoomDir dir,float speed,float duration)
    {
        curZoom = new Zoom(dir, speed, duration);
    }
    public void PlayZoom(ZoomDir dir, float speed,bool check)
    {
        curZoom = new Zoom(dir, speed, check);
    }


    public void PlayStepEffect()
    {
        //��� ����Ʈ(������ ���ư��� ����Ʈ)
        //���� �߰�

    }
    public void PlayLeftAttEffect()
    {
        PlayShake("Player_LeftAtt");
    }
    public void PlayRightAttEffect()
    {
        PlayShake("Player_RightAtt");
    }
    public void PlayTwoHandAttEffect()
    {
        //��հ���(�������,���������� �ֵθ���)
    }

    public void ChargeAttEffect()
    {
        //��������(������ ����)
        //�ִϸ��̼� �̺�Ʈ�� ����
        PlayZoom(ZoomDir.Front, 0.1f, true);
    }
    public void SuccessParringEffect()
    {

    }
    public void SuccessHoldEffect()
    {
        //�ڷΰ��� ������

    }
    public void HitEffect()
    {
        //�Ʒ��� �������� ����(�ⷷ �ѹ�)
        
    }
}

public enum ZoomDir
{
    Front,
    Back,
}
public class Zoom
{
    private bool check;
    public ZoomDir dir;
    public float speed;
    public float power;
    public float duration;
    public Vector3 originPos;
    private float startTimer;

    public bool isFinish;
    public bool isFinishDir;
    public bool startBack;

    private Vector3 Dir;
    private Vector3 tempPos;

    public bool Check { get{ return check; } set{ check = value; } }

    public Zoom(ZoomDir _dir, float _speed,bool _check)
    {
        dir = _dir;
        speed = _speed;
        check = _check;
        if (dir == ZoomDir.Front)
        {
            Vector3 vec = Player.instance.transform.position - Camera.main.transform.localPosition;
            Dir = vec.normalized;
        }
        else
        {
            Vector3 vec = Camera.main.transform.localPosition - Player.instance.transform.position;
            Dir = vec.normalized;
        }
    }
    public Zoom(ZoomDir _dir, float _speed, float _duration)
    {
        dir = _dir;
        speed = _speed;
        duration = _duration;
        if (dir == ZoomDir.Front)
        {
            Vector3 vec = Player.instance.transform.position - Camera.main.transform.localPosition;
            Dir = vec.normalized;
        }
        else
        {
            Vector3 vec = Camera.main.transform.localPosition - Player.instance.transform.position;
            Dir = vec.normalized;
        }
    }
    public void Update()
    {
        if (!check) Play();
        else CheckPlay();
    }
    private void Play()
    {
        Debug.Log("���� ����");
        if (startTimer < duration)
        {
            startTimer += Time.deltaTime;
            if (!isFinishDir)
            {
                if (startTimer <= duration * 0.5f)
                {
                    Vector3 vec = new Vector3(0.0f, 0.0f, originPos.z + -Dir.z * speed * startTimer);
                    Camera.main.transform.localPosition = vec;
                }
                else
                {
                    tempPos = Camera.main.transform.localPosition;
                    isFinishDir = true;
                    startTimer = 0.0f;
                }
            }
            else
            {
                startTimer += Time.deltaTime;
                Vector3 vec = new Vector3(0.0f, 0.0f, tempPos.z + Dir.z * speed * startTimer);
                Camera.main.transform.localPosition = vec;
            }
        }
        else
        {
            Camera.main.transform.localPosition = originPos;
            isFinish = true;
            isFinishDir = false;
            startTimer = 0.0f;
        }
    }

    public void CheckPlay()
    {
        if(!isFinishDir)
        {
            //����
            startTimer += Time.deltaTime;
            Vector3 vec = new Vector3(0.0f, 0.0f, originPos.z + -Dir.z * speed * startTimer);
            Camera.main.transform.localPosition = vec;
        }
        else
        {
            //�ܺο��� ������ ��
            tempPos = Camera.main.transform.localPosition;
            startTimer = 0.0f;
            startBack = true;
        }

        if(startBack)
        {
            startTimer += Time.deltaTime;
            Vector3 vec = new Vector3(0.0f, 0.0f, tempPos.z + Dir.z * speed * startTimer);
            Camera.main.transform.localPosition = vec;
            if (originPos == Camera.main.transform.localPosition)
            {
                startBack = false;
                isFinish = true;
            }
        }
    }
}
