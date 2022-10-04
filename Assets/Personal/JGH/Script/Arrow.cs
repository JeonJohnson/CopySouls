using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IPoolingObject
{
    public float spd;

    public float time = 0;
    public float fullTime = 10f;

    public void ResetForReturn()
    {
        time = 0f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public IEnumerator AliveCouroutine()
    {
        time += Time.deltaTime;

        while (time < fullTime)
        {
            yield return null;
        }

        ResetForReturn();
        ObjectPoolingCenter.Instance.ReturnObj(this.gameObject, Enums.ePoolingObj.Arrow);
        //ResetForReturn();
    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * spd;
    }

    private void OnEnable()
    {
        StartCoroutine(AliveCouroutine());
    }

}
