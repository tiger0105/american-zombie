using UnityEngine;
using System.Collections;

public class FrontCam : MonoBehaviour {
	WebCamDevice[] device = WebCamTexture.devices;
	public GameObject plane;
	// Use this for initialization
	void Start () {

		plane = GameObject.FindWithTag ("Player");

		foreach(WebCamDevice cam in device)
		{
			if(cam.isFrontFacing )
			{    
				WebCamTexture webCameraTexture  =   new WebCamTexture(1, Screen.width, Screen.height);
				//Renderer render = plane.GetComponent<Renderer> ();
				webCameraTexture.deviceName  = cam.name;
				webCameraTexture.Play();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
