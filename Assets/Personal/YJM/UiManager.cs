using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UiManager : Manager<UiManager>
{
    public static bool UIActivated = false;

    public static int WindowProcedureIndex;

    [SerializeField] GameObject playerStatusUi;
    [SerializeField] GameObject hpBarUi;

    [SerializeField] GameObject hpBarPrefab;
    [SerializeField] GameObject bossHpBarPrefab;


    [Header("QuickSlot")]
    [SerializeField] public QuickSlot quickSlot1;
    [SerializeField] public QuickSlot quickSlot2;
    [SerializeField] public QuickSlot quickSlot3;
    [SerializeField] public QuickSlot quickSlot4;

    [Header("EquiptSlot")]
    public EquiptSlot EquiptSlot_Defence;
    public EquiptSlot EquiptSlot_Weapon;
    public EquiptSlot EquiptSlot_Helmet;
    public EquiptSlot EquiptSlot_Armor;

    [Header("EquiptSlot_Q")]
    public EquiptSlot_Q EquiptSlotQ_Defence;
    public EquiptSlot_Q EquiptSlotQ_Weapon;


    [Header("BlurEffect")]
    public Canvas screenEffectCanvas;
    //public Image screenBlurImg;
    public Material screenBlurMat;

    public void SetBlurAmount(float amount)
    {
        screenEffectCanvas.gameObject.SetActive(true);
        screenBlurMat.SetFloat("_BlurAmount", amount * 0.0025f);
    }

    public HpBar InstantiateHpBar(Enemy target)
    {
        GameObject hpBarGo = Instantiate(hpBarPrefab, hpBarUi.transform);
        HpBar hpBar = hpBarGo.GetComponent<HpBar>();
        hpBar.target = target;
        return hpBar;
    }

    public HpBar_Boss InstantiateBossHpBar(Golem target)
    {
        GameObject hpBarObj = Instantiate(bossHpBarPrefab, playerStatusUi.transform);
        HpBar_Boss hpBar = hpBarObj.GetComponent<HpBar_Boss>();
        hpBar.target = target;
        return hpBar;
    }

    private void Awake()
    {
        //TestMakeHpBar();
        screenBlurMat.SetFloat("_BlurAmount",0f);
        if(!EquipmentWindow.EquipmentActivated) EquipmentWindow.Instance.TryOpenEquiptment();
    }

    private void Start()
    {
        if (EquipmentWindow.EquipmentActivated) EquipmentWindow.Instance.TryOpenEquiptment();
    }

    private void Update()
    {
        UI_KeyboardShortcut();
        if (UIActivated) Player.instance.ActivatePlayerInput(false);
        else Player.instance.ActivatePlayerInput(true);
    }
    //void TestMakeHpBar()
    //{
    //    for (int i = 0; i < UnitManager.Instance.aliveEnemyList.Count; i++)
    //    {
    //        InstantiateHpBar(UnitManager.Instance.aliveEnemyList[i]);
    //    }
    //}

    //UI단축키
    //인벤 장비창 켜지거나 alt눌르면 마우스 활성화
    //인벤이나 장비창이 꺼지거나 alt한번 더 누르면 마우스 비활성화

    public void UI_KeyboardShortcut()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (!UIActivated) UIActivated = true;
            else UIActivated = false;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Inventory.Instance.TryOpenInventory();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            EquipmentWindow.Instance.TryOpenEquiptment();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //제일 위에 있는 거 꺼야함
            //인벤
            if (Inventory.inventoryActivated)
            {
                WindowProcedureIndex = 0;
                Inventory.Instance.TryOpenInventory();
                if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            }
            if (EquipmentWindow.EquipmentActivated)
            {
                WindowProcedureIndex = 0;
                EquipmentWindow.Instance.TryOpenEquiptment();
            }
            if (SettingWindow.SettingActivated)
            {
                WindowProcedureIndex = 0;
                SettingWindow.Instance.TryOpenSetting();
            }

            if (Inventory.inventoryActivated && DivisionProcess.DivisionActivated)
            {
                //분할창 끄기
                Inventory.Instance.DivisionParent.Button_DivisionCancel();
            }
            else if (Inventory.inventoryActivated && ThrowingProcess.ThrowingActivated)
            {
                //버리기 창 끄기
                Inventory.Instance.ThrowingParent.Button_ThrowCancel();
            }
            //else if (Inventory.inventoryActivated && !DivisionProcess.DivisionActivated && !ThrowingProcess.ThrowingActivated)
            //{
            //    //인벤토리 창 끄기
            //    Inventory.Instance.Button_InventoryExit();
            //    if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            //}
            //else if (EquipmentWindow.EquipmentActivated)
            //{
            //    EquipmentWindow.Instance.TryOpenEquiptment();
            //}
            //else if(SettingWindow.SettingActivated)
            //{
            //    SettingWindow.Instance.TryOpenSetting();
            //}
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (DivisionProcess.DivisionActivated)
            {
                //분할 ENTER적용
                Inventory.Instance.DivisionParent.Button_Division();
            }
            else if (ThrowingProcess.ThrowingActivated)
            {
                //버리기 ENTER적용
                Inventory.Instance.ThrowingParent.Button_Throw();
            }
        }

        //퀵슬롯단축키
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            quickSlot1.QuickSlotUse();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            quickSlot2.QuickSlotEquipt(quickSlot2, EquiptSlot_Weapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            quickSlot3.QuickSlotUse();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (SelectionProcess.SelectionActivated) Inventory.Instance.SelectionParent.Selection_AllOff();
            quickSlot4.QuickSlotEquipt(quickSlot4, EquiptSlotQ_Defence);
        }
    }

    private void EquiptmentInitialize()
    {
        Debug.Log("야메 발동!");
    }

    public void WindowProcedure()
    {
        WindowProcedureIndex++;
    }

    const float MIN_FOG_DENSITY = 0.002f;
    const float MAX_FOG_DENSITY = 0.2f;
    public void fogChanged(float inten)
    {
        float diff = MAX_FOG_DENSITY - MIN_FOG_DENSITY;
        float value = MIN_FOG_DENSITY + diff * inten; // 0 ~ 1의 값
        RenderSettings.fogDensity = value;
    }

    public void ShowFog()
    {
        StartCoroutine(ShowFogCoro(true));
    }

    public void HideFog()
    {
        StartCoroutine(ShowFogCoro(false));
    }

    IEnumerator ShowFogCoro(bool i, float value = 1f)
    {
        float timer = 1f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime * 0.7f;
            if (i == true)
            {
                fogChanged(1-timer);
            }
            else
            {
                fogChanged(timer);
            }
            yield return null;
        }
        yield return null;
    }
    public void PlayFogEffect()
    {
        StartCoroutine(PlayerFogEffectCoro());
    }
    IEnumerator PlayerFogEffectCoro()
    {
        StartCoroutine(ShowFogCoro(true));
        yield return new WaitForSeconds(3f);
        StartCoroutine(ShowFogCoro(false));
    }
}
