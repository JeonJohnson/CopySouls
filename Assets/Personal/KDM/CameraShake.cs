using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float roughness; // 거칠기 정도
    public float magnitude; // 움직임 범위
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Shake(float duration)
    {
        float halfDuration = duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * roughness;
            transform.position = new Vector3
                (Mathf.PerlinNoise(tick, 0) - 0.5f, Mathf.PerlinNoise(0, tick) - 0.5f, 0f) * magnitude * Mathf.PingPong(elapsed, halfDuration);

            yield return null;
        }
    }
}
