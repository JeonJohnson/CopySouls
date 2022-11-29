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
        //생성은 각자 에너미 Start에서 해줌.
    }

    private void Start()
    {

        ResetHpBar();
        //print(target);
        //curHp = target.status.maxHp;
        //canvasGroup.alpha = 0f;
        //damageText.color = new Color(1f, 1f, 1f, 0f);
        //InitName();
    }

    void Update()
    {
		if (curHp <= 0f)
		{
            if (!isDestroyed)
            {
                isDestroyed = true;
                StartCoroutine(DisappearEffect());
            }
		}

		if (target != null)
        {
            AutoUpdateHpBar();
            
            if (isDamaged == true)
            {
                //print("위치변경중");
                canvasGroup.alpha = 1f;
                if (target.headTr)
                {
                    if (target.status.name_e == Enums.eEnemyName.Spirit)
                    {
                        transform.position = target.headTr.position + new Vector3(0, 0.75f, 0f);
                    }
                    else 
                    {
                        transform.position = target.headTr.position + new Vector3(0, 0.35f, 0f);
                    }
                }
                else
                {
                    transform.position = target.transform.position + new Vector3(0, 2f, 0f);
                }
                transform.forward = -(Camera.main.transform.position - transform.position);
            }
        }
        //else
        //{
        //    ResetHpBar();
        //    Destroy(this.gameObject);
        //}
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

    IEnumerator DisappearEffect()
    {
        yield return new WaitForSeconds(2f);
        //ResetHpBar();
        gameObject.SetActive(false);
    }

    public void DestorySceneReset()
    {//어차피 씬 초기화 될때 잠시 로딩 시간 있으니 Destroy해도 될듯
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void ResetHpBar()
    {
        StopAllCoroutines();

        isDestroyed = false;
        curHp = target.status.maxHp;
        canvasGroup.alpha = 0f;
        damageText.color = new Color(1f, 1f, 1f, 0f);
        hpSlider.value = 1f;
        hpEffectImage.fillAmount = 1f;

        InitName();
    }



    public void InitName()
    {
        if (nameText != null && target != null)
        {
            nameText.text = target.status.name;
        }

    }
}
