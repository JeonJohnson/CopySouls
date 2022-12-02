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
    LensDistortion Lens; // 렌즈

    [Header("Bloom")]
    [SerializeField]
    private float originIntensity; //강도
    [SerializeField]
    private float originThreshold; //0 ~ 1값으로 조절하며 알파 값 느낌으로 막을 하나 씌우네 하얀거
    [SerializeField]
    private float originSoftKnee; //광원의 범위

    [Header("DepthOfField")]
    [SerializeField]
    private float origindFocusDistance; // 아이폰 인물사진모드 강도 느낌

    [Header("Lens")]
    [SerializeField]
    private float originLensIntensity; //굴곡정도

    private void Awake()
    {
        InitOriginSettings();
    }

    private void InitOriginSettings()
    {
        ppProfile = GetComponent<PostProcessVolume>().profile;
        ppProfile.TryGetSettings<DepthOfField>(out DOF);
        ppProfile.TryGetSettings<LensDistortion>(out Lens);
        ppProfile.TryGetSettings<Bloom>(out bloom);

        originIntensity = bloom.intensity.value;
        originThreshold = bloom.threshold.value;
        originSoftKnee = bloom.softKnee.value;

        origindFocusDistance = DOF.focusDistance.value;

        originLensIntensity = Lens.intensity.value;

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

    //아마 달릴때 쓰면 좋을듯
    public void DoFocus(float fouseDistance, float time)
    {
        StartCoroutine(SetFocus(fouseDistance, time));
    }

    IEnumerator SetFocus(float fouseDistance, float time)
    {
        DOF.focusDistance.value = fouseDistance;
        yield return new WaitForSecondsRealtime(time);
        DOF.focusDistance.value = origindFocusDistance;
    }

    public void DoLens(float intensity, float time)
    {
        StartCoroutine(SetLens(intensity, time));
    }

    IEnumerator SetLens(float intensity, float time)
    {
        float timer = 1f;
        while (timer > 0f)
        {
            Lens.intensity.value = Mathf.Lerp(originLensIntensity, intensity, 1 - timer);
            timer -= Time.unscaledDeltaTime * 3f;
            Debug.Log(Lens.intensity.value);
            yield return null;
        }
        Lens.intensity.value = intensity;
        yield return new WaitForSecondsRealtime(time);
        timer = 1f;
        while (timer > 0f)
        {
            Lens.intensity.value = Mathf.Lerp(originLensIntensity, intensity, timer);
            timer -= Time.unscaledDeltaTime * 3f;
            Debug.Log(bloom.intensity.value);
            yield return null;
        }
        Lens.intensity.value = originLensIntensity;
        yield break;
    }
}
