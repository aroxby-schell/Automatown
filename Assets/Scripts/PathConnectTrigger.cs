using UnityEngine;
using System.Collections;

public class PathConnectTrigger : MonoBehaviour
{
	private PathNode pathNode;
	
	void Start()
	{
		pathNode = transform.parent.parent.GetComponent<PathNode>();
	}
	
	public void AddNode(PathNode node)
	{
		pathNode.SetConnection(this, node);
	}
	
	public void RemoveNode()
	{
		pathNode.SetConnection(this, null);
	}
}
