using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_AutoDestroy : MonoBehaviour
{
    // If true, deactivate the object instead of destroying it
    public bool OnlyDeactivate;
    public float DestroyTime = 4.5f;

    void OnEnable()
    {
        StartCoroutine(CheckIfAlive());
    }

    IEnumerator CheckIfAlive()
    {
        ParticleSystem ps = this.GetComponent<ParticleSystem>();

        while (true && ps != null)
        {
            yield return new WaitForSeconds(DestroyTime);
            if (!ps.IsAlive(true))
            {
                if (OnlyDeactivate)
                {
#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
#else
                    this.gameObject.SetActive(false);
#endif
                }
                else
                    ps.Stop();
                    ObjectPoolingCenter.Instance.ReturnObj(this.gameObject);
                    GameObject.Destroy(this.gameObject);
                break;
            }
        }
    }
}
