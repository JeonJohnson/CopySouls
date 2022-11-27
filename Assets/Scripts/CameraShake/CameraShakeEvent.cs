using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shake Transform Event", menuName = "Custom/Shake Transfomr Event", order = 1)]
public class CameraShakeEvent : ScriptableObject
{
    public enum Target
    {
        Position,
        Rotation
    }

    public Target target = Target.Position;

    public float amplitude = 1.0f;
    public float frequency = 1.0f;

    public float duration = 1.0f;

   
    public AnimationCurve blendOverLifetime = new AnimationCurve(

        new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
        new Keyframe(0.2f, 1.0f),
        new Keyframe(1.0f, 0.0f));

    public void Init(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime, Target target)
    {
        this.target = target;

        this.amplitude = amplitude;
        this.frequency = frequency;

        this.duration = duration;

        this.blendOverLifetime = blendOverLifetime;
    }
}


// Rad -> Deg
// PI Rad = 180 Deg
// 1 Rad = 180 / PI (Deg)

// Deg -> Rad
// PI Rad = 180 Deg
// PI / 180 (Rad)= 1 Deg

// Rad을 Deg로 바꿀려면 : x Rad * (180 / PI) = Deg
// Deg를 Rad을 바꿀려면 : x Deg * (PI / 180) = Rad

// Deg : 말 그대로 각도
// Rad : 호의 길이와 반지름의 길이가 같게 되는 각도

// Rad을 쓰는 이유 : Deg와는 다르게 반지름과 rad을 안다면 호의 길이를
//                 : 호와 반지름을 안다면 rad을
//                 : 호와 rad을 안다면 반지름을 쉽게 구할 수 있음


// _> Mathf.Deg2Rad * 720.0f 는 720.0도를 라디안으로 바꾸는 과정