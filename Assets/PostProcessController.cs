using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessController : MonoBehaviour
{
    public PostProcessProfile ppProfile;
    Bloom bloom; //빈 눈뽕(광원을 가진 애들이 눈뽕을 줌)
    DepthOfField DOF; // 아이폰 인물사진 모드 
    Vignette vignette;// 카메라 테두리 어둡게
    ColorGrading CG; // 색감

    [Header("Bloom")]
    [SerializeField]
    private float originIntensity; //강도
    [SerializeField]
    private float Threshold; //0 ~ 1값으로 조절하며 알파 값 느낌으로 막을 하나 씌우네 하얀거
    [SerializeField]
    private float SoftKnee; //광원의 범위

    [Header("DepthOfField")]
    [SerializeField]
    private float FocusDistance; // 아이폰 인물사진모드 강도 느낌

    [Header("Vignette")]
    [SerializeField]
    private float Vignette_Intensity;

    //private float FocusDistance; // 아이폰 인물사진모드 강도 느낌
    //[SerializeField]
    //private float originIntensity; //강도
    //[SerializeField]
    //private float originIntensity; //강도
    //[SerializeField]
    //private float originIntensity; //강도
    //[SerializeField]
    //private float originIntensity; //강도
    //[SerializeField]
    float originSoftKnee; 

    private void Awake()
    {
        InitOriginSettings();
    }

    private void InitOriginSettings()
    {
        ppProfile = GetComponent<PostProcessVolume>().profile;
        ppProfile.TryGetSettings<Bloom>(out bloom);
        originIntensity = bloom.intensity.value;
        originSoftKnee = bloom.softKnee.value;
    }

    public void DoBloom(float intenValue, float SoftValue, float time)
    {
        StartCoroutine(SetValueCoro(intenValue, SoftValue, time));
    }

    IEnumerator SetValueCoro(float intenValue, float SoftValue, float time)
    {

        float timer = 1f;
        while(timer > 0f)
        {
            bloom.intensity.value = Mathf.Lerp(originIntensity, intenValue, 1 - timer);
            bloom.softKnee.value = Mathf.Lerp(originSoftKnee, SoftValue, 1 - timer);
            timer -= Time.unscaledDeltaTime * 3f;
            yield return null;
        }
        bloom.intensity.value = intenValue;
        bloom.softKnee.value = SoftValue;


        yield return new WaitForSecondsRealtime(time);
        timer = 1f;
        while (timer > 0f)
        {
            bloom.intensity.value = Mathf.Lerp(originIntensity, intenValue, timer);
            bloom.softKnee.value = Mathf.Lerp(originSoftKnee, SoftValue, timer);
            timer -= Time.unscaledDeltaTime * 3f;
            yield return null;
        }

        bloom.intensity.value = originIntensity;
        bloom.softKnee.value = originSoftKnee;
        yield break;
    }
    
}
