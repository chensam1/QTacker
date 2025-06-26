using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
	private static float baseScale = 0.4f;
	private bool isAnimating;
	private bool isScaleUp;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (isAnimating)
		{
			if (transform.localScale.x > 1.5)
			{
				isScaleUp = false;
			}
			else if (transform.localScale.x < baseScale)
			{
				isScaleUp = true;
			}
			Vector3 rotageValue = Vector3.one * 1.5f * Time.deltaTime;
			if (isScaleUp == false)
			{
				rotageValue = -rotageValue;
			}
			transform.localScale = transform.localScale + rotageValue;
		}
	}

	private void OnEnable()
	{
		isAnimating = true;
		isScaleUp = true;
		transform.localScale = new Vector3(baseScale, baseScale, baseScale);
	}

}
