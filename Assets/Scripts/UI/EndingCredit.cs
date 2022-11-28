using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EndingCredit : MonoBehaviour
{
    public RectTransform contentRectTr;

    //public float scrollVal;
    //public float spd;

    public Text pressAnyKeyTxt;

    public void SetScrollVal(float val)
    { //normalize 된 값이라고 생각하삼
        float scrollVal = val * contentRectTr.rect.height;
        Vector2 tempPos =   contentRectTr.rect.position;
        tempPos.y = scrollVal;
        contentRectTr.anchoredPosition = tempPos;
    }

	private void Awake()
	{
        pressAnyKeyTxt.gameObject.SetActive(false);
    }
	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
