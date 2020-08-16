﻿using UnityEngine;
using System.Collections;

public class CameraPivot : MonoBehaviour
{
	float _BaseAngle = -90f;
	
	void Start()
	{
		Input.gyro.enabled = true;
	}
	
	void Update()
	{
		Quaternion q1 = Input.gyro.attitude;
		Quaternion q2 = new Quaternion(q1.y, -q1.z, -q1.x, q1.w);
		
		transform.rotation = Quaternion.Euler(new Vector3(q2.eulerAngles.x, _BaseAngle, q2.eulerAngles.z));
		if (transform.rotation.eulerAngles.z <= 275f && transform.rotation.eulerAngles.z > 180f)
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 275f));
		if (transform.rotation.eulerAngles.z >= 355f || transform.rotation.eulerAngles.z <= 180f)
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 355f));
	}
}