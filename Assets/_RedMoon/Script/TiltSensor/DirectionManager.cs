using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DirectionManager : MonoBehaviour
{
	public Text logText;
	public vp_FPCamera targetCamera;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
        return;
#endif
		Input.gyro.enabled = true;
		Input.gyro.updateInterval = 0.01f;
		//logText.text = "Gyro Enabled.........";
        targetCamera = GameObject.FindObjectOfType<vp_FPCamera>();
	}
	
	// Update is called once per frame
	void Update() {
		if (!Input.gyro.enabled) {
			logText.text = "Gyro: Not Working";
			return;
		}
//		//this returns the direction use Vector3.right if Vector3.up doesn't work
//		Vector3 direction = Input.gyro.attitude * Vector3.up;
//		//this just will take away any up or down rotation
//		direction.y = 0;
//
//		Quaternion targetRotation = Quaternion.LookRotation(direction,Vector3.up);

        Vector3 rot = Input.gyro.attitude.eulerAngles;
        //Vector3 gravity = Input.gyro.gravity;
		//logText.text = string.Format ("Gyro Rotation:{0}", rot);
		logText.text = "Gyro: Working";
        Quaternion targetRotation = Quaternion.Euler(90, 0, 0) * Quaternion.Euler(-rot.x, -rot.y, rot.z);
        Vector3 euler = targetRotation.eulerAngles;
		Debug.Log ("rot:" + rot);
//        float cross = Input.gyro.gravity.y;
//        if(cross < -0.95f)
//        {
//            euler.y = Input.compass.trueHeading;
//        }

		Quaternion nextRotation = Quaternion.Lerp(targetCamera.Transform.rotation, targetRotation, 5f*Time.deltaTime);

		targetCamera.SetRotation(new Vector2(nextRotation.eulerAngles.x, nextRotation.eulerAngles.y));
    }
}
