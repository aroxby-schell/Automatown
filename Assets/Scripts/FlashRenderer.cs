using UnityEngine;
using System.Collections;

public class FlashRenderer : MonoBehaviour
{
	private const float flashTime = 2f;
	private float offTime = 0f;
	
	public void Flash()
	{
		gameObject.SetActive(true);
		offTime = Time.timeSinceLevelLoad + flashTime;
	}
	
	void Update()
	{
		if(offTime<=Time.timeSinceLevelLoad)
		{
			gameObject.SetActive(false);
		}
	}
}
