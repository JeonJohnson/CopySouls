using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eArrowState
{
    Draw,
    Hook,
    Shoot,
    End
}

public class CommonArrow : Weapon, IPoolingObject
{
    public Rigidbody rd;

    public eArrowState state = eArrowState.Draw;

    public LayerMask colLayer;

    public float aliveTime;
    public float spd;
    public float maxRange;
    [HideInInspector] public float mileage = 0f;

    public Transform arrowHeadTr;

    public Transform rightHandTr;
    public Transform bowLeverTr;

    public Transform targetTr;
	//public Vector3 hookDir;

	public IEnumerator AliveCoroutine()
	{
        yield return new WaitForSeconds(aliveTime);

        ResetForReturn();
        ObjectPoolingCenter.Instance.ReturnObj(this.gameObject);
	}

	public void LookTarget()
    {
        transform.LookAt(targetTr.position);
        //Debug.Log(targetTr.position);
    }

    public void ResetForReturn()
    {

        rd.velocity = Vector3.zero;
    }

    protected override void weaponInitialize()
    {
        rd = GetComponent<Rigidbody>();
    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();


		switch (state)
		{
			case eArrowState.Draw:
                {
                    transform.forward = rightHandTr.right;
                    transform.localPosition = Vector3.zero;
                }
				break;
			case eArrowState.Hook:
                {
                    transform.forward = bowLeverTr.forward;
                }
				break;
			case eArrowState.Shoot:
                { 
                    
                }
				break;
			case eArrowState.End:
                { 
                
                }
				break;
			default:
				break;
		}

    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (state == eArrowState.Shoot)
        {
            rd.MovePosition(transform.position + (transform.forward * Time.deltaTime * spd));
        }
    }

	public override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);

        if (((1 << other.gameObject.layer) & colLayer) != 0)
        {
            //((1 << other.gameObject.layer) & includeLayers) != 0
            Transform tempTr = other.gameObject.transform;

            GameObject staticArrow = ObjectPoolingCenter.Instance.LentalObj("CommonArrow_Static");

            staticArrow.transform.position = transform.position;
            staticArrow.transform.rotation = transform.rotation;
            staticArrow.transform.SetParent(tempTr);

            ResetForReturn();
            ObjectPoolingCenter.Instance.ReturnObj(this.gameObject);
        }
    }
}
