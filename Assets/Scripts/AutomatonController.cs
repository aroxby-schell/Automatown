using UnityEngine;
using System.Collections;

public class AutomatonController : MonoBehaviour
{
	public JunklingSensor sensor;
	public FlashRenderer errorText;
	
	public GameObject Emitter;
	public GameObject ForgeFlipped;
	public GameObject Forge;
	public GameObject Hearth;
	public GameObject Junkling;
	public GameObject Miner;
	public GameObject Path;
	public GameObject SiloFlipped;
	public GameObject Silo;

	private Vector3 destination;
	private LayerMask gridMask, tileMask;
	private int GUILayerID;
	
	private const float SPEED = 4.0f;
	private float totalPan = 0f;
	private bool panning = false;
	
	private const float MOUSE_HDEADZONE = 0.25f;
	private const float MOUSE_VDEADZONE = 0.20f;
	
	void Start()
	{
		//I don't know why this works, it seems I still don't understand Raycast
		gridMask = ~(1 << LayerMask.NameToLayer("Grid Projection"));
		gridMask &= ~(1 << LayerMask.NameToLayer("PowerField"));
		
		tileMask = 1 << LayerMask.NameToLayer("Tile Marker");
		GUILayerID = LayerMask.NameToLayer("GUI");
		FixPostion();
	}
	
	private bool isMoving()
	{
		return (Vector3.Distance(transform.position, destination)>=0.1f);
	}
	
	private float DeadZone(float t, float zone)
	{
		if(t>zone) return t-zone;
		if(-t>zone) return t+zone;
		return 0f;
	}
	
	void Update()
	{
		if(Input.GetMouseButton(0))
		{
			float hDistance = Input.GetAxis("Horizontal");
			float vDistance = Input.GetAxis("Vertical");
			
			if(!panning)
			{
				hDistance = DeadZone(hDistance, MOUSE_HDEADZONE);
				vDistance = DeadZone(vDistance, MOUSE_VDEADZONE);
				
				totalPan += Mathf.Abs(hDistance)+Mathf.Abs(vDistance);
				
				if(totalPan>MOUSE_HDEADZONE) panning = true;
			}

			Camera.main.transform.position += hDistance * Vector3.left;
			Camera.main.transform.position += vDistance * Vector3.back;
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			if(!panning)
			{
				RaycastHit hit;
				Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, gridMask);
				if(hit.collider.gameObject.layer!=GUILayerID)
				{
					SetDestination(hit.point);
					Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, tileMask);
				}
			}
			panning = false;
			totalPan = 0f;
		}
		
		if(Input.GetKeyDown(KeyCode.G))
		{
			StaticData.gears++;
		}
		
		transform.position = Vector3.MoveTowards(transform.position, destination, SPEED * Time.deltaTime);
	}
	
	private void CreatePrefab(GameObject prefab)
	{		
		Vector3 worldPos = destination;
		worldPos.y = prefab.transform.position.y;		
		Quaternion rotation = prefab.transform.rotation;		
		Instantiate(prefab, worldPos, rotation);
	}
	
	private void FixPostion()
	{
		destination = BuildGrid.WorldToGridCenter(transform.position);
		destination.y = transform.position.y;
		transform.position = destination;
	}
	
	private void SetDestination(Vector3 worldPos)
	{
		destination = BuildGrid.WorldToGridCenter(worldPos);
		destination.y = transform.position.y;
		transform.LookAt(destination);
	}
	
	private void RemoveItem()
	{
		Collider c = sensor.GetTileCollider();
		if(c)
		{
			GameObject g = c.gameObject;
			if(g.tag==TileTypes.FORGE_IN || g.tag==TileTypes.FORGE_OUT) g= g.transform.parent.gameObject;
			else if(g.tag==TileTypes.PATH) g = g.transform.parent.gameObject;
			else if(TileTypes.IsResource(g.tag)) return;
			Destroy(g);
		}
	}
	
	public void ButtonClicked(string name)
	{
		name = name.Replace(" (LTR)", " (Flipped)");
		name = name.Replace(" (RTL)", "");
		switch(name)
		{
		case "Emitter":
			CreatePrefab(Emitter);
			break;
		case "Forge (Flipped)":
			CreatePrefab(ForgeFlipped);
			break;
		case "Forge":
			CreatePrefab(Forge);
			break;
		case "Hearth":
			CreatePrefab(Hearth);
			break;
		case "Junkling":
			CreatePrefab(Junkling);
			break;
		case "Miner":
			if(StaticData.gears>0)
			{
				StaticData.gears--;
				CreatePrefab(Miner);
			}
			else errorText.Flash();
			break;
		case "Path":
			CreatePrefab(Path);
			break;
		case "Silo (Flipped)":
			CreatePrefab(SiloFlipped);
			break;
		case "Silo":
			CreatePrefab(Silo);
			break;
		case "Remove":
			RemoveItem();
			break;
			
		default:
			print("Bad button: " + name);
			break;
		}
	}
}
