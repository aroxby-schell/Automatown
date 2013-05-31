using UnityEngine;
using System.Collections;

public class ForgeController : MonoBehaviour
{
	public DepotController depot;
	public ResourceController resource;
	
	private const float FORGE_TIME = 4f;
	private float nextGenerationTime = 0f;
	
	void Update()
	{
		if(!PowerField.IsWorldPosPowered(transform.position))
		{
			//Try to preserve the move delay when power is lost, rather keep another varible, this is mostly correct
			nextGenerationTime += Time.deltaTime;
			return;
		}
		
		if(depot.HasReource(TileTypes.SCRAP) && !resource.IsSpawned())
		{
			float time = Time.timeSinceLevelLoad;
			if(nextGenerationTime<=time)
			{
				resource.SetHarvestItem(depot.gearOutputMarker);
				resource.Respawn();
				depot.SetOutputResource(TileTypes.GEAR);
				depot.SetInputResource(TileTypes.NONE);
				nextGenerationTime = Time.timeSinceLevelLoad + FORGE_TIME;
			}
		}
		else if(depot.HasReource(TileTypes.WATER)&& !resource.IsSpawned())
		{
			float time = Time.timeSinceLevelLoad;
			if(nextGenerationTime<=time)
			{
				resource.SetHarvestItem(depot.steamOutputMarker);
				resource.Respawn();
				depot.SetOutputResource(TileTypes.STEAM);
				depot.SetInputResource(TileTypes.NONE);
				nextGenerationTime = Time.timeSinceLevelLoad + FORGE_TIME;
			}
		}
		else
		{
			//Grrr...
			nextGenerationTime = Time.timeSinceLevelLoad + FORGE_TIME;
		}
	}
}
