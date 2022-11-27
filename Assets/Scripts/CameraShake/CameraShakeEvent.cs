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

// Rad�� Deg�� �ٲܷ��� : x Rad * (180 / PI) = Deg
// Deg�� Rad�� �ٲܷ��� : x Deg * (PI / 180) = Rad

// Deg : �� �״�� ����
// Rad : ȣ�� ���̿� �������� ���̰� ���� �Ǵ� ����

// Rad�� ���� ���� : Deg�ʹ� �ٸ��� �������� rad�� �ȴٸ� ȣ�� ���̸�
//                 : ȣ�� �������� �ȴٸ� rad��
//                 : ȣ�� rad�� �ȴٸ� �������� ���� ���� �� ����


// _> Mathf.Deg2Rad * 720.0f �� 720.0���� �������� �ٲٴ� ����