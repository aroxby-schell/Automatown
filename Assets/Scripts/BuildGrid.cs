using UnityEngine;
using System.Collections;

public class BuildGrid : MonoBehaviour
{
	private Projector projector;
	
	private static float gridSize;
	private static Vector3 gridCenter;
	
	void Awake()
	{
		projector = GetComponent<Projector>();

		gridSize = projector.orthographicSize*2.0f;
		gridCenter = projector.transform.position;
	}
	
	public static float getGridSize()
	{
		return gridSize;
	}
	
	public static Vector3 getGridCenter()
	{
		return gridCenter;
	}
	
	public static Vector3 WorldToGridCenter(Vector3 pos)
	{
		pos = WorldToGridVertex(pos);
		
		pos.x += gridSize/2.0f;
		pos.z += gridSize/2.0f;
		
		return pos;
	}
	
	public static Vector3 WorldToGridVertex(Vector3 pos)
	{
		pos -= gridCenter;
		pos.x = Mathf.Floor(pos.x / gridSize) * gridSize;
		pos.z = Mathf.Floor(pos.z / gridSize) * gridSize;
		pos += gridCenter;
		return pos;
	}
}
