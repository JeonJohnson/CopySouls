using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffect : MonoBehaviour
{
    [SerializeField] Material grayScaleMat;
    public float grayScaleAmount;

	private void Awake()
	{
		grayScaleMat.SetFloat("_GrayScaleAmount", 0f);
	}

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.J))
		//{
		//	grayScaleAmount += Time.unscaledTime * 0.01f;
		//	grayScaleMat.SetFloat("_GrayScaleAmount", grayScaleAmount);
		//}
	}

	public void SetGrayScaleAmount(float amount)
	{
		grayScaleAmount = Mathf.Clamp(amount, 0f, 1f);
		grayScaleMat.SetFloat("_GrayScaleAmount", grayScaleAmount);
	}
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, grayScaleMat);
    }

}
