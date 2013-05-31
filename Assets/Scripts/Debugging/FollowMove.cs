using UnityEngine;
using System.Collections;

public class FollowMove : MonoBehaviour
{
	public Transform target;

	void Update()
	{
		transform.position = target.position;
	}
}
