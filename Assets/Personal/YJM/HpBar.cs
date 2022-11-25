using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Enemy target;
    bool isDamaged = false;
    bool isDestroyed = false;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Slider hpSlider;
    [SerializeField] Image hpEffectImage;
    [SerializeField] Text damageText;
    [SerializeField] Text nameText;


    float curHp = 0f;

    private void Awake()
    {

    }

    private void Start()
    {
        print(target);
        curHp = target.status.maxHp;
        canvasGroup.alpha = 0f;
        damageText.color = new Color(1f, 1f, 1f, 0f);
        InitName();
    }

    void Update()
    {
        if(curHp <= 0f)
        {
            isDestroyed = true;
            if(isDestroyed == true)
            {
                StartCoroutine(DestroyEffect());
            }
        }

        if (target != null)
        {
            AutoUpdateHpBar();

            if (isDamaged == true)
            {
                print("위치변경중");
                canvasGroup.alpha = 1f;
                transform.position = target.transform.position + new Vector3(0, 1.9f, 0f);
                transform.forward = -(Camera.main.transform.position - transform.position);
            }
        }
        else
        {
            ResetHpBar();
            Destroy(this.gameObject);
        }
    }

    public void AutoUpdateHpBar()
    {
        if (curHp != target.status.curHp)
        {
            UpdateHpBar(curHp - target.status.curHp, target.status.maxHp);
            curHp = target.status.curHp;
        }
    }

    public void UpdateHpBar(float damage, float maxHp)
    {
        isDamaged = true;
        float damageValue = target.status.curHp / maxHp;

        hpSlider.value = damageValue;
        StopAllCoroutines();
        StartCoroutine(HpBarEffect(damage, damageValue));
    }

    IEnumerator HpBarEffect(float damage, float _value)
    {
        hpSlider.value = _value;
        damageText.color = new Color(1f, 1f, 1f, 1f);
        damageText.text = damage.ToString();

        float originEffectValue = hpEffectImage.fillAmount;

        float lerpTime = 2f;
        float timer = 0;

        yield return new WaitForSeconds(2f);

        while(timer < 2f)
        {
            timer += Time.unscaledDeltaTime;
            damageText.color = new Color(1f, 1f, 1f, 1 - timer / lerpTime);
            hpEffectImage.fillAmount = Mathf.Lerp(originEffectValue, _value, timer / lerpTime);
            yield return null;
        }
        yield break;
    }

    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(2f);
        ResetHpBar();
        Destroy(this.gameObject);
    }

    public void ResetHpBar()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0f;
        damageText.color = new Color(1f, 1f, 1f, 1f);
        hpSlider.value = 1f;
        hpEffectImage.fillAmount = 1f;
    }

    public void InitName()
    {
        if(nameText != null) nameText.text = target.status.name;
    }
}
