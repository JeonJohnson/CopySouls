using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Structs;
using Enums;
using UnityEditor.Animations;

public class Player : MonoBehaviour
{
    public GameObject playerModel;
    public PlayerActionTable playerAt;
    public PlayerLocomove playerLocomove;
    public Animator animator;

    public Transform spine3Tr;
    public Transform headTr;
    public Transform handTr;
    [HideInInspector] public List<Collider> modelColliders = new List<Collider>();
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
        status.isInputtable = true;
    }
    #endregion


    CameraShake cameraShake;
    

    // Start is called before the first frame update
    private void Start()
    {
        //RigidBodySetting();
        SetPlayerWeapon();
    }

    public AnimationClip[] idleAnimClips;

    public void ChangeAnimClipInBlendTree(AnimationClip animClip)
    {
        RuntimeAnimatorController temp = animator.runtimeAnimatorController;
        AnimatorController c = UnityEditor.AssetDatabase.LoadAssetAtPath<AnimatorController>(UnityEditor.AssetDatabase.GetAssetPath(temp)); 
        ChildMotion[] tempMotions = ((BlendTree)c.layers[0].stateMachine.states[0].state.motion).children;
        print(c.layers[0].stateMachine.states[0].state.name);
        print(tempMotions[0]);
        tempMotions[0].motion = animClip;
        ((BlendTree)c.layers[0].stateMachine.states[0].state.motion).children = tempMotions;
    }

    // Update is called once per frame
    private void Update()
    {

        curState.UpdateState();
        if(Input.GetKeyDown(KeyCode.Y))
        {
            //UnitManager.Instance.SpawnTestEnemy(this.transform.position);
            //DamagedStruct dmgst = new DamagedStruct();
            //dmgst.atkType = eAttackType.Strong;
            //dmgst.dmg = 1f;
            //PlayerActionTable.instance.Hit(dmgst);
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            //if (Inventory.Instance.InventoryBase.activeSelf)
            //{
            //    Inventory.Instance.TryOpenInventory();
            //    
            //    //Inventory.Instance.InventoryBase.SetActive(false);
            //    //Inventory.inventoryActivated = false;
            //}
            //else
            //{
            //    Inventory.Instance.TryOpenInventory();
            //    //Inventory.Instance.InventoryBase.SetActive(true);
            //    //Inventory.inventoryActivated = true;
            //}
        }

        if (status.RightHand != status.mainWeapon)
        {
            status.mainWeapon.GetComponent<Player_Weapon>().item_Weapon.SetAsMainWeapon();
            status.mainWeapon = status.RightHand.GetComponent<Player_Weapon>();
            PlayerActionTable.instance.ChangeWeaponHoldType(false);
            PlayerActionTable.instance.holdType = false;
        }
        if (status.LeftHand != status.subWeapon)
        {
            status.subWeapon.GetComponent<Player_Weapon>().item_Weapon.SetAsSubWeapon();
            status.subWeapon = status.LeftHand.GetComponent<Player_Weapon>();
            PlayerActionTable.instance.ChangeWeaponHoldType(false);
            PlayerActionTable.instance.holdType = false;
        }
    }

    public void ActivatePlayerInput(bool b)
    {
        if(b == true)
        {
            Player.instance.status.isInputtable = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Player.instance.status.isInputtable = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void SetPlayerWeapon()
    {
        status.mainWeapon = status.RightHand.gameObject.GetComponent<Weapon>();
        status.mainWeapon.GetComponent<Player_Weapon>().owner = this.gameObject;
        //status.mainWeapon.GetComponent<Item_Weapon>().PlayFuncs();
        //ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[2]);// юс╫ц

        status.subWeapon = status.LeftHand.gameObject.GetComponent<Weapon>();
        status.subWeapon.GetComponent<Player_Weapon>().owner = this.gameObject;
        //ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[2]);

        PlayerActionTable.instance.ChangeWeaponHoldType(false);
        PlayerActionTable.instance.holdType = false;
    }

    void InitializeState()
    {
        fsm = new Player_cState[(int)ePlayerState.End];
        fsm[(int)ePlayerState.Idle] = new Player_Idle();
        fsm[(int)ePlayerState.Move] = new Player_Move();
        fsm[(int)ePlayerState.Hit] = new Player_Hit();
        fsm[(int)ePlayerState.Dodge] = new Player_Dodge();
        fsm[(int)ePlayerState.Atk] = new Player_Atk();
        fsm[(int)ePlayerState.Interacting] = new Player_Interacting();
        SetState(Enums.ePlayerState.Idle);

        status.interactionRange = 2f;
    }

    void ColliderSetting()
    {
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.GetMask("Player_Hitbox"))
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
}
