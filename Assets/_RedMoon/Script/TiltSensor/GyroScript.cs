using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GyroScript : MonoBehaviour {
	
	public Text 		gyroX;
	public Text 		gyroY;
	public Text 		gyroZ;
	public Text 		gyroW;
//	public GameObject	firstPointer;
//	public GameObject	secondPointer;
//	public GameObject	obj;


	float 			gyroSensitivity = 50.0f;
	float 			x, y, z;
	Vector3			currentPosition;
	Vector3			initPosition;
	Gyroscope 		gyro;

	Quaternion 			quatMult;
	Quaternion 			quatCam;

	// Use this for initialization
	void Awake () {
		gyro = Input.gyro;
		gyro.enabled = true;

	}

	void Start()
	{
		quatMult = new Quaternion(0, 0, 1, 0);

		//only for decision of localrotation of the camera.
		//Quaternion quatMap = new Quaternion (0.2f, 0.57f, 0.73f, 0.3f);
//		Quaternion quatMap = new Quaternion (0.3577917f, 0.4798584F, 0.6417891f, 0.4794065f);
//		Quaternion qt = quatMap * quatMult;
//		transform.localRotation = qt;
	}

	// Update is called once per frame
	void Update () {
//		gyroX.text = gyro.attitude.x.ToString();
//		gyroY.text = gyro.attitude.y.ToString();
//		gyroZ.text = gyro.attitude.z.ToString();
//		gyroW.text = gyro.attitude.w.ToString();

		Quaternion quatMap = new Quaternion (gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
		Quaternion qt = quatMap * quatMult;
		transform.localRotation = Quaternion.Slerp(transform.localRotation, qt, Time.deltaTime * 7f);
		//transform.localRotation = qt;
	}

	void RotateUpDown(float axis){
		transform.RotateAround(transform.position , transform.right, -axis * Time.deltaTime * gyroSensitivity);
	}

	//rotate the camera rigt and left (y rotation)
	void RotateRightLeft(float axis){
		transform.RotateAround(transform.position, Vector3.up, -axis * Time.deltaTime * gyroSensitivity);
	}

	void RotateZee(float axis){
		transform.RotateAround(transform.position, new Vector3(0,0,1), -axis * Time.deltaTime * gyroSensitivity);
	}
}
