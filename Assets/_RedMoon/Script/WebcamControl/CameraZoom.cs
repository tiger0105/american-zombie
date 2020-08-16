using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
	Camera cam;
	// Use this for initialization
	void Start () {
		cam = gameObject.GetComponent<Camera> ();
	}
	public void SetFieldOfView(float val){
		cam.fieldOfView = val;
	}
}
