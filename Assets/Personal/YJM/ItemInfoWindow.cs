using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWindow : MonoBehaviour
{
    public static ItemInfoWindow Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] Image itemImage;
    [SerializeField] Text nameText;
    [SerializeField] Text countText;

    [SerializeField] CanvasGroup CanvasGroup;

    float canvasAlpha = 0f;
    public bool isEnabled = false;

    public void InitContents(Sprite image, string name, int count)
    {
        itemImage.sprite = image;
        nameText.text = name;
        countText.text = count.ToString();
    }

    public void ShowItemInfo()
    {
        isEnabled = true;
        this.gameObject.SetActive(true);
    }

    public void HideItemInfo()
    {
        isEnabled = false;
    }

    private void Update()
    {
        CanvasGroup.alpha = canvasAlpha;
        if (isEnabled)
        {
            canvasAlpha += Time.deltaTime * 5f;
            canvasAlpha = Mathf.Clamp(canvasAlpha, 0f, 1f);
        }
        else
        {
            canvasAlpha -= Time.deltaTime * 5f;
            if (canvasAlpha <= 0f) gameObject.SetActive(false);
            canvasAlpha = Mathf.Clamp(canvasAlpha, 0f, 1f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isEnabled = false;
        }
    }
}
