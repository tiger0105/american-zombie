﻿using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public WebCamTexture mCamera = null;
	public GameObject plane;
	
	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Script has been started");
		plane = GameObject.FindWithTag ("Player");
		Renderer render = plane.GetComponent<Renderer> ();

		mCamera = new WebCamTexture ();
		render.material.mainTexture = mCamera;
		mCamera.Play ();
	}

}
