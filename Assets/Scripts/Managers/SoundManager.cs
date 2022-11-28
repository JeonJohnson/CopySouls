using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager<SoundManager>
{
    public Dictionary<string, AudioClip> audClips;
    string audioFolderPath = "AudioClips";

    public AudioSource bgmAS;
    public List<AudioSource> tempAS;


    //브금 -> 여기서
    //엠비언트 -> 걍 맵 히어라키에서 세팅
    //일시적 물체음 -> Pooling해두고 세팅
    //해당 오브젝트에서 날 소리 -> transform으로 ㄴㄴ 오디오 소스 찾아서 넣고 없으면 일시적 물체음으로 ㄱㄱ

    void SearchAllAudClips()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(audioFolderPath);

        foreach (AudioClip clip in clips)
        {
            string clipName = clip.name;
            audClips.Add(clipName, clip);
        }
    
    }


    public void PlayBgm(string clipName)
    { //이건 한무반복
        
    }

    public void PlayTempSound(string clipName, Vector3 pos)
    {
    
    }

    public void PlaySound(string clipName, GameObject obj)
    {//그 오브젝트 안에 AuidoSource 컴포넌트 찾아보고 없으면 PlayTempSound로 돌릴꺼임 
    

    }

	void Awake()
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
