using UnityEngine;
using System.Collections;

public class FactoryTrigger : MonoBehaviour
{
	public GameObject prefab;
	public Vector3 size;
	
	private int tileLayer;
	
	void Start()
	{
		tileLayer = 1<<LayerMask.NameToLayer("Tile Marker");

		size *= BuildGrid.getGridSize();

		if(AreaClear(size))
		{
			float halfWidth = 0.5f;
			//HACK: The +/- 2 here comes from the factory bounds
			CreatePrefab(prefab, transform.position, new Vector3(size.x-2f, 0f, -size.z+2f)*halfWidth );
		}
		
		Destroy(gameObject);
	}
	
	bool AreaClear(Vector3 size)
	{
		float gridSize = BuildGrid.getGridSize();
		
		int girdCountX = (int)Mathf.Ceil( size.x/gridSize );
		int girdCountZ = (int)Mathf.Ceil( size.z/gridSize );
		
		Vector3 center = transform.position;
		
		for(int x = 0; x<girdCountX; x++)
		{
			for(int z = 0; z<girdCountZ; z++)
			{
				Vector3 spherePos = center + x*gridSize*Vector3.right - z*gridSize*Vector3.forward;
				
				Collider[] overlap = Physics.OverlapSphere(spherePos, 0.01f, tileLayer);
				
				//HACK
				if( overlap.Length==1 && overlap[0].tag=="Rock")
				{
					ResourceController rc = overlap[0].GetComponent<ResourceController>();
					if(rc && !rc.IsSpawned())
					{
						Destroy(overlap[0].gameObject);
						return true;
					}
					return false;
					
				}
				else if( overlap.Length > 0 )
				{
					return false;
				}
			}
		}
		
		return true;
	}
	
	private void CreatePrefab(GameObject prefab, Vector3 worldPos, Vector3 offset)
	{		
		worldPos = BuildGrid.WorldToGridCenter(worldPos);
		worldPos.y = prefab.transform.position.y;
		worldPos += offset;
		
		Quaternion rotation = prefab.transform.rotation;
		
		Instantiate(prefab, worldPos, rotation);
	}
}
