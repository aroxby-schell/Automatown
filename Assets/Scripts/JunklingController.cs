using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JunklingController : MonoBehaviour
{
	private const float MOVE_DELAY = 1f;
	private const float SPEED = 2f;
	private const float HARVEST_TIME = 1f;
	
	private enum States
	{
		idle,
		moving,
		harvesting,
		depositing
	}
	
	public bool canHarvestRock = false;
	
	public JunklingSensor left, right, forward;
	public GameObject scrapMarker, grassMarker, waterMarker, rockMarker, gearMarker, steamMarker;
	
	private States state = States.idle;
	private float nextMoveTime;
	private Vector3 destination;
	
	private string resourceCarried = TileTypes.NONE;
	private Vector3 harvestStart, harvestFinish;
	private float harvestStartTime;
	
	private ResourceController targetResource = null;
	private DepotController targetDepot = null;
	
	//HACK: SendMessage whines when you say 'ref', so we use this to create a referene to reference
	public class DepotControllerContainer
	{
		public DepotController controller;
	}
	
	private Dictionary<string, GameObject> resourceMarkers = new Dictionary<string, GameObject>();
	
	void Start()
	{		
		destination = transform.position;
		state = States.idle;		
		
		resourceMarkers.Add(TileTypes.SCRAP, scrapMarker);
		resourceMarkers.Add(TileTypes.GRASS, grassMarker);
		resourceMarkers.Add(TileTypes.WATER, waterMarker);
		resourceMarkers.Add(TileTypes.ROCK, rockMarker);
		resourceMarkers.Add(TileTypes.GEAR, gearMarker);
		resourceMarkers.Add(TileTypes.STEAM, steamMarker);
	}
	
	private bool CheckMovement()
	{
		if(forward.GetTileType()==TileTypes.PATH)
		{
			StartMovement(0f);
			return true;
		}
		if(right.GetTileType()==TileTypes.PATH)
		{
			StartMovement(90f);
			return true;
		}
		if(left.GetTileType()==TileTypes.PATH)
		{
			StartMovement(-90f);
			return true;
		}
		
		return false;
	}
	
	private void StartMovement(float angle)
	{
		transform.RotateAround(transform.up, angle * Mathf.Deg2Rad);
		destination = transform.position + transform.forward * BuildGrid.getGridSize();
		state = States.moving;
		nextMoveTime = Time.timeSinceLevelLoad + MOVE_DELAY + BuildGrid.getGridSize()/SPEED;
	}
	
	private void UpdateMovement()
	{
		if(state!=States.moving) return;
		transform.position = Vector3.MoveTowards(transform.position, destination, SPEED * Time.deltaTime);
		if(Vector3.Distance(transform.position, destination)<=0.001f) state = States.idle;
	}
	
	private bool CheckHarvest()
	{
		if(resourceCarried != TileTypes.NONE) return false;
		
		if(TileTypes.IsResource( right.GetTileType() ))
		{
			if(CanHarvest(right)) return true;
		}
		if(TileTypes.IsResource( left.GetTileType() ))
		{
			if(CanHarvest(left)) return true;
		}
		if(TileTypes.IsResource( forward.GetTileType() ))
		{
			if(CanHarvest(forward)) return true;
		}
		
		return false;
	}
	
	private bool CanHarvest(JunklingSensor resource)
	{
		targetResource = resource.GetTileCollider().GetComponent<ResourceController>();
		string type = targetResource.GetTileType();
		if(type==TileTypes.ROCK && !canHarvestRock) return false;
		if(targetResource.IsSpawned())
		{
			StartHarvest();
			return true;
		}
		return false;
	}
	
	private void StartHarvest()
	{		
		state = States.harvesting;
		resourceCarried = targetResource.GetTileType();
		
		harvestFinish = resourceMarkers[resourceCarried].transform.position;
		harvestStart = targetResource.GetHarvestPoint();
		targetResource.HarvestStart();
		
		resourceMarkers[resourceCarried].transform.position = harvestStart;
		resourceMarkers[resourceCarried].SetActive(true);
		
		harvestStartTime = Time.timeSinceLevelLoad;
	}
	
	private void UpdateHarvest()
	{
		if(state!=States.harvesting) return;
		
		float harvestDistance = Vector3.Distance(resourceMarkers[resourceCarried].transform.position, harvestFinish);
		
		if(harvestDistance<=0.001f)
		{
			state = States.idle;
			return;
		}
		
		resourceMarkers[resourceCarried].transform.position =
			Vector3.Lerp(harvestStart, harvestFinish, Time.timeSinceLevelLoad-harvestStartTime * HARVEST_TIME);
	}
	
	private bool CheckDeposit()
	{
		if(resourceCarried == TileTypes.NONE) return false;
		DepotControllerContainer con = new DepotControllerContainer();
		
		if(TileTypes.IsDepot( right.GetTileType() ))
		{
			right.GetTileCollider().gameObject.SendMessage("GetDepotController", con);
			targetDepot = con.controller;
			if(targetDepot.AcceptsResource(resourceCarried))
			{
				StartDeposit();
				return true;
			}
		}
		if(TileTypes.IsDepot( left.GetTileType() ))
		{
			left.GetTileCollider().gameObject.SendMessage("GetDepotController", con);
			targetDepot = con.controller;
			if(targetDepot.AcceptsResource(resourceCarried))
			{
				StartDeposit();
				return true;
			}
		}
		if(TileTypes.IsDepot( forward.GetTileType() ))
		{
			forward.GetTileCollider().gameObject.SendMessage("GetDepotController", con);
			targetDepot = con.controller;
			if(targetDepot.AcceptsResource(resourceCarried))
			{
				StartDeposit();
				return true;
			}
		}
		
		return false;
	}
	
	private void StartDeposit()
	{
		state = States.depositing;
		
		harvestStart = resourceMarkers[resourceCarried].transform.position;
		harvestFinish = targetDepot.GetInputPosition();
		harvestStart.y = harvestFinish.y;
		
		harvestStartTime = Time.timeSinceLevelLoad;
	}
	
	private void UpdateDeposit()
	{
		if(state!=States.depositing) return;
		
		float harvestDistance = Vector3.Distance(resourceMarkers[resourceCarried].transform.position, harvestFinish);
		
		if(harvestDistance<=0.001f)
		{
			state = States.idle;
			resourceMarkers[resourceCarried].transform.position = harvestStart;
			resourceMarkers[resourceCarried].SetActive(false);
			targetDepot.SetInputResource(resourceCarried);
			resourceCarried = TileTypes.NONE;			
			return;
		}
		
		resourceMarkers[resourceCarried].transform.position =
			Vector3.Lerp(harvestStart, harvestFinish, Time.timeSinceLevelLoad-harvestStartTime * HARVEST_TIME);
	}
	
	void Update()
	{
		if(!PowerField.IsWorldPosPowered(transform.position))
		{
			//Try to preserve the move delay when power is lost, rather keep another varible, this is mostly correct
			nextMoveTime += Time.deltaTime;
			return;
		}
		
		UpdateHarvest();
		UpdateDeposit();
		UpdateMovement();
		
		if(state==States.idle && nextMoveTime<=Time.timeSinceLevelLoad)
		{
			//Check resource before we rotate
			if(!CheckHarvest() && !CheckDeposit())
			{			
				CheckMovement();
			}
		}
	}
}
