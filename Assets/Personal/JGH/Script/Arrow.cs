using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IPoolingObject
{
    public Archer archer;

    public Rigidbody rd;
    
    public Transform rightIndexFingerBoneTr;
    public Transform bowLeverTr;
    public GameObject arrowHead;

    public float maxRange;
    public float spd;// m/s 

    public GameObject target;

    public Vector3 straightDir;
    public Quaternion beginAngle;
    public Quaternion endAngle;
    public float mileage = 0f;

    public float time = 0;
    public float fullTime = 10f;

    public bool isShoot = false;
    public bool isHook = false;



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
        isShoot = true;

        straightDir = (target.transform.position - arrowHead.transform.position).normalized;
      
        StartCoroutine(AliveCoroutine());
    }

    #region test
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
    #endregion


    public IEnumerator AliveCoroutine()
    {
        while (time < fullTime)
        {
            time += Time.deltaTime;

            yield return null;
        }

        ResetForReturn();
        ObjectPoolingCenter.Instance.ReturnObj(this.gameObject, Enums.ePoolingObj.Arrow);
    }

    public void Awake()
    {
        rd = GetComponent<Rigidbody>();

        if (arrowHead == null)
        { 
            arrowHead = Funcs.FindGameObjectInChildrenByName(this.gameObject,"arrowhead");
        }
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
        if (isShoot)
        {
            if (mileage < maxRange)
            {
                //rd.MovePosition(transform.position + (transform.forward * spd * Time.deltaTime));
                rd.MovePosition(transform.position + (straightDir * spd * Time.deltaTime));
                
                //나중에 요거 한번만 호출되도록 하기 
                beginAngle = transform.rotation;
                endAngle = Quaternion.AngleAxis(90f, Vector3.right);
                //나중에 요거 한번만 호출되도록 하기 
            }
            else 
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, endAngle, Time.deltaTime * 4f);
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
            Gizmos.DrawLine(arrowHead.transform.position, target.transform.position);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (transform.rotation * new Vector3(0f,0f,1f)), 0.075f);

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
        }

        
    }
}
