using UnityEngine;
using System.Collections;

public class ResourceController : MonoBehaviour
{
	public bool manualSpawnOnly = false;
	
	private const float RESPWAN_TIME = 5f;
	private float nextRespawn = 0f;
	private bool needsRespawn = true;
	private GameObject harvestItem = null;
	
	void Start()
	{
		needsRespawn = manualSpawnOnly;
		if(!harvestItem) harvestItem = gameObject;
	}
	
	public void SetHarvestItem(GameObject marker)
	{
		harvestItem = marker;
	}
	
	public Vector3 GetHarvestPoint()
	{
		return harvestItem.transform.position;
	}
	
	public string GetTileType()
	{
		return harvestItem.tag;
	}
	
	public void Respawn()
	{
		//HACK
		if(harvestItem.tag==TileTypes.ROCK) return;
		harvestItem.renderer.enabled = true;
		needsRespawn = false;
	}
	
	public bool IsSpawned()
	{
		return !needsRespawn;
	}
	
	public void HarvestStart()
	{
		harvestItem.renderer.enabled = false;
		needsRespawn = true;
		nextRespawn = Time.timeSinceLevelLoad + RESPWAN_TIME;
	}
	
	void Update()
	{
		if(!needsRespawn || manualSpawnOnly) return;
		
		if(nextRespawn <= Time.timeSinceLevelLoad)
		{
			Respawn();
		}
	}
}
