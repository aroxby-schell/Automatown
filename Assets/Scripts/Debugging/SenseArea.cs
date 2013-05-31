using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SenseArea : MonoBehaviour
{
	private HashSet<GameObject> types = new HashSet<GameObject>();
	private int tileLayer;
	
	void Start()
	{
		tileLayer = LayerMask.NameToLayer("Tile Marker");
	}
	
	void OnTriggerEnter(Collider trigger)
	{
		if(trigger.gameObject.layer==tileLayer) types.Add(trigger.gameObject);
	}
	
	void OnTriggerExit(Collider trigger)
	{
		if(trigger.gameObject.layer==tileLayer) types.Remove(trigger.gameObject);
	}
}
