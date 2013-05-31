using UnityEngine;
using System.Collections;

public class TileTypes
{
	public const string NONE = "None";
	
	public const string PATH = "Path";
	
	public const string SCRAP = "Scrap";
	public const string GRASS = "Grass";
	public const string WATER = "Water";
	public const string ROCK = "Rock";
	public const string GEAR = "Gears";
	public const string STEAM = "Steam";
	
	public const string EMITTER = "Emitter";	
	public const string HEARTH = "Hearth";
	public const string SHED = "Shed";
	//public const string FORGE = "Forge";
	public const string SILO = "Silo";
	
	public const string FORGE_IN = "Forge In";
	public const string FORGE_OUT = "Forge Out";
	
	public static bool IsResource(string type)
	{
		//if(type==PATH || type==NONE) return false;		
		//Debug.Log("IsResource: " + type);
		
		if(type==SCRAP) return true;
		if(type==GRASS) return true;
		if(type==WATER) return true;
		if(type==ROCK) return true;
		if(type==GEAR) return true;
		if(type==STEAM) return true;
		
		if(type==FORGE_OUT) return true;
		
		//Debug.Log("IsResource: NO.");
		
		return false;
	}
	
	public static bool IsDepot(string type)
	{		
		if(type==HEARTH) return true;
		if(type==FORGE_IN) return true;
		if(type==SILO) return true;		
		return false;
	}
}
