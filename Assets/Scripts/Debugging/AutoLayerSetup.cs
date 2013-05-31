using UnityEngine;
using System.Collections;

public class AutoLayerSetup : MonoBehaviour
{
	public LayerMask startWith;
	public LayerMask thenRemove;
	
	void Start()
	{
		Camera c;
		Projector p;
		
		int mask = startWith.value;
		mask &= ~(thenRemove.value);
		
		if(c = GetComponent<Camera>())
		{
			c.cullingMask = mask;
		}
		if(p = GetComponent<Projector>())
		{
			p.ignoreLayers = mask;
		}
	}
}
