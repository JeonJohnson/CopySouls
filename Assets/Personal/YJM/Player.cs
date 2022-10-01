using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enums;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject playerModel;
    public Animator animator;

    public bool isInteracting = true;

    public int hp = 100;
    public int mp = 100;
    public float stamina = 100;

    //FSM
    public Player_cState[] fsm;
    public Player_cState preState = null;
    public ePlayerState preState_e = ePlayerState.End;
    public Player_cState curState = null;
    public ePlayerState curState_e = ePlayerState.End;

    public GameObject Weapon;

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
    }

    #endregion

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        curState.UpdateState();

        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerActionTable.instance.Hit(5);
        }
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
