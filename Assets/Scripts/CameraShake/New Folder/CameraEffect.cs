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
        else Debug.Log("해당 데이터는 딕셔너리에 존재하지 않습니다.");
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
        //찌르는 이펙트(앞으로 나아가는 이펙트)
        //줌인 추가

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
        //양손공격(내려찍기,오른쪽으로 휘두르기)
    }

    public void ChargeAttEffect()
    {
        //차지공격(오른쪽 위로)
        //애니메이션 이벤트로 만듬
        PlayZoom(ZoomDir.Front, 0.1f, true);
    }
    public void SuccessParringEffect()
    {

    }
    public void SuccessHoldEffect()
    {
        //뒤로갔다 앞으로

    }
    public void HitEffect()
    {
        //아래로 흔들렸으면 좋겠(출렁 한번)
        
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
        Debug.Log("줌인 중임");
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
            //진행
            startTimer += Time.deltaTime;
            Vector3 vec = new Vector3(0.0f, 0.0f, originPos.z + -Dir.z * speed * startTimer);
            Camera.main.transform.localPosition = vec;
        }
        else
        {
            //외부에서 강제로 꺼
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
