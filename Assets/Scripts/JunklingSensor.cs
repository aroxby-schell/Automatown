using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JunklingSensor : MonoBehaviour
{
	private int tileLayer;
	private Collider currentTrigger = null;
	
	void Start()
	{
		tileLayer = LayerMask.NameToLayer("Tile Marker");
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.S))
		{
			print(name + ": " + GetTileType());
		}
	}
	
	public Collider GetTileCollider()
	{
		return currentTrigger;
	}
	
	public string GetTileType()
	{
		if(!currentTrigger) return TileTypes.NONE;
		return currentTrigger.tag;
	}
	
	void OnTriggerEnter(Collider trigger)
	{
		if(trigger.gameObject.layer==tileLayer)
		{
			currentTrigger = trigger;
		}
	}
	
	void OnTriggerExit(Collider trigger)
	{
		if(trigger==currentTrigger)
		{
			currentTrigger = null;
		}
	}
}
