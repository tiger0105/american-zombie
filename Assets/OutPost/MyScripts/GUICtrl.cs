using UnityEngine;
using System.Collections;

public class GUICtrl : MonoBehaviour {
	
	public Texture sightTexture;
	public float sightSize = 1;
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect((Screen.width/2)-(sightTexture.width*sightSize/2),(Screen.height/2)-(sightTexture.height*sightSize/2), sightTexture.width*sightSize, sightTexture.height*sightSize), sightTexture, ScaleMode.ScaleToFit);
	}
	
	void Start()
	{
		//Screen.lockCursor = true;			
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//Screen.lockCursor = false;
		}
	}
}
