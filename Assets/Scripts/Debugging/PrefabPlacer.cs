using UnityEngine;
using System.Collections;

public class PrefabPlacer : MonoBehaviour
{
	public GameObject prefab;
	private LayerMask gridMask;
	
	void Start()
	{
		//I don't know why this works, it seems I still don't understand Raycast
		gridMask = ~(1 << LayerMask.NameToLayer("Grid Projection"));
		gridMask &= ~(1 << LayerMask.NameToLayer("PowerField"));
	}
	
	void Update()
	{
		if(Input.GetMouseButtonDown(2))
		{
			RaycastHit hit;			
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, gridMask);			
			CreatePrefab(prefab, hit.point);
		}
	}
	
	private void CreatePrefab(GameObject prefab, Vector3 worldPos)
	{
		worldPos = BuildGrid.WorldToGridCenter(worldPos);
		worldPos.y = prefab.transform.position.y;
		Instantiate(prefab, worldPos, Quaternion.identity);
	}
}
