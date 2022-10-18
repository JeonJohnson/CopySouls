using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionTable : MonoBehaviour
{
    #region singletone
    /// <singletone>    
    static public PlayerActionTable instance = null;
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
    }

    #endregion

    public bool isComboCheck = false;
    bool antiRagTrigger = false;

    IEnumerator SetPlayerStatusCoroutine(Enums.ePlayerState state, float time)
    {
        yield return new WaitForSeconds(time);
        Player.instance.SetState(state);
    }

    IEnumerator TakeDamage(int damage, float stunTime = 0.767f)
    {
        Player.instance.hp -= damage;
        Player.instance.animator.SetTrigger("Hit");
        yield return new WaitForSeconds(stunTime);
        Player.instance.SetState(Enums.ePlayerState.Idle);
    }

    IEnumerator PlayerInvincible(float enterTime, float exitTime)
    {
        yield return new WaitForSeconds(enterTime);
        Player.instance.SetPlayerMat(0);
        Player.instance.isInteracting = false;
        yield return new WaitForSeconds(exitTime);
        Player.instance.SetPlayerMat(1);
        Player.instance.isInteracting = true;
    }

    IEnumerator DealDamage(int damage, Enemy enemy)
    {
        yield return null;
    }

    public void Hit(int damage, Vector3 dir)
    {
        isComboCheck = false;
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Hit);
        if (Player.instance.isInteracting == true)
        {
            StopAllCoroutines();
        }
        StartCoroutine(TakeDamage(damage));
    }

    public void Rolling()
    {
        CurCoroCounter2 = CurCoroCounter1;
        isComboCheck = false;
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        EnableWeaponMeshCol(0);
        Player.instance.animator.SetTrigger("Rolling");
        PlayerLocomove.instance.SetPlayerTrSlow();
        StartCoroutine(PlayerInvincible(0.15f, 0.3667f));
    }

    public void Backstep()
    {
        isComboCheck = false;
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        EnableWeaponMeshCol(0);
        Player.instance.animator.SetTrigger("Backstep");
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        StartCoroutine(PlayerInvincible(0.25f, 0.3667f));
    }

    int combo = 0;
    public void WeakAttack()
    {
        CurCoroCounter2 = CurCoroCounter1;
        print("같게함");
        isComboCheck = false;
        Player.instance.SetState(Enums.ePlayerState.Atk);
        EnableWeaponMeshCol(0);
        //PlayerLocomove.instance.SetPlayerTrSlow(PlayerLocomove.instance.isCameraLock);
        Player.instance.animator.SetTrigger("WeakAttack" + "_" + combo.ToString());
        combo++;
        if (combo > 2)
        {
            combo = 0;
        }
        PlayerLocomove.instance.SetPlayerTrSlow(PlayerLocomove.instance.isCameraLock);
    }

    public void StrongAttack()
    {
        isComboCheck = false;
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }

    public void DashAttack()
    {
        isComboCheck = false;
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);
        Player.instance.animator.SetTrigger("DashAttack");
        Player.instance.SetState(Enums.ePlayerState.Atk);
    }

    public void RollingAttack()
    {
        CurCoroCounter2 = CurCoroCounter1;
        print("같게함");
        isComboCheck = false;
        combo = 0;
        Player.instance.SetState(Enums.ePlayerState.Atk);
        EnableWeaponMeshCol(0);
        Player.instance.animator.SetTrigger("RollingAttack");
    }

    public void FrontHoldAttack()
    {
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }

    public void BackHoldAttack()
    {
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }

    public void Parrying()
    {

    }



    //Funcs

    public void SetPlayerStatus(int i)
    {
        print("셋아이들");
        if (antiRagTrigger == false)
        {
            combo = 0;
            isComboCheck = false;
            Player.instance.SetState((Enums.ePlayerState)i);
        }
    }

    #region 콤보시스템 픽스 함수
    int CurCoroCounter1 = 0;
    int CurCoroCounter2 = 0;

    public void StartComboCheck()
    {
        isComboCheck = true;
        CurCoroCounter1++;
    }

    public void StopComboCheck()
    {
        if (CurCoroCounter1 != CurCoroCounter2)
        {
            print(CurCoroCounter1 + " " + CurCoroCounter2);
            isComboCheck = false;
            SetPlayerStatus(0);
        }
    }
    #endregion
    public void EnableWeaponMeshCol(int i)
    {
        Player.instance.mainWeapon.GetComponent<Player_Weapon>().EnableWeaponMeshCollider(i);
    }
}
