using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoButtonCreate : MonoBehaviour
{
	public Transform buttonGroup;
	public GameObject reciever;
	public TextMesh gearsLbl;
	
	private List<Pair> buttons = new List<Pair>();
	
	private float w,h,xOff,yOff;
	
	void Start()
	{
		w = Screen.width;
		h = Screen.height;
		xOff = Screen.width;
		yOff = Screen.height;
		w *= 0.12f;
		h *= 0.05f;
		xOff *= 0.045f;
		yOff *= -0.02f;
		
		foreach(Transform t in buttonGroup)
		{
			string btnName;
			if(t.name=="Remove") btnName = "Remove Item";
			else btnName = "Build " + t.name;
			
			buttons.Add( new Pair(CreateButtonRect(t), btnName) );
		}
	}
	
	Vector3 worldPosToScreenPos(Vector3 xy)
	{
		Vector3 screen = Camera.main.WorldToScreenPoint(xy);
		screen.y = Screen.height - screen.y;
		return screen;
	}
	
	Vector3 worldPosToScreenSize(Vector3 xy)
	{
		Vector3 screen = Camera.main.WorldToScreenPoint(xy);
		return screen;
	}
	
	Rect CreateButtonRect(Transform t)
	{
		Vector3 pos = worldPosToScreenPos(t.position);
		
		return new Rect(pos.x+xOff, pos.y+yOff, w, h);
	}
	
	void OnGUI()
	{
		GUIStyle buttonStyle = new GUIStyle( GUI.skin.button );
		buttonStyle.alignment = TextAnchor.MiddleLeft;
		
		foreach(Pair p in buttons)
		{			
			if(GUI.Button(p.area, p.name, buttonStyle))
			{
				if(p.name=="Remove Item") reciever.SendMessage("ButtonClicked", "Remove");
				else reciever.SendMessage("ButtonClicked", p.name.Substring(6));
			}
		}
		
		gearsLbl.text = "Gears x " + StaticData.gears;
	}
	
	private class Pair
	{
		public Rect area;
		public string name;
		
		public Pair(Rect r, string s)
		{
			area = r;
			name = s;
		}
	}
}
