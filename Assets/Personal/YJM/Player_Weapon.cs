using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Player_Weapon : MonoBehaviour
{
    public WeaponStatus status;
    int atk = 10;

    MeshCollider meshCollider;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    public void EnableWeaponMeshCollider(int i)
    {
        if (i == 0)
        {
            meshCollider.enabled = false;
        }
        else
        {
            meshCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            print("Deal Damage To Enemy Funcs");
        }
        meshCollider.enabled = false;
    }
}
