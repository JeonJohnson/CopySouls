using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Structs;
using Enums;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject playerModel;
    public Animator animator;

    public List<Collider> modelColliders = new List<Collider>();
    public PlayerStatus status;

    //FSM
    public Player_cState[] fsm;
    public Player_cState preState = null;
    public ePlayerState preState_e = ePlayerState.End;
    public Player_cState curState = null;
    public ePlayerState curState_e = ePlayerState.End;

    public void SetState(ePlayerState state)
    {
        if (state == curState_e || fsm[(int)state] == null)
        {
            return;
        }

        if (curState_e != ePlayerState.End)
        { curState.ExitState(); }

        preState = curState;
        preState_e = curState_e;

        curState_e = state;
        curState = fsm[(int)state];

        curState.EnterState(this);
    }


    #region singletone and InitializeState
    /// <singletone>    
    static public Player instance = null;
    /// <singletone>

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        InitializeState();
        ColliderSetting();
    }

    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        //RigidBodySetting();
    }

    // Update is called once per frame
    private void Update()
    {
        curState.UpdateState();
    }

    void InitializeState()
    {
        fsm = new Player_cState[(int)ePlayerState.End];
        fsm[(int)ePlayerState.Idle] = new Player_Idle();
        fsm[(int)ePlayerState.Move] = new Player_Move();
        fsm[(int)ePlayerState.Hit] = new Player_Hit();
        fsm[(int)ePlayerState.Dodge] = new Player_Dodge();
        fsm[(int)ePlayerState.Atk] = new Player_Atk();
        SetState(Enums.ePlayerState.Idle);
    }

    void ColliderSetting()
    {
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            if (collider.gameObject.layer == 14)
            {
                modelColliders.Add(collider);
            }
        }
    }

    public void SetModelCollider(bool value)
    {
            for (int i = 0; i < modelColliders.Count; i++)
            {
                modelColliders[i].enabled = value;
            }
    }

    /**
     * old code
    void RigidBodySetting()
    {
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            if (rigidBody.gameObject.tag != "Weapon")
            {
                HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            }
        }
    }
    */
    #region PLAYER INVINCIBLE TEST CODE
    [Header("TEST CODE")]
    [SerializeField] Material testMat_0;
    [SerializeField] Material testMat_1;

    public void SetPlayerMat(int i)
    {
        if (i == 0)
        {
            playerModel.transform.Find("Character_Hero_Knight_Male").GetComponent<SkinnedMeshRenderer>().material = testMat_1;
        }
        else
        {
            playerModel.transform.Find("Character_Hero_Knight_Male").GetComponent<SkinnedMeshRenderer>().material = testMat_0;
        }
    }
    #endregion
}
