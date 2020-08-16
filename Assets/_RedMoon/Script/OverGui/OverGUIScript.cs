using UnityEngine;
using System.Collections;

public class OverGUIScript : MonoBehaviour {
	public Camera overGUICamera;

	void OnGUI(){
		if (Event.current.type == EventType.Repaint)
		{
			overGUICamera.Render();
		}
	}
}
