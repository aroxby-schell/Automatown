using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode : MonoBehaviour
{
	public PathConnectTrigger northCollider;
	public PathConnectTrigger southCollider;
	public PathConnectTrigger eastCollider;
	public PathConnectTrigger westCollider;
	
	public GameObject northMesh;
	public GameObject southMesh;
	public GameObject eastMesh;
	public GameObject westMesh;
	
	public enum AdjacencyMask
	{
		none = 0,
		north = 1,
		south = 2,
		east = 4,
		west = 8
	}
	
	private class ReferenceCompare<T> : IEqualityComparer<T>
	{
		public bool Equals(T a, T b) { return object.ReferenceEquals(a,b); }
		public int GetHashCode(T obj) { return ((object)obj).GetHashCode(); }
	}
	
	private int gridLayer;
	private AdjacencyMask adjacencies = AdjacencyMask.none;
	
	private Dictionary<PathConnectTrigger, AdjacencyMask> triggerDictionary = 
		new Dictionary<PathConnectTrigger, AdjacencyMask>( new ReferenceCompare<PathConnectTrigger>() );
	
	private SortedDictionary<AdjacencyMask, GameObject> meshDictionary = 
		new SortedDictionary<AdjacencyMask, GameObject>();
	
	private SortedDictionary<AdjacencyMask, PathNode> connectionDictionary = 
		new SortedDictionary<AdjacencyMask, PathNode>();
	
	void Start()
	{
		triggerDictionary.Add(northCollider, AdjacencyMask.north);
		triggerDictionary.Add(southCollider, AdjacencyMask.south);
		triggerDictionary.Add(eastCollider, AdjacencyMask.east);
		triggerDictionary.Add(westCollider, AdjacencyMask.west);
		
		meshDictionary.Add(AdjacencyMask.north, northMesh);
		meshDictionary.Add(AdjacencyMask.south, southMesh);
		meshDictionary.Add(AdjacencyMask.east, eastMesh);
		meshDictionary.Add(AdjacencyMask.west, westMesh);
		
		gridLayer = LayerMask.NameToLayer("Grid Projectable");
	}
	
	void OnTriggerEnter(Collider trigger)
	{
		if(trigger.gameObject.layer==gridLayer) return;
		
		PathConnectTrigger pct;
		pct = trigger.GetComponent<PathConnectTrigger>();
		if(pct) pct.AddNode(this);
	}
	
	void OnTriggerExit(Collider trigger)
	{
		if(trigger.gameObject.layer==gridLayer) return;
		
		PathConnectTrigger pct;
		pct = trigger.GetComponent<PathConnectTrigger>();
		if(pct) pct.RemoveNode();
	}
	
	public void SetConnection(PathConnectTrigger trigger, PathNode connection)
	{
		SetConnection(triggerDictionary[trigger], connection);
	}
	
	public void SetConnection(AdjacencyMask direction, PathNode connection)
	{
		if(connection != null) adjacencies |= direction;
		else adjacencies &= ~direction;
		
		connectionDictionary[direction] = connection;			
		UpdateVisual();
	}
	
	private void UpdateVisual()
	{
		UpdateVisual(AdjacencyMask.north);
		UpdateVisual(AdjacencyMask.south);
		UpdateVisual(AdjacencyMask.east);
		UpdateVisual(AdjacencyMask.west);
	}
	
	private void UpdateVisual(AdjacencyMask direction)
	{
		meshDictionary[direction].SetActive( (adjacencies&direction)!=0 );
	}
	
	private PathNode TraverseConnection(AdjacencyMask direction)
	{
		PathNode node;
		if(!connectionDictionary.TryGetValue(direction, out node)) return null;
		return node;
	}
	
	private void WithdrawFrom(AdjacencyMask direction)
	{
		PathNode neighbor = TraverseConnection(direction);
		if(neighbor!=null) neighbor.SetConnection( Reverse(direction), null );
	}
	
	void OnDestroy()
	{
		WithdrawFrom(AdjacencyMask.north);
		WithdrawFrom(AdjacencyMask.south);
		WithdrawFrom(AdjacencyMask.east);
		WithdrawFrom(AdjacencyMask.west);
	}
	
	private AdjacencyMask Reverse(AdjacencyMask dir)
	{
		switch(dir)
		{
		case AdjacencyMask.north:
			dir = AdjacencyMask.south;
			break;
			
		case AdjacencyMask.south:
			dir = AdjacencyMask.north;
			break;
			
		case AdjacencyMask.east:
			dir = AdjacencyMask.west;
			break;
			
		case AdjacencyMask.west:
			dir = AdjacencyMask.east;
			break;
		}
		
		return dir;
	}
}
