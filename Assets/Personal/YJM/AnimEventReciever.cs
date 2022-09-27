using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventReciever : MonoBehaviour
{
    #region singletone
    /// <singletone>    
    static public AnimEventReciever instance = null;
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

    public int i = 0;

    public void SetPlayerStatusIdle()
    {
        Player.instance.SetState(Enums.ePlayerState.Idle);
    }
}
