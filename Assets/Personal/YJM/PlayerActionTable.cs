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
        PlayerLocomove.instance.SetPlayerTrSlow();
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        StartCoroutine(PlayerInvincible(0.15f, 0.3667f));
    }

    public void Backstep()
    {
        Player.instance.animator.SetTrigger("Backstep");
        Player.instance.SetState(Enums.ePlayerState.Dodge);
        StartCoroutine(PlayerInvincible(0.25f, 0.3667f));
    }

    int combo = 0;
    public void WeakAttack()
    {
        //PlayerLocomove.instance.SetPlayerTrSlow(PlayerLocomove.instance.isCameraLock);
        Player.instance.animator.SetTrigger("WeakAttack" + "_" + combo.ToString());
        Player.instance.SetState(Enums.ePlayerState.Atk);
        combo++;
        if(combo >2)
        {
            combo = 0;
        }
    }

    public void StrongAttack()
    {
        Player.instance.SetState(Enums.ePlayerState.Atk);

        StartCoroutine(SetPlayerStatusCoroutine(Enums.ePlayerState.Idle, 1.733f));
    }

    public void DashAttack()
    {
        Player.instance.animator.SetTrigger("DashAttack");
        Player.instance.SetState(Enums.ePlayerState.Atk);
    }

    public void RollingAttack()
    {
        Player.instance.animator.SetTrigger("RollingAttack");
        Player.instance.SetState(Enums.ePlayerState.Atk);
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



    //Funcs

    public void SetPlayerStatus(int i)
    {
        Player.instance.SetState((Enums.ePlayerState)i);
    }

    public void ComboAttackCheck()
    {
        StartCoroutine(ComboInput());
    }

    float _timer;
    bool isPlaying;
    bool isNextComboInput = false;
    IEnumerator ComboInput()
    {
        _timer = 0.2f;
        isPlaying = true;

        while (_timer > 0 && isPlaying)
        {
            _timer -= Time.deltaTime;
            if (Input.GetButtonDown("Fire1"))
            {
                isNextComboInput = true;
            }
            yield return null;

            if (_timer <= 0)
            {
                if (isNextComboInput == true)
                {
                    WeakAttack();
                    isNextComboInput = false;
                }
                else
                {
                    combo = 0;
                    SetPlayerStatus(0);
                }
            }
        }
        #region
        //Combo Input을 실행시키는 코루틴 함수를 팜 밑은 내용
        //타이머 하나 만들고
        //타이머 중에 공격버튼이 입력되면
        //타이머 끝나고 다음콤보가 재생되게
        //아니면 타이머 끝나면 플레이어 스테이터스를 아이들로

        //그리고 그 코루틴함수를 애니메이션 끝나갈때쯤에 부착! 완성!
        #endregion
    }

    public void RollingAttackCheck()
    {
            StartCoroutine(RollingAttackInput());
    }

    IEnumerator RollingAttackInput()
    {
        _timer = 0.2f;
        isPlaying = true;

        while (_timer > 0 && isPlaying)
        {
            _timer -= Time.deltaTime;
            if (Input.GetButtonDown("Fire1") && Player.instance.curState_e == Enums.ePlayerState.Dodge)
            {
                RollingAttack();
            }
            yield return null;
        }
    }

    public void EnableWeaponMeshCol(int i)
    {
        Player.instance.Weapon.GetComponent<Player_Weapon>().EnableWeaponMeshCollider(i);
    }
}
