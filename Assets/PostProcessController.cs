using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessController : MonoBehaviour
{
    public PostProcessProfile ppProfile;
    Bloom bloom;

    [Header("Bloom")]
    float originIntensity;
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
