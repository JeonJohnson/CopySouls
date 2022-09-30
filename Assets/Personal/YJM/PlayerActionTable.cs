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

    public void SetWeaponCol(int i)
    {
        if (i == 0)
        {
            //무기판정 off
            
        }
        else
        {
            //무기판정 on
        }
    }

    public void Hit(int damage)
    {
        Player.instance.SetState(Enums.ePlayerState.Hit);
        if (Player.instance.isInteracting == true)
        {
            StopAllCoroutines();
        }
            StartCoroutine(TakeDamage(damage));
    }

    public void Rolling()
    {
        Player.instance.animator.SetTrigger("Rolling");
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        StartCoroutine(PlayerInvincible(0.15f, 0.3667f));
    }

    public void Backstep()
    {
        Player.instance.animator.SetTrigger("Backstep");
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        StartCoroutine(PlayerInvincible(0.25f, 0.3667f));
    }

    public void WeakAttack()
    {
        Player.instance.animator.SetTrigger("WeakAttack");
        Player.instance.SetState(Enums.ePlayerState.Atk);
    }

    public void StrongAttack()
    {
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }

    public void DashAttack()
    {
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }

    public void FrontHoldAttack()
    {
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }
    
    public void BackHoldAttack()
    {
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }

    public void SetPlayerStatus(int i)
    {
        Player.instance.SetState((Enums.ePlayerState)i);
    }
}
