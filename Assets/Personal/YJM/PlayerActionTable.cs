﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;
using Enums;
using System;
using System.Runtime.CompilerServices;

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

    IEnumerator TakeDamageCoro(DamagedStruct dmgStruct, float stunTime = 0.767f)
    {
        curActAtkValue = 1.0f;
        player.status.curHp -= (int)dmgStruct.dmg;
        if (player.status.curHp > 0)
        {
            if (dmgStruct.atkType == eAttackType.Week)
            {
                Player.instance.animator.SetTrigger("Hit");
            }
            else
            {
                stunTime = 1.9f;
                Player.instance.animator.SetTrigger("Hit_Hard");
            }
            yield return null;
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
        EnableWeaponMeshCol(0);
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
        player.status.isInvincible = true;
        player.SetModelCollider(false);
        yield return new WaitForSeconds(exitTime);
        player.status.isInvincible = false;
        player.SetModelCollider(true);
    }

    public void SetPlayerInvincible(int i)
    {
        if(i == 0)
        {
            player.SetModelCollider(true);
            player.status.isInvincible = false;
        }
        else
        {
            player.SetModelCollider(false);
            player.status.isInvincible = true;
        }
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
                    TakeDamage(dmgStruct);
                }
        }
        else if(player.status.isInvincible == true)
        {
            print("회피함");
        }
        else
        {
            TakeDamage(dmgStruct);
        }
    }

    void TakeDamage(DamagedStruct dmgStruct)
    {
        player.status.isParrying = false;
        isComboCheck = false;
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Hit);
        StopAllCoroutines();
        player.SetModelCollider(true);
        StartCoroutine(TakeDamageCoro(dmgStruct));
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
            if(Player.instance.status.isGuard == true)
            {
                Player.instance.status.curStamina += Time.deltaTime * 5;
            }
            else
            {
                Player.instance.status.curStamina += Time.deltaTime * 20;
            }
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

    public bool HoldAttackCheck()
    {
        CurCoroCounter2 = CurCoroCounter1;
        isComboCheck = false;

        //1. 적 리스트 돌아서 가장 인근한 적 확인
        //2. 거리가 2f 이내이고,  적이 스턴인지 or 플레이어를 인식 못했는지 확인
        //3. 각도 계산해서 30도 이내이면 거기에 맞는 앞잡/뒤잡 실행
        bool isAct = false;

        Enemy target = null;
        float distance = float.MaxValue;
        for (int i = 0; i < UnitManager.Instance.aliveEnemyList.Count; i++)
        {
            if (Vector3.Distance(UnitManager.Instance.aliveEnemyList[i].transform.position, this.transform.position) < distance)
            {
                distance = Vector3.Distance(UnitManager.Instance.aliveEnemyList[i].transform.position, this.transform.position);
                print(distance);
                target = UnitManager.Instance.aliveEnemyList[i];
            }
        }

        if (target != null)
        {
            if (!target.status.isDead && distance <= 2.5f )
            {
                float dot = Vector3.Dot(target.transform.forward, -Player.instance.playerModel.transform.forward);
                float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

                Vector3 playerFrontpos = transform.position + Player.instance.playerModel.transform.forward * 1f;

				if (theta < 90 && target.status.isGroggy && !target.status.isFrontHold)
				{
					FrontHoldAttack(transform, playerFrontpos, target);
					isAct = true;
				}
				else if (theta >= 135 && !target.status.isBackHold)
				{
					BackHoldAttack(transform, playerFrontpos, target);
					Debug.Log(transform.localRotation);
					isAct = true;
				}
				else
				{
					print("각도 :" + theta + "잡기실패");
					isAct = false;
				}
			}

			#region original

			//if (distance <= 2.5f && (target.status.isGroggy == true | target.combatState == eCombatState.Alert))
			//{
			//    float dot = Vector3.Dot(target.transform.forward, - Player.instance.playerModel.transform.forward);
			//    float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

			//    Vector3 playerFrontpos = transform.position + Player.instance.playerModel.transform.forward * 1f;

			//    if (theta < 90)
			//    {
			//        if(target.status.isGroggy == true)
			//        {
			//            FrontHoldAttack(transform, playerFrontpos, target);
			//            isAct = true;
			//        }
			//        else
			//        {
			//            isAct = false;
			//        }
			//    }
			//    else if(theta >= 135)
			//    {
			//        BackHoldAttack(transform, playerFrontpos, target);
			//        Debug.Log(transform.localRotation);
			//        isAct = true;
			//    }
			//    else
			//    {
			//        print("각도 :" + theta + "잡기실패");
			//        isAct = false;
			//    }
			//}
			#endregion
		}
		else
        {
            isAct = false;
        }

        return isAct;
    }

    public void FrontHoldAttack(Transform dir,Vector3 forwardVec , Enemy enemy)
    {
        Player.instance.status.curStamina += 10;
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);
        Player.instance.animator.SetTrigger("BackHoldAttack");
        DamagedStruct dmgStruct = new DamagedStruct();
        dmgStruct.isRiposte = true;
        dmgStruct.attackObj = Player.instance.gameObject;
        curActAtkValue = 6f;
        dmgStruct.dmg = Player.instance.status.mainWeapon.GetComponent<Player_Weapon>().Dmg * curActAtkValue;
        enemy.Hit(dmgStruct);
        //여기 적 앞잡함수(적이 뿅 하고 플레이어 앞으로 이동후 찔리는모션 실행)
        enemy.HoldTransPos_Enemy(dir, forwardVec);
    }

    public void BackHoldAttack(Transform dir, Vector3 forwardVec, Enemy enemy)
    {
        Player.instance.status.curStamina += 10;
        EnableWeaponMeshCol(0);
        Player.instance.SetState(Enums.ePlayerState.Atk);
        Player.instance.animator.SetTrigger("BackHoldAttack");
        DamagedStruct dmgStruct = new DamagedStruct();
        dmgStruct.isBackstab = true;
        dmgStruct.attackObj = Player.instance.gameObject;
        curActAtkValue = 6f;
        dmgStruct.dmg = Player.instance.status.mainWeapon.GetComponent<Player_Weapon>().Dmg * curActAtkValue;
        enemy.Hit(dmgStruct);
        //여기 적 뒤잡함수
        enemy.HoldTransPos_Enemy(dir, forwardVec);
    }

    public bool holdType = false;
    public void ChangeWeaponHoldType(bool holdType)
    {
        if (holdType == false)
        {
            print("falsing");
            Player.instance.status.LeftHand.gameObject.GetComponent<MeshRenderer>().enabled = true;
            Player_Weapon mainWeapon = Player.instance.status.mainWeapon.GetComponent<Player_Weapon>();
            Player_Weapon subWeapon = Player.instance.status.subWeapon.GetComponent<Player_Weapon>();
            switch (mainWeapon.type)
            {
                case eWeaponType.None:
                    print("falsing + dile");
                    Player.instance.ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[2]);
                    StartCoroutine(waitCoro(0, true));
                    break;
                case eWeaponType.Melee:
                    Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 1);
                    Player.instance.ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[2]);
                    StartCoroutine(waitCoro(1, true));
                    break;
                case eWeaponType.Sheild:
                    print("falsing + shield");
                    Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 2);
                    Player.instance.ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[2]);
                    StartCoroutine(waitCoro(2, true));
                    break;
                case eWeaponType.Arrow:
                    break;
                case eWeaponType.Range:
                    break;
            }
        }
        else
        {
            Player.instance.status.LeftHand.gameObject.GetComponent<MeshRenderer>().enabled = false;
            var mainWeapon = Player.instance.status.mainWeapon.gameObject.GetComponent<Player_Weapon>();
            var subWeapon = Player.instance.status.subWeapon.gameObject.GetComponent<Player_Weapon>();
            print("양손잡!" + mainWeapon.type);
            switch(mainWeapon.type)
            {
                case eWeaponType.None:
                    break;
                case eWeaponType.Melee:
                    print("메인");
                    Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 3);
                    Player.instance.ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[0]);
                    StartCoroutine(waitCoro(3));
                    break;
                case eWeaponType.Sheild:
                    print("실드");
                    Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 4);
                    Player.instance.ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[1]);
                    StartCoroutine(waitCoro(4));
                    //Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 4);
                    break;
                case eWeaponType.Arrow:
                    break;
                case eWeaponType.Range:
                    break;
            }

            if(mainWeapon.type == eWeaponType.None)
            {
                Player.instance.status.LeftHand.gameObject.GetComponent<MeshRenderer>().enabled = true;
                switch (subWeapon.type)
                {
                    case eWeaponType.Sheild:
                        print("실드");
                        Player.instance.animator.SetInteger("WeaponHoldTypeIndex", 4);
                        Player.instance.ChangeAnimClipInBlendTree(Player.instance.idleAnimClips[1]);
                        StartCoroutine(waitCoro(4));
                        break;
                }
            }
        }
    }

    IEnumerator waitCoro(int i, bool isInput = false)
    {
        yield return null;
        Player.instance.animator.SetInteger("WeaponHoldTypeIndex", i);
    }

    float guardParam = 0;
    float holdParam = 0;
    public void Guard()
    {
        float targetHoldParam = 1 - guardParam;
        holdParam = Mathf.Clamp(holdParam, 0f, targetHoldParam);
        if (holdType == true)
        {
            player.animator.SetLayerWeight(2, holdParam);
            holdParam += Time.deltaTime * 3f;
        }
        else
        {
            player.animator.SetLayerWeight(2, holdParam);
            holdParam -= Time.deltaTime * 3f;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            guardParam += Time.deltaTime * 10;
            Player.instance.animator.SetTrigger("Guard");
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
        holdParam = 0;
        player.animator.SetLayerWeight(1, 0);
        player.animator.SetLayerWeight(2, 0);
    }

    public Collider[] nearColliders;
    public GameObject curInteractionItem;
    bool isGet;

    public void NearObjectSearch()
    {
        //overlapSphere마스크
        int m_Mask = 0;

        m_Mask = 1 << LayerMask.NameToLayer("Item");
        //m_Mask |= 1 << LayerMask.NameToLayer("Environment");

        nearColliders = Physics.OverlapSphere(transform.position, Player.instance.status.interactionRange, m_Mask);

        if (nearColliders != null)
        {
            float shortDis = 99999999;
            Collider Object = null;
            foreach (Collider found in nearColliders)
            {
                float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);
                if (Distance < shortDis)
                {
                    Object = found;
                    shortDis = Distance;
                }
            }
            if (Object != null) curInteractionItem = Object.gameObject;
            else curInteractionItem = null;
        }
    }

    public void Interaction()
    {
            if (curInteractionItem != null)
            {
                Item obj = curInteractionItem.GetComponent<Item>();
                if (obj.ObjectType == Enums.ObjectType.Item)
                {
                    //if (!isGet)
                    //{
                        Item curItem = obj.GetComponent<Item>();
                        
                        if (Inventory.Instance.ItemIn(curItem)) curItem.gameObject.SetActive(false);
                      //  isGet = true;
                    //}
                }
                else if (obj.ObjectType == Enums.ObjectType.Environment)
                {
                    //anim.SetTrigger("");
                }
            }
    }

    public void UseFood()
    {
        Player.instance.SetState(ePlayerState.Interacting);
        Player.instance.animator.SetTrigger("UseDrink");
        player.animator.SetLayerWeight(1, 1f);
    }

    public void PlayCurItemFuncs()
    {
        GameObject healingEffect = ObjectPoolingCenter.Instance.LentalObj("Healing",1);
        healingEffect.transform.SetParent(Player.instance.transform);
        healingEffect.transform.position = Player.instance.spine3Tr.position;
        healingEffect.GetComponent<ParticleSystem>().Play();
        Player.instance.status.curHp += 10;
        if(Player.instance.status.curHp > Player.instance.status.maxHp)
        {
            Player.instance.status.curHp = Player.instance.status.maxHp;
        }
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
        if (CurCoroCounter1 != CurCoroCounter2)
        {
            isComboCheck = false;
            SetPlayerStatus(0);
        }
    }
    #endregion
    public void EnableWeaponMeshCol(int i)
    {
        //player.status.mainWeapon.GetComponent<Player_Weapon>().EnableWeaponMeshCollider(i);
        Player.instance.status.mainWeapon.GetComponent<Player_Weapon>().hittedEnemyList.Clear();
        player.status.mainWeapon.GetComponent<Player_Weapon>().WeaponColliderOnOff(i);
        if(i == 0)
        {
            Player.instance.status.mainWeapon.GetComponent<Player_Weapon>().trailRenderer.enabled = false;
        }
        else
        {
            Player.instance.status.mainWeapon.GetComponent<Player_Weapon>().trailRenderer.enabled = true;
        }
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
