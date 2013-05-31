using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DepotController : MonoBehaviour
{
	public bool canFill = false;

	public Transform inputTransform, outputTransform;
	public GameObject scrapInputMarker, grassInputMarker, waterInputMarker, rockInputMarker, gearInputMarker, steamInputMarker;
	public GameObject scrapOutputMarker, grassOutputMarker, waterOutputMarker, rockOutputMarker, gearOutputMarker, steamOutputMarker;
	private string inputResource = TileTypes.NONE, outputResource = TileTypes.NONE;
	
	private Dictionary<string, GameObject> inputResourceMarkers = new Dictionary<string, GameObject>();
	private Dictionary<string, GameObject> outputResourceMarkers = new Dictionary<string, GameObject>();
	
	private bool isLocked;
	
	void Start()
	{
		inputResourceMarkers.Add(TileTypes.SCRAP, scrapInputMarker);
		inputResourceMarkers.Add(TileTypes.GRASS, grassInputMarker);
		inputResourceMarkers.Add(TileTypes.WATER, waterInputMarker);
		inputResourceMarkers.Add(TileTypes.ROCK, rockInputMarker);
		inputResourceMarkers.Add(TileTypes.GEAR, gearInputMarker);
		inputResourceMarkers.Add(TileTypes.STEAM, steamInputMarker);
		
		outputResourceMarkers.Add(TileTypes.SCRAP, scrapOutputMarker);
		outputResourceMarkers.Add(TileTypes.GRASS, grassOutputMarker);
		outputResourceMarkers.Add(TileTypes.WATER, waterOutputMarker);
		outputResourceMarkers.Add(TileTypes.ROCK, rockOutputMarker);
		outputResourceMarkers.Add(TileTypes.GEAR, gearOutputMarker);
		outputResourceMarkers.Add(TileTypes.STEAM, steamOutputMarker);
	}
	
	public bool HasReource(string type)
	{
		return inputResource==type;
	}

	public bool AcceptsResource(string type, bool lockDepot = true)
	{
		if(isLocked) return false;
		if(canFill && inputResource!=TileTypes.NONE) return false;
		if(canFill) isLocked = lockDepot;
		return inputResourceMarkers[type]!=null;
	}
	
	public Vector3 GetInputPosition()
	{
		return inputTransform.position;
	}
	
	public Vector3 GetOutputPosition()
	{
		return outputTransform.position;
	}
	
	public void SetInputResource(string type)
	{
		if(inputResource!=TileTypes.NONE) inputResourceMarkers[inputResource].SetActive(false);
		inputResource = type;
		if(inputResource!=TileTypes.NONE) inputResourceMarkers[inputResource].SetActive(true);
		isLocked = false;
		
		if(!canFill && inputResource==TileTypes.GEAR) StaticData.gears++;
	}
	
	public void SetOutputResource(string type)
	{
		if(outputResource!=TileTypes.NONE) outputResourceMarkers[outputResource].SetActive(false);
		outputResource = type;
		outputResourceMarkers[outputResource].SetActive(true);
	}
	
	//Also used in classes that extend DepotController, since GetComponent is not aware of that
	public void GetDepotController(JunklingController.DepotControllerContainer output)
	{
		output.controller = this;
	}
}
