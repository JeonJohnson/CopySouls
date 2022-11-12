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

    private void StuckArrow(Transform tr)
    {
        //Transform tempTr = other.gameObject.transform;

        GameObject staticArrow = ObjectPoolingCenter.Instance.LentalObj("CommonArrow_Static");

        staticArrow.transform.position = transform.position;
        staticArrow.transform.rotation = transform.rotation;
        staticArrow.transform.SetParent(tr);


        WeaponColliderOnOff(false);
        ResetForReturn();
        ObjectPoolingCenter.Instance.ReturnObj(this.gameObject);
    }

    private void BounceArrow()
    {
        //GameObject staticArrow = ObjectPoolingCenter.Instance.LentalObj("CommonArrow_Static");
        //staticArrow.transform.position = transform.position;
        //staticArrow.transform.rotation = transform.rotation;
        //ResetForReturn();
        //ObjectPoolingCenter.Instance.ReturnObj(this.gameObject);

        WeaponColliderOnOff(false);

        //Funcs.Vec3_Random(-5f, 5f);
        transform.Rotate(Funcs.Vec3_Random(-100f, 100f));
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

            //int tempLayerIndex = LayerMask.NameToLayer("Player");
            //int tempLayerBit = 1 << LayerMask.GetMask("Player");

        if (((1 << other.gameObject.layer) & colLayer) != 0)
        {
            LayerMask playerLayers =
                LayerMask.GetMask("Player_Hitbox")
                | LayerMask.GetMask("PlayerWeapon");
                //| LayerMask.GetMask("Player");

            if (((1 << other.gameObject.layer) & playerLayers) != 0)
            {
                if (Player.instance.status.isGuard)
                {
                    float degree =
                        Mathf.Acos(Vector3.Dot(transform.forward, -Player.instance.playerModel.transform.forward))
                        * Mathf.Rad2Deg;
                    //Debug.Log($"화살 방어각 : {degree}");
                    if (degree <= 35f)
                    {
                        //Debug.Log("화살 튕겨나감 ㅅㄱ ㅋㅋ");
                        BounceArrow();
                        return;
                    }
                }
            }

            StuckArrow(other.gameObject.transform);
        }
    }
}
