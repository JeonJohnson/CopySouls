using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;
using Enums;

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

    [SerializeField] Player player;
    public bool isComboCheck = false;
    bool antiRagTrigger = false;
    public float curActAtkValue = 1.0f;

    IEnumerator SetPlayerStatusCoroutine(Enums.ePlayerState state, float time)
    {
        yield return new WaitForSeconds(time);
        Player.instance.SetState(state);
    }

    IEnumerator TakeDamageCoro(int damage, float stunTime = 0.767f)
    {
        curActAtkValue = 1.0f;
        player.status.curHp -= damage;
        if (player.status.curHp > 0)
        {
            Player.instance.animator.SetTrigger("Hit");
            yield return new WaitForSeconds(stunTime);
            Player.instance.SetState(Enums.ePlayerState.Idle);
        }
        else
        {
            Death();
        }
    }

    public void Death()
    {
        Player.instance.SetState(Enums.ePlayerState.Death);
        Player.instance.animator.SetTrigger("Death");
        print("사망");
    }

    public void isParryingCheck(int i)
    { 
        if(i == 0)
        {
            player.status.isParrying = false;
        }
        else
        {
            player.status.isParrying = true;
        }
    }

    public void Parrying()
    {
        Player.instance.status.curStamina -= 15;
        CurCoroCounter2 = CurCoroCounter1;
        isComboCheck = false;
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        Player.instance.animator.SetTrigger("Parrying");
    }

    IEnumerator PlayerInvincible(float enterTime, float exitTime)
    {
        yield return new WaitForSeconds(enterTime);
        Player.instance.SetPlayerMat(0);
        player.status.isInvincible = false;
        player.SetModelCollider(false);
        yield return new WaitForSeconds(exitTime);
        Player.instance.SetPlayerMat(1);
        player.status.isInvincible = true;
        player.SetModelCollider(true);
    }

    IEnumerator DealDamage(int damage, Enemy enemy)
    {
        yield return null;
    }

    public void Hit(DamagedStruct dmgStruct)
    {
        if(player.status.isParrying == true && dmgStruct.atkType == eAttackType.Week)
        {
            print("적 isRiposte" + dmgStruct.isRiposte + "공격 패링함");
        }
        else if(player.status.isGuard == true)
        {
            player.status.curStamina -= 10f;
                float dot = Vector3.Dot(dmgStruct.attackObj.transform.forward, -Player.instance.playerModel.transform.forward);
                float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if (theta <= 35f)
                {
                    if (dmgStruct.atkType == eAttackType.Week && player.status.curStamina > 0f)
                    {
                        print("attacker : " + dmgStruct.attackObj + "방어성공 " + theta);
                        player.animator.SetTrigger("GuardSuccess");
                    }
                    else
                    {
                        player.animator.SetTrigger("GuardFail");
                        SetPlayerStatus((int)ePlayerState.Hit);
                    print("방어실패");
                    }
                }
                else
                {
                    TakeDamage((int)dmgStruct.dmg);
                }
        }
        else
        {
            TakeDamage((int)dmgStruct.dmg);
        }
    }

    void TakeDamage(float damage)
    {
        player.status.isParrying = false;
        isComboCheck = false;
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Hit);
        StopAllCoroutines();
        player.SetModelCollider(true);
        StartCoroutine(TakeDamageCoro((int)damage));
    }


    float isActedTimer = 0.5f;
    public void UpdateStamina()
    {
        if(isActedTimer >= 0f)
        {
            isActedTimer -= Time.deltaTime;
        }
        else
        {
            Player.instance.status.curStamina += Time.deltaTime * 20;
            Player.instance.status.curStamina = Mathf.Clamp(Player.instance.status.curStamina, 0, Player.instance.status.maxStamina);
        }
    }

    public void Rolling()
    {
        isActedTimer = 0.5f;
        Player.instance.status.curStamina -= 20;
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
        isActedTimer = 0.5f;
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
        curActAtkValue = 1.0f;
        isActedTimer = 0.5f;
        Player.instance.status.curStamina -= 10;
        CurCoroCounter2 = CurCoroCounter1;
        isComboCheck = false;
        Player.instance.SetState(Enums.ePlayerState.Atk);
        EnableWeaponMeshCol(0);
        //PlayerLocomove.instance.SetPlayerTrSlow(PlayerLocomove.instance.isCameraLock);
        Player.instance.animator.SetTrigger("WeakAttack" + "_" + combo.ToString());
        combo++;
        if (combo > 1)
        {
            combo = 0;
        }
        PlayerLocomove.instance.SetPlayerTrSlow(PlayerLocomove.instance.isCameraLock,0.4f);
    }

    public void StrongAttack()
    {
        curActAtkValue = 1.5f;
        isActedTimer = 0.5f;
        isComboCheck = false;
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);

    }

    public void ChargeAttack()
    {
        isActedTimer = 0.5f;
        isComboCheck = false;
        CurCoroCounter2 = CurCoroCounter1;
        Player.instance.SetState(Enums.ePlayerState.Atk);
        Player.instance.animator.SetTrigger("ChargeAttack");
        Player.instance.animator.SetFloat("ChargeAnimSpeed", 0.8f);
    }

    public void ChargeAttackCheck()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartCoroutine(PlayRemainChargeFuncs());
        }
        else
        {
            curActAtkValue = 1.2f;
            Player.instance.animator.SetFloat("ChargeAnimSpeed", 1f);
            Player.instance.status.curStamina -= 35;
        }
    }

    IEnumerator PlayRemainChargeFuncs()
    {
        Player.instance.animator.SetFloat("ChargeAnimSpeed", 0.05f);
        float timer = 1f;
        while(timer > 0f)
        {
            curActAtkValue = 1.0f + ((1 - timer) * 3.0f);
            timer -= Time.deltaTime;
            yield return null;
            if(Input.GetKey(KeyCode.Mouse0) == false)
            {
                Player.instance.status.curStamina -= 35 + ((1 - timer) * 15);
                PlayerLocomove.instance.SetPlayerTrSlow();
                Player.instance.animator.SetFloat("ChargeAnimSpeed", 1f);
                yield break;
            }
        }
        Player.instance.status.curStamina -= 35 + ((1 - timer) * 15);
        PlayerLocomove.instance.SetPlayerTrSlow();
        Player.instance.animator.SetFloat("ChargeAnimSpeed", 1f);
    }

    public void DashAttack()
    {
        curActAtkValue = 1.2f;
        isActedTimer = 0.5f;
        Player.instance.status.curStamina -= 15;
        isComboCheck = false;
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);
        Player.instance.animator.SetTrigger("DashAttack");
        PlayerLocomove.instance.SetPlayerTrSlow(PlayerLocomove.instance.isCameraLock);
    }

    public void RollingAttack()
    {
        curActAtkValue = 1.2f;
        isActedTimer = 0.5f;
        Player.instance.status.curStamina -= 15;
        CurCoroCounter2 = CurCoroCounter1;
        isComboCheck = false;
        combo = 0;
        Player.instance.SetState(Enums.ePlayerState.Atk);
        EnableWeaponMeshCol(0);
        Player.instance.animator.SetTrigger("RollingAttack");
        PlayerLocomove.instance.SetPlayerTrSlow(PlayerLocomove.instance.isCameraLock, 0.4f);
    }

    public void FrontHoldAttack()
    {
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);
    }

    public void BackHoldAttack()
    {
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);
        Player.instance.animator.SetTrigger("BackHoldAttack");
    }

    float guardParam = 0;
    public void Guard()
    {
        if(Input.GetKey(KeyCode.Mouse1))
        {
            guardParam += Time.deltaTime * 10;
        }    
        else
        {
            guardParam -= Time.deltaTime * 10;
        }
        guardParam = Mathf.Clamp(guardParam, 0, 1);
        if (guardParam >= 0.95)
        {
            player.status.isGuard = true;
        }
        else
        {
            player.status.isGuard = false;
        }
        player.animator.SetLayerWeight(1, guardParam);
    }
    public void ResetGuardValue()
    {
        guardParam = 0;
        player.animator.SetLayerWeight(1, 0);
    }

    //Funcs

    public void SetPlayerStatus(int i)
    {
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
        print(CurCoroCounter1 + " " + CurCoroCounter2);
        if (CurCoroCounter1 != CurCoroCounter2)
        {
            isComboCheck = false;
            SetPlayerStatus(0);
        }
    }
    #endregion
    public void EnableWeaponMeshCol(int i)
    {
        player.status.mainWeapon.GetComponent<Player_Weapon>().EnableWeaponMeshCollider(i);
    }
    
    public bool StaminaCheck()
    {
        if(Player.instance.status.curStamina <= 1f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
