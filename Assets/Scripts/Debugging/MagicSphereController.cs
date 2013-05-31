using UnityEngine;
using System.Collections;

public class MagicSphereController : MonoBehaviour
{
	public GameObject[] prefabs;
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.KeypadPeriod))
		{
			Destroy(gameObject);
			enabled = false;
			return;
		}
		
		for(int i = 0; i<Mathf.Min(10, prefabs.Length); i++)
		{			
			if(Input.GetKeyDown(KeyCode.Keypad0+i))
			{
				CreatePrefab(prefabs[i], transform.position);
				Destroy(gameObject);
				enabled = false;
				return;
			}
		}
		
		if(Input.GetKeyDown(KeyCode.KeypadDivide))
		{
			CreatePrefab(prefabs[10], transform.position);
			Destroy(gameObject);
			enabled = false;
			return;
		}
		
		if(Input.GetKeyDown(KeyCode.KeypadMultiply))
		{
			CreatePrefab(prefabs[11], transform.position);
			Destroy(gameObject);
			enabled = false;
			return;
		}
	}
	
	private void CreatePrefab(GameObject prefab, Vector3 worldPos)
	{
		worldPos = BuildGrid.WorldToGridCenter(worldPos);
		worldPos.y = prefab.transform.position.y;
		Instantiate(prefab, worldPos, Quaternion.identity);
	}
}
