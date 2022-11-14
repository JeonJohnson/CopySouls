using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ViewCastInfo
{
    public bool hit;
    public Vector3 point;
    public float dst;
    public float angle;

    public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
    {
        hit = _hit;
        point = _point;
        dst = _dst;
        angle = _angle;
    }
}

public class FieldOfView : MonoBehaviour
{
    public Enemy me;
    public Transform HeadPos;

    public float viewRadius;
    [Range(1, 360)]
    public float viewAngle;
    public LayerMask targetMask, targetHeadMask, obstacleMask;

    public List<Transform> findObj = new List<Transform>();
    public float meshResolution;
    Mesh viewMesh;
    public MeshFilter viewMeshFilter;

    void Start()
    {
        viewRadius = me.status.ricognitionRange;
        targetMask = LayerMask.GetMask("Player");
        obstacleMask = LayerMask.GetMask("Environment");
        //obstacleMask = LayerMask.GetMask("Obstacle");
        targetHeadMask = LayerMask.GetMask("Player_Hitbox");

        HeadPos = ((Spirit)me).headPos;
        ((Spirit)me).targetHeadPos = Player.instance.headTr;

        viewMeshFilter = gameObject.GetComponentInChildren<MeshFilter>();

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

    }

    void Update()
    {
        FindTargets();
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }





    //========================================================================================================================
    //FindObject

    void FindTargets()
    {
        findObj.Clear();
        Collider[] hitObjs = Physics.OverlapSphere(transform.position, viewRadius);

        if (hitObjs.Length == 0) return;

        for (int i = 0; i < hitObjs.Length; i++)
        {
            Transform target = hitObjs[i].transform;
            //Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 dirToTarget = (target.position - HeadPos.position).normalized;

            if (viewAngle <= 180f && viewAngle > 0f)
            {
                if (InnerProduct_Rad(transform.forward, dirToTarget, false) < viewAngle / 2)
                {
                    //float distToTarget = Vector3.Distance(transform.position, target.transform.position);
                    float distToTarget = Vector3.Distance(HeadPos.position, target.transform.position);

                    
                    //if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    if (!Physics.Raycast(HeadPos.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        if (target.gameObject == gameObject) continue;

                        if(target.gameObject.name == "Head") findObj.Add(target);

                        //else
                        //{
                        //    if (target.gameObject.layer == targetMask) findObj.Add(target);
                        //}
                    }
                }
            }
            else if (viewAngle > 180f && viewAngle <= 360f)
            {
                float distToTarget = Vector3.Distance(HeadPos.position, target.transform.position);
                //if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                if (!Physics.Raycast(HeadPos.position, dirToTarget, distToTarget, obstacleMask))

                {
                    if (target.gameObject == gameObject) continue;

                if (target.gameObject.name == "Head") findObj.Add(target);
                //else
                //{
                //    if (target.gameObject.layer == targetMask) findObj.Add(target);
                //}
                }
            }
        }

        bool isFind = false;

        //Debug.Log(findObj.Count);

        if (findObj.Count != 0)
        {
            foreach (Transform target in findObj)
            {
                GameObject obj = target.gameObject;
                if (obj.layer == 14)
                {
                    isFind = true;
                }
            }
        }
        else isFind = false;

        if (isFind)
        {
            me.combatState = eCombatState.Alert;
            //Debug.Log("���߰�!");
        }
        else
        {
            me.combatState = eCombatState.Idle;
            //Debug.Log("�������!");
        }
    }
    //========================================================================================================================

    //========================================================================================================================
    //FOV_Draw

    void DrawFieldOfView()
    {
        //Mathf.RoundToInt(float) -> �ݿø�
        //Mathf.CeilToInt(float) -> �ø�
        //Mathf.FloorToInt(float) -> ����

        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;

            ViewCastInfo newViewCast = RayToView(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    //========================================================================================================================

    //========================================================================================================================
    //RayCast -> ViewScatInfo

    ViewCastInfo RayToView(float angle)
    {
        //Ray����
        Vector3 dir = DirFromAngle(angle,true);
        //Ray����
        RaycastHit hit;

        //ref = c++���� ���۷����� ����(������)
        //out = out�޼���� ref�� ����� �����ϳ� �Ű������� ����� �������� ������ ������ ������ ��
        //����, out�� �ش� �Լ� ������ ���� �д°͵� �Ұ���
        //��, ������ �ο�
        //RaycastHit->point = Ray�� ���� ��ġ
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, angle);
        }
    }

    //========================================================================================================================

    //========================================================================================================================
    //

    public Vector3 DirFromAngle(float angleDegree, bool angleIsGlobal)
    {
        if (!angleIsGlobal) angleDegree += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegree * Mathf.Deg2Rad));
    }


    //========================================================================================================================
    //InnerProduct

    public float InnerProduct_Rad(Vector3 a, Vector3 b, bool radian)
    {
        float lengthA = a.sqrMagnitude;
        float lengthB = b.sqrMagnitude;

        float Rad_Inner = Mathf.Acos(((a.x * b.x) + (a.y * b.y) + (a.z * b.z)) / Mathf.Sqrt(lengthA * lengthB));
        float Deg_Inner = Rad_Inner * Mathf.Rad2Deg;

        if (radian) return Rad_Inner;
        else return Deg_Inner;
    }
    
    //========================================================================================================================

}
 