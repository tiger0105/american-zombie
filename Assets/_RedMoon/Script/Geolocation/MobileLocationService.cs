using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DeadMosquito.AndroidGoodies;

public class MobileLocationService : MonoBehaviour {
	public Text CurrentLocation;
	public static bool isTracking = false;

	void Start() {
		//try {
		//	AGGPS.RequestLocationUpdates (200, 1, OnLocationChanged);
		//	AGGPS.Location location = AGGPS.GetLastKnownGPSLocation();
		//	GameInfo.curMobileLatitude = location.Latitude;
		//	GameInfo.curMobileLongitude = location.Longitude;
		//	//CurrentLocation.text = location.Latitude + " " + location.Longitude + " " + location.Accuracy + " " + location.Timestamp;
		//} catch (Exception) {
		//	//CurrentLocation.text = "Could not get last known location.";
		//}
	}
				
	//private void OnLocationChanged(AGGPS.Location location)	{
	//	//CurrentLocation.text = location.Latitude + " " + location.Longitude + " " + location.Accuracy + " " + location.Timestamp;
	//	GameInfo.curMobileLatitude = location.Latitude;
	//	GameInfo.curMobileLongitude = location.Longitude;
	//}
}

//unity documentation
//public class MobileLocationService : MonoBehaviour
//{
//	public Text CurrentLocationText;
//	string stateStr;
//	public string StateStr{
//		get{ return stateStr;}
//	}
//	static bool isTryed;
//	public static bool IsTryed{
//		get{ return isTryed;}
//	}	 
//	IEnumerator Start()
//	{
//		// First, check if user has location service enabled
//		isTryed = false;
//		if (!Input.location.isEnabledByUser) {
//			stateStr = ("user has not enabled location service");	
//			CurrentLocationText.text = stateStr;
//			isTryed = true;
//			yield break;
//		}
//		Debug.Log ("Before..................... Input.location.Start()");
//		// Start service before querying location
//		Input.location.Start(0.1f, 0.1f);
//		Debug.Log ("After....................... Input.location.Start()");
//		// Wait until service initializes
//		int maxWait = 20;
//		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
//		{
//			yield return new WaitForSeconds(1);
//			maxWait--;
//		}
//
//		// Service didn't initialize in 20 seconds
//		if (maxWait < 1)
//		{
//			print("Timed out");
//			stateStr = "Time out";
//			CurrentLocationText.text = stateStr;
//			isTryed = true;
//			yield break;
//		}
//
//		// Connection has failed
//		if (Input.location.status == LocationServiceStatus.Failed) {
//			print ("Unable to determine device location");
//			stateStr = "Service Failed";
//			CurrentLocationText.text = stateStr;
//			isTryed = true;
//			yield break;
//		}
//		else
//		{
//			// Access granted and location value could be retrieved
//			print(Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
//			stateStr = Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
//			CurrentLocationText.text = stateStr;
//			isTryed = true;
//		}
//
//		// Stop service if there is no need to query location updates continuously
//		//Input.location.Stop();
//	}