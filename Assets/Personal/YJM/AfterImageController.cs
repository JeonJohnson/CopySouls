using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AfterImageController : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer smr;
    public Mesh weaponMesh;
    [SerializeField] Material mat;
    // Start is called before the first frame update
    void Start()
    {
        weaponMesh = Player.instance.status.mainWeapon.gameObject.GetComponent<MeshFilter>().sharedMesh;
    }

    public bool isWeaponEffect = false;
    // Update is called once per frame
    void Update()
    {
        if (isWeaponEffect) MakeWeaponAfterImage();
    }

    public float timer = 0f;

    public void MakeWeaponAfterImage()
    {
        float timer = 0f;
        if(timer < 0.04f)
        {
            timer -= Time.deltaTime;
            GameObject afterImageObj = new GameObject("AfterImage");
            MeshFilter mf = afterImageObj.AddComponent<MeshFilter>();
            mf.mesh = weaponMesh;
            print("¾Ì!~!");
            MeshRenderer mr = afterImageObj.AddComponent<MeshRenderer>();
            mr.material = mat;
            mr.material.color = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, 0.2f);
            mr.material.DOFade(0f, 1f).OnComplete(() => { Destroy(afterImageObj); });
            afterImageObj.transform.position = Player.instance.status.mainWeapon.transform.position;

            afterImageObj.transform.rotation = Player.instance.status.mainWeapon.transform.rotation;
        }
    }

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

    public int sprintCount = 5;
    public void MakeAfterImage()
    {
        if(sprintCount > 0)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                sprintCount--;
                Mesh mesh = new Mesh();
                smr.BakeMesh(mesh);

                GameObject afterImageObj = new GameObject("AfterImage");
                MeshFilter mf = afterImageObj.AddComponent<MeshFilter>();
                mf.mesh = mesh;

                MeshRenderer mr = afterImageObj.AddComponent<MeshRenderer>();
                mr.material = mat;
                mr.material.color = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, 0.1f);
                mr.material.DOFade(0f, 1f).OnComplete(() => { Destroy(afterImageObj); });
                afterImageObj.transform.position = this.transform.position;

                afterImageObj.transform.rotation = this.transform.rotation;
                timer = 0f;

            }
        }
    }

    public void MakeAfterImageCoro(float charge)
    {
        StartCoroutine(MakeAfterCoro(charge));
    }

    IEnumerator MakeAfterCoro(float charge)
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
            mr.material.color = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, 0.1f + charge * 0.9f);
            mr.material.DOFade(0f, 1f).OnComplete(() => { Destroy(afterImageObj); });
            afterImageObj.transform.position = this.transform.position;

            afterImageObj.transform.rotation = this.transform.rotation;
            yield return new WaitForSeconds(0.017f);
        }
    }
}
