using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	Gyroscope 			gyro;
	Quaternion 			quatMult;
	Quaternion 			quatCam;

//	Vector3 			initPos;
//	Gyroscope			initGyro;

	public Text gyroX;
	public Text gyroY;

	public GameObject 	obj;

	void Awake() {
		gyro = Input.gyro;
		gyro.enabled = true;
		quatMult = new Quaternion(0, 0, 1, 0);

	}

	// Use this for initialization
	void Start () {
		//initPos = transform.position;
		//initGyro = Input.gyro;
	}
	
	// Update is called once per frame
	void Update () {
		
		Quaternion quatMap = new Quaternion (gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
		Quaternion qt = quatMap * quatMult;
		obj.transform.localRotation = Quaternion.Slerp(obj.transform.localRotation, qt, Time.deltaTime * 10);
		//obj.transform.Translate(gyro.rotationRateUnbiased.x, gyro.rotationRateUnbiased.y, gyro.rotationRateUnbiased.z);
//		transform.Translate(new Vector3(initPos.x + (initGyro.attitude.x - gyro.attitude.x)*30, initPos.y, initPos.z));
//		gyroX.text = (initPos.x + (initGyro.attitude.x - gyro.attitude.x)*5).ToString ();
//		gyroY.text = initPos.x.ToString ();
	}
}
