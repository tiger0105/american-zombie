using UnityEngine;
using System.Collections;

public class GetImages : MonoBehaviour {

	public Material frontimage;
	public Material rightimage;
	public Material leftimage;
	public Material backimage;
	public Material upimage;
	public Material downimage;

	public int size = 512;
	public int zoom = 0;
	public WWW DownloadImg;
	double lat;
	double lon;
	public double Lat{
		get{return lat; }
		set{lat = value; }
	}
	public double Lon{
		get{ return lon; }
		set{ lon = value;}
	}

	// Use this for initialization
	IEnumerator Start () {
//		double lat = AppConst.g_lat;//128;//GlobalObject.getInstance().Params[0];
//		double lon = AppConst.g_lot;//40;//GlobalObject.getInstance().Params[1];

		var frontTex = RetrieveGPSData(0, 0, lat, lon);
		var leftTex = RetrieveGPSData(90, 0, lat, lon);
		var rightTex = RetrieveGPSData(270, 0, lat, lon);
		var backTex = RetrieveGPSData(180, 0, lat, lon);
		var upTex = RetrieveGPSData(0, 90, lat, lon);
		var downTex = RetrieveGPSData(0, -90, lat, lon);
		yield return new WaitForSeconds(4);

		RenderSettings.skybox.SetTexture("_FrontTex",frontTex.texture);
		RenderSettings.skybox.SetTexture("_LeftTex",leftTex.texture);
		RenderSettings.skybox.SetTexture("_BackTex",backTex.texture);
		RenderSettings.skybox.SetTexture("_RightTex",rightTex.texture);
		RenderSettings.skybox.SetTexture("_UpTex",upTex.texture);
		RenderSettings.skybox.SetTexture("_DownTex",downTex.texture);		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			//GameManager.Instance.GetUIMan().streetviewObject.GetComponent<StreetView>().OnExit ();
			//Combat_UIManager.Instance.streetView.GetComponent<StreetView>().OnExit();
		}
	}

	WWW RetrieveGPSData(float head, float pit, double lat, double lon) {
		var url = "http://maps.googleapis.com/maps/api/streetview?";
		var qs = "";
		qs += "size=" + size + "x" + size;
		qs += "&location=" + lat + "," + lon;
		qs += "&heading=" + head + "&pitch=" + pit;  
		qs += "&fov=90.0&sensor=false";

		url += qs;
		Debug.Log (url);
		DownloadImg = new WWW(url);

		return DownloadImg ; 
	}
}