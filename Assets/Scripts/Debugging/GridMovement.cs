using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour
{
	private LayerMask gridMask;
	
	void Start()
	{
		moveTo(transform.position);
		gridMask = ~(1 << LayerMask.NameToLayer("Grid Projection"));
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{			
			RaycastHit hit;			
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, gridMask);
			moveTo(hit.point);
		}
	}
	
	private void moveTo(Vector3 worldPos)
	{
		worldPos.y = transform.position.y;
		worldPos = BuildGrid.WorldToGridCenter(worldPos);		
		transform.position = worldPos;
	}
}
