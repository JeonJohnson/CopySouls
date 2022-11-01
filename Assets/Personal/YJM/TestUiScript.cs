using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUiScript : MonoBehaviour
{
    static public TestUiScript instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] Image hpBar;
    [SerializeField] Image mpBar;
    [SerializeField] Image staminaBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI(float hpValue, float mpValue, float staminaValue)
    {
        hpBar.fillAmount = hpValue / Player.instance.status.maxHp;
        mpBar.fillAmount = mpValue / Player.instance.status.maxMp;
        staminaBar.fillAmount = staminaValue / Player.instance.status.maxStamina;
    }
}
