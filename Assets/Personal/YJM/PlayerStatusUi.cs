using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using Structs;

public class PlayerStatusUi : MonoBehaviour
{

    [SerializeField] Slider hpBarSlider;
    [SerializeField] Slider mpBarSlider;
    [SerializeField] Slider staminaBarSlider;

    RectTransform hpBarRt;
    RectTransform mpBarRt;
    RectTransform staminaBarRt;

    // Start is called before the first frame update
    void Start()
    {
        hpBarRt = hpBarSlider.gameObject.GetComponent<RectTransform>();
        mpBarRt = mpBarSlider.gameObject.GetComponent<RectTransform>();
        staminaBarRt = staminaBarSlider.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStatus status = Player.instance.status;
        UpdateUI(status.curHp, status.curMp, status.curStamina);
    }

    public void UpdateUI(float hpValue, float mpValue, float staminaValue)
    {
        hpBarRt.sizeDelta = new Vector2(Player.instance.status.maxHp * 3, hpBarRt.sizeDelta.y);
        staminaBarRt.sizeDelta = new Vector2(Player.instance.status.maxStamina * 3, staminaBarRt.sizeDelta.y);
        hpBarSlider.value = hpValue / Player.instance.status.maxHp;
        mpBarSlider.value = mpValue / Player.instance.status.maxMp;
        staminaBarSlider.value = staminaValue / Player.instance.status.maxMp;
        //hpBar.fillAmount = hpValue / Player.instance.status.maxHp;
        //mpBar.fillAmount = mpValue / Player.instance.status.maxMp;
        //staminaBar.fillAmount = staminaValue / Player.instance.status.maxStamina;
    }
}
