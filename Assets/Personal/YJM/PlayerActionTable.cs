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


    public void Hit(int damage)
    {
        Player.instance.SetState(Enums.ePlayerState.Hit);
        StartCoroutine(TakeDamage(damage));
    }

    IEnumerator TakeDamage(int damage, float stunTime = 0.767f)
    {
        Player.instance.hp -= damage;
        Player.instance.animator.SetTrigger("Hit");
        yield return new WaitForSeconds(stunTime);
        Player.instance.SetState(Enums.ePlayerState.Idle);
    }
}
