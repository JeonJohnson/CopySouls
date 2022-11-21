using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Portion : Item
{
    public override void Initialize()
    {

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
        base.Start();
    }

    public override void PlayFuncs()
    {
        base.PlayFuncs();
        PlayerActionTable.instance.UseFood();
        print("»ç¿ë");
    }
}
