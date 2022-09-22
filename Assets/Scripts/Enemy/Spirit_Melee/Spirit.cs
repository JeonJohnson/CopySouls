using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : Enemy
{
    public override void InitializeState()
    {
        fsm[(int)Enums.eEnmeyState.Idle] = new Spirit_Idle();

        SetState(Enums.eEnmeyState.Idle);

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
    }

}
