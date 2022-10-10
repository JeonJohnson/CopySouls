using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IPoolingObject
{
    public Archer archer;
    
    public Transform rightIndexFingerBoneTr;
    public Transform bowLeverTr;

    
    public Rigidbody rd;
    public Vector3 massPivot;
    
    public GameObject head;
    
    

    public float maxRange;
    public float spd;// m/s 

    public GameObject target;
    public Vector3 destPos;

    public Vector3 straightDir;
    public Quaternion beginAngle;
    public Quaternion endAngle;
    float curveTime = 0f;
    public Vector3 curveDir;
    public float mileage;

    public float time = 0;
    public float fullTime = 10f;

    public bool isShoot = false;
    public bool isHook = false;

    //public GameObject stuckObj;
    //public Vector3 stuckPos;
    //public bool isStuck = false;


    public void ResetForReturn()
    {
        time = 0f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        rd.velocity = Vector3.zero;

        rightIndexFingerBoneTr = null;
        bowLeverTr = null;
        target = null;

        mileage = 0f;
        curveTime = 0f;

        isShoot = false;
        isHook = false;


        archer.HookArrowEvent -= Hooking;
        archer.ShootArrowEvent -= Shoot;
        archer = null;

    }



    public void Hooking()
    {
        isHook = true;
    
    }

    public void Shoot()
    {
        isHook = false;


        //transform.LookAt(target.transform);
        //float angle = Vector3.Angle(head.transform.position, target.transform.position);
        Vector3 destPos = target.transform.position;
        //Vector3 vel = GetVelocity(head.transform.position, destPos, angle);

        straightDir = (target.transform.position - head.transform.position).normalized;
        //Vector3 dir = (target.transform.position - head.transform.position).normalized;
        //rd.velocity = dir * spd;
        //rd.AddForce(dir/**spd*/, ForceMode.Impulse);
        StartCoroutine(AliveCoroutine());
    }

    //public Vector3 GetVelocity(Vector3 beginPos, Vector3 destPos, float beginAngle)
    //{
    //      이거 지금 싀~~발 가끔 Nan~~ 값 나옴.
            //아마 삼각함수 쪽에서 뭐 문제 생긴거 같은데 일단 패스.
    //    float gravity = Physics.gravity.magnitude;
    //    float angle = beginAngle * Mathf.Deg2Rad;

    //    Vector3 planarTarget = new Vector3(destPos.x, 0, destPos.z);
    //    Vector3 planarPosition = new Vector3(beginPos.x, 0, beginPos.z);

    //    float distance = Vector3.Distance(planarTarget, planarPosition);
    //    float yOffset = beginPos.y - destPos.y;

    //    float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

    //    Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

    //    float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (destPos.x > beginPos.x ? 1 : -1);
    //    Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

    //    return finalVelocity;
    //}



    public IEnumerator AliveCoroutine()
    {
        
        isShoot = true;

        while (time < fullTime)
        {
            time += Time.deltaTime;
            //transform.position += transform.forward * Time.deltaTime * spd;

            yield return null;
        }

        ResetForReturn();
        ObjectPoolingCenter.Instance.ReturnObj(this.gameObject, Enums.ePoolingObj.Arrow);
        //ResetForReturn();
    }

    public void MoveCoroutine()
    { 
        
    
    
    }


    public void Awake()
    {
        //StartCoroutine(AliveCouroutine());
        rd = GetComponent<Rigidbody>();
        rd.centerOfMass = massPivot;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHook)
        {
            transform.position = rightIndexFingerBoneTr.position;
            transform.forward = (bowLeverTr.position - transform.position).normalized;
        }

        if (isShoot)
        {
            mileage += spd * Time.deltaTime;
   
        }


       
	}

	public void FixedUpdate()
	{
        //Vector3 lookDir = destPos - head.transform.position;
        //transform.rotation = Quaternion.LookRotation(lookDir);

        if (isShoot)
        {
            if (mileage < maxRange)
            {
                //float angle = Mathf.Atan2(transform.up, transform.forward) * Mathf.Rad2Deg;
                rd.MovePosition(transform.position + (straightDir * spd * Time.deltaTime));

                //rd.MovePosition(transform.position + (transform.forward * spd * Time.deltaTime));
                beginAngle = transform.rotation;
                endAngle = Quaternion.AngleAxis(90f, transform.right);
                
                //endAngle = transform.Rotate()
                //Quaternion.ro
            }
            else 
            {
                curveTime += Time.deltaTime;
                //transform.rotation = Quaternion.Lerp(beginAngle, endAngle, curveTime *0.2f);
                transform.rotation = Quaternion.Lerp(transform.rotation, endAngle, Time.deltaTime * 9.8f);
                
                rd.MovePosition(transform.position + (transform.forward * spd * Time.deltaTime));
            }


        }
    }

	private void OnEnable()
    {
        
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.black;
        if (target != null)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);

            Gizmos.DrawLine(head.transform.position, target.transform.position);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (transform.rotation * massPivot), 0.1f);

    }

	private void OnTriggerEnter(Collider other)
	{
        if (isShoot
            && (other.CompareTag("Player") || other.CompareTag("Environment")))
        {
            Debug.Log("바닥이나 플레이어한테 박힘");
            GameObject staticArrow = ObjectPoolingCenter.Instance.LentalObj(Enums.ePoolingObj.Arrow_Static);

            staticArrow.transform.position = transform.position;
            staticArrow.transform.rotation = transform.rotation;

            staticArrow.transform.SetParent(other.gameObject.transform);


            ResetForReturn();
            ObjectPoolingCenter.Instance.ReturnObj(this.gameObject, Enums.ePoolingObj.Arrow);
            //박히는건 그냥 static 하나 가지고 와서 자식 오브젝트로 넣으면 될듯 일단 패스
            //rd.velocity = Vector3.zero;
            ////rd.useGravity 
            //rd.isKinematic = true;
        }

        
    }
}
