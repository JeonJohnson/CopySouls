using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IPoolingObject
{

    public bool isShoot = false;
    public GameObject hand;
    public GameObject target;
    public GameObject master;

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
        
        isShoot = true;

        while (time < fullTime)
        {
            time += Time.deltaTime;
            transform.position += transform.forward * Time.deltaTime * spd;
            yield return null;
        }

        ResetForReturn();
        ObjectPoolingCenter.Instance.ReturnObj(this.gameObject, Enums.ePoolingObj.Arrow);
        //ResetForReturn();
    }


    public void Awake()
    {
        //StartCoroutine(AliveCouroutine());

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShoot && hand != null)
        { 
            
            transform.position = hand.transform.position;
            transform.forward = target.transform.position - transform.position;
        }
    }

    private void OnEnable()
    {
        
    }

}
