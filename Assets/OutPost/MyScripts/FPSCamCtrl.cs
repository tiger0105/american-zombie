using UnityEngine;
using System.Collections;

public class FPSCamCtrl : MonoBehaviour {
	

	public float sensitivityX = 1f;
	public float sensitivityY = 1f;

	public float minY = -60f;
	public float maxY = 60f;

	float rotationY = 0f;

	void Update ()
	{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minY, maxY);			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
	}
}
