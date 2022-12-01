using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AfterImageController : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer smr;
    [SerializeField] Material mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float timer = 0f;

    public void MakeSingleAfterImage()
    {
        Mesh mesh = new Mesh();
        smr.BakeMesh(mesh);

        GameObject afterImageObj = new GameObject("AfterImage");
        MeshFilter mf = afterImageObj.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        MeshRenderer mr = afterImageObj.AddComponent<MeshRenderer>();
        mr.material = mat;
        mr.material.DOFade(0f, 1f).OnComplete(() => { Destroy(afterImageObj); });
        afterImageObj.transform.position = this.transform.position;

        afterImageObj.transform.rotation = this.transform.rotation;
    }

    public void MakeAfterImage()
    {
        timer += Time.deltaTime;
        if(timer > 0.1f)
        {
            Mesh mesh = new Mesh();
            smr.BakeMesh(mesh);

            GameObject afterImageObj = new GameObject("AfterImage");
            MeshFilter mf = afterImageObj.AddComponent<MeshFilter>();
            mf.mesh = mesh;

            MeshRenderer mr = afterImageObj.AddComponent<MeshRenderer>();
            mr.material = mat;
            mr.material.DOFade(0f, 1f).OnComplete(() => { Destroy(afterImageObj); });
            afterImageObj.transform.position = this.transform.position;

            afterImageObj.transform.rotation = this.transform.rotation;
            timer = 0f;
        }
    }

    public void MakeAfterImageCoro()
    {
        StartCoroutine(MakeAfterCoro());
    }

    IEnumerator MakeAfterCoro()
    {
        int count = 0;
        while(count < 6)
        {
            count++;
            Mesh mesh = new Mesh();
            smr.BakeMesh(mesh);

            GameObject afterImageObj = new GameObject("AfterImage");
            MeshFilter mf = afterImageObj.AddComponent<MeshFilter>();
            mf.mesh = mesh;

            MeshRenderer mr = afterImageObj.AddComponent<MeshRenderer>();
            mr.material = mat;
            mr.material.DOFade(0f, 1f).OnComplete(() => { Destroy(afterImageObj); });
            afterImageObj.transform.position = this.transform.position;

            afterImageObj.transform.rotation = this.transform.rotation;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
