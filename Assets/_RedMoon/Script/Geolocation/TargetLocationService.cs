using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using LitJson;

public class TargetLocationService : MonoBehaviour {
	TargetManager targetMan;
	WWWHelper 			helper;
	// Use this for initialization
	void Start () {
		targetMan = GameManager.Instance.GetTargetMan();
		string url = "http://flamingoprints.com/admin/index.php/Admin/GetDragonData";
		helper = WWWHelper.Instance;
		helper.OnHttpRequest += OnHttpRequest;
		helper.get (100, url);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnHttpRequest(int id, WWW www) {
		if (www.error != null) {
			Debug.Log ("[Error] " + www.error);
		} else {
			//Debug.Log (www.text);
			JsonData jsonData = JsonMapper.ToObject (www.text);
			jsonData = jsonData[0];
			int count = jsonData.Count; 
//			targetMan.targetList = new ArrayList ();


			//			JsonData owner = data["owner"];
			////			string v_owner = owner.ToString ();
			//			m_OwnerName = owner.ToString();
			//			
			//			JsonData trackData = data ["trackData"];
			//			JsonData gpsLan = trackData [0] ["gpsLat"];
			////			double v_gpsLan = double.Parse (gpsLan.ToString ());
			//			targetLatitude = double.Parse(gpsLan.ToString());
			////			targetLatitude = 43.299D;
			//
			//
			//			JsonData gpsLong = trackData [0] ["gpsLongt"];
			////			double v_gpsLong = double.Parse (gpsLong.ToString ());
			//			targetLongitude = double.Parse(gpsLong.ToString());
			////			targetLongitude = 43.299D;
			//
			//			JsonData gpsVelocity = trackData [0] ["gpsVelocity"];
			////			double v_gpsVelocity = double.Parse (gpsVelocity.ToString());
			//			m_GpsVelocity = double.Parse(gpsVelocity.ToString());

			for(int i = 0; i<count; i++){
				JsonData data = jsonData [i];
				JsonData gpsLan = data ["lat"];
				JsonData gpsLong = data ["lng"];
				JsonData _dragonID = data["dragonId"];
				double targetLatitude = double.Parse(gpsLan.ToString());
				double targetLongitude = double.Parse(gpsLong.ToString());
				int dragonID = int.Parse (_dragonID.ToString()); 
				Target aTarget = new Target ();
				//aTarget.= dragonID;
				aTarget.longitude = targetLongitude;
				aTarget.latitude = targetLatitude;
//				targetMan.targetList.Add (aTarget);
//				Debug.Log ("count     >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + count);
//				Debug.Log ("id        >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + aTarget.targetID);
//				Debug.Log ("longitude >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + aTarget.longitude);
//				Debug.Log ("latitude  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + aTarget.latitude);

			}
		}
	}
}
