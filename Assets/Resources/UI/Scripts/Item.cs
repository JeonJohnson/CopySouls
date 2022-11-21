using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Enums.ObjNameTable objName;
    public Enums.ObjectType ObjectType;
    public Rigidbody rigid;
    public Enums.ItemType itemType;
    public Sprite itemImage;

    public abstract void Initialize();

    protected virtual void Awake()
    {
        Initialize();
    }
    protected virtual void Start()
    {
    }
    protected virtual void Update()
    {
    }

    public virtual void PlayFuncs()
    {

    }
}
