using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ragdoll : MonoBehaviour
{
    public float aliveTime;

    public IEnumerator DisappearCoroutine()
    {
        yield return new WaitForSeconds(aliveTime);

        gameObject.SetActive(false);
    }

	public void OnEnable()
	{
        StartCoroutine(DisappearCoroutine());
	}
}
