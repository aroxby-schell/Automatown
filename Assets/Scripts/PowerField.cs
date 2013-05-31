using UnityEngine;
using System.Collections;

public class PowerField : MonoBehaviour
{
	public GameObject powerFieldPrefab;
	public bool requiresPower = true;
	public float radius = 6;
	[HideInInspector]
	public GameObject instantiatedField = null;
	
	void Start()
	{
		radius *= BuildGrid.getGridSize();
		CreatePrefab(powerFieldPrefab, transform.position, Vector3.one*radius);
	}
	
	void Update()
	{
		if(requiresPower) SetPowerState(IsWorldPosPowered(transform.position, this));
	}
	
	private void CreatePrefab(GameObject prefab, Vector3 worldPos, Vector3 scale)
	{
		worldPos = BuildGrid.WorldToGridCenter(worldPos);
		
		worldPos.y = prefab.transform.position.y;
		scale.y = prefab.transform.lossyScale.y;
		
		instantiatedField = (GameObject)Instantiate(prefab, worldPos, Quaternion.identity);
		instantiatedField.transform.localScale = scale;
		instantiatedField.transform.parent = transform;
		instantiatedField.SetActive(!requiresPower);
	}
	
	public void SetPowerState(bool powered)
	{
		if(instantiatedField) instantiatedField.SetActive(powered);
	}
	
	public static bool IsWorldPosPowered(Vector3 pos, PowerField ignore = null)
	{
		int powerLayer = 1<<LayerMask.NameToLayer("PowerField");
		
		GameObject myField = ignore? ignore.instantiatedField : null;
		Collider me = myField ? myField.collider : null;
		
		//Collider[] overlap = Physics.OverlapSphere(pos, BuildGrid.getGridSize(), powerLayer);
		Collider[] overlap = Physics.OverlapSphere(pos, 0f, powerLayer);
		foreach(Collider c in overlap) if(c!=me) return true;
		return false;
	}
}
