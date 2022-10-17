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

    public bool isCombo = false;
    bool antiRagTrigger = false;

    IEnumerator SetPlayerStatusCoroutine(Enums.ePlayerState state ,float time)
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
        isCombo = false;
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
        print("스테이트 롤링");
        isCombo = false;
        AntiRagCoro = StartCoroutine(AntiRag());
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        EnableWeaponMeshCol(0);
        Player.instance.animator.SetTrigger("Rolling");
        PlayerLocomove.instance.SetPlayerTrSlow();
        StartCoroutine(PlayerInvincible(0.15f, 0.3667f));
    }

    public void Backstep()
    {
        isCombo = false;
        AntiRagCoro = StartCoroutine(AntiRag());
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        EnableWeaponMeshCol(0);
        Player.instance.animator.SetTrigger("Backstep");
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        StartCoroutine(PlayerInvincible(0.25f, 0.3667f));
    }

    int combo = 0;
    public void WeakAttack()
    {
        isCombo = false;
        AntiRagCoro = StartCoroutine(AntiRag());
        Player.instance.SetState(Enums.ePlayerState.Atk);
        EnableWeaponMeshCol(0);
        //PlayerLocomove.instance.SetPlayerTrSlow(PlayerLocomove.instance.isCameraLock);
        Player.instance.animator.SetTrigger("WeakAttack" + "_" + combo.ToString());
        combo++;
        if(combo >2)
        {
            combo = 0;
        }
        PlayerLocomove.instance.SetPlayerTrSlow(PlayerLocomove.instance.isCameraLock);
    }

    public void StrongAttack()
    {
        isCombo = false;
        AntiRagCoro = StartCoroutine(AntiRag());
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }

    public void DashAttack()
    {
        isCombo = false;
        AntiRagCoro = StartCoroutine(AntiRag());
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);
        Player.instance.animator.SetTrigger("DashAttack");
        Player.instance.SetState(Enums.ePlayerState.Atk);
    }

    public void RollingAttack()
    {
        AntiRagCoro = StartCoroutine(AntiRag());
        isCombo = false;
        combo = 0;
        Player.instance.SetState(Enums.ePlayerState.Atk);
        EnableWeaponMeshCol(0);
        Player.instance.animator.SetTrigger("RollingAttack");
    }

    public void FrontHoldAttack()
    {
        AntiRagCoro = StartCoroutine(AntiRag());
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }
    
    public void BackHoldAttack()
    {
        AntiRagCoro = StartCoroutine(AntiRag());
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }



    //Funcs

    public void SetPlayerStatus(int i)
    {
        print("SetPlayerStatus 호출" + antiRagTrigger);
        if (antiRagTrigger == false)
        {
            print("스테이트 아이들");
            combo = 0;
            isCombo = false;
            Player.instance.SetState((Enums.ePlayerState)i);
        }
    }

    public void StartComboCheck()
    {
        isCombo = true;
    }

    public void StopComboCheck()
    {
        if(antiRagTrigger == false)
        {
            print("콤보 off");
            isCombo = false;
        }
    }

    Coroutine AntiRagCoro;
    IEnumerator AntiRag()
    {
        antiRagTrigger = true;
        print(antiRagTrigger);
        yield return new WaitForSeconds(0.15f);
        print("끝");
        antiRagTrigger = false;
        yield return null;
    }

    public void EnableWeaponMeshCol(int i)
    {
        Player.instance.Weapon.GetComponent<Player_Weapon>().EnableWeaponMeshCollider(i);
    }
}
