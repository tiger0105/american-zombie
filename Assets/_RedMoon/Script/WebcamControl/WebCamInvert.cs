using UnityEngine;
using System.IO;
using System.Collections;

public class WebCamInvert : MonoBehaviour {
	//public GUITexture mytext;
	//public UITexture mytext;
	public DeviceCameraController camController;
	private WebCamTexture frontWebcamTexture = null;

	private WebCamTexture rearWebcamTexture = null;

	private WebCamDevice[] devices;
	[HideInInspector]
	public WebCamTexture activeCam;


	bool flag = false ;
	static int cameraIndex = 0 ;

	/* 按钮控件 */
	public GameObject btn_shot ;
	public GameObject btn_change_camera ;

	void Awake () {


		for( int i = 0 ; i < WebCamTexture.devices.Length ; i++ )
			Debug.Log(WebCamTexture.devices[i].name);

		devices = WebCamTexture.devices;

		string frontCamName = "";

		string rearCamName = "";

		for(int i=0; i<devices.Length; i++)
		{

			if (devices[i].isFrontFacing)

				frontCamName = devices[i].name;

			else

				rearCamName = devices[i].name;

		}

		frontWebcamTexture = new WebCamTexture(frontCamName);

		rearWebcamTexture = new WebCamTexture(rearCamName);

		frontWebcamTexture.Stop();
		rearWebcamTexture.Stop();
		activeCam = rearWebcamTexture;
		//mytext.material.mainTexture = rearWebcamTexture ;
		//mytext.material.SetTexture("_MainTex", rearWebcamTexture);
		activeCam.Play();
		camController.cameraTexture = activeCam;

	}

	void Start()

	{

		/* 事件 */
//		CustomEventListener.Get(btn_shot).onClick += OnShotButtonClick ;
//		CustomEventListener.Get(btn_change_camera).onClick += OnCameraChangeClick ;

		Debug.Log ("Active camera: " + activeCam);

	}

	public bool HasFrontCamera()
	{

		if (Application.isEditor)

			return false;

		return frontWebcamTexture.deviceName != "";

	}


	public void OnShotButtonClick (GameObject go) {
		SavePhoto() ;
	}

	/* 相机切换 */
	public void OnCameraChangeClick (GameObject go) {

		if (!HasFrontCamera())

			return;


		activeCam.Stop ();

		if (activeCam == frontWebcamTexture)

		{

			Debug.Log ("Switching to rear cam...");

			//renderer.material.mainTexture = rearWebcamTexture;
			//mytext.material.mainTexture = rearWebcamTexture ;
			//mytext.material.SetTexture("_MainTex", rearWebcamTexture);
			activeCam = rearWebcamTexture;

		}

		else

		{

			Debug.Log ("Switching to front cam...");


			//mytext.material.mainTexture = frontWebcamTexture ;
			//mytext.material.SetTexture("_MainTex", frontWebcamTexture);

			activeCam = frontWebcamTexture;

		}

		Debug.Log ("... done.");

		Debug.Log("Trying to Play the active WebCamTexture: (" + activeCam + ")");

		activeCam.Play();
	}

	public void ShowCamera()
	{
		//myCameraTexture.guiTexture.enabled = true;
		//mytext.enabled = true;
		activeCam.Play();
		//Debug.Log("-------------show--------->>>>>" + mytext) ;

	}

	public void HideCamera()
	{
		//Debug.Log("-------------hide--------->>>>>" + mytext) ;
		//myCameraTexture.guiTexture.enabled = false;
		//mytext.enabled = false;
		activeCam.Stop();

	}

	public void SavePhoto() {

//		Debug.Log("***************oh fuck***********************finally worked!!!") ;
//
//		Texture tex = mytext.material.mainTexture;
//
//		Texture2D tx = new Texture2D(activeCam.width, activeCam.height) ;
//		tx.SetPixels(activeCam.GetPixels());
//
//
//		byte[] byte_photo = tx.EncodeToPNG() ;
//		Debug.Log("#####length######################333333" + byte_photo.Length) ;
//		string photoName = "nimei.png" ;
//
//		FileStream fs = new System.IO.FileStream("/mnt/sdcard/DCIM/Camera/nimei.png", System.IO.FileMode.Create);
//		fs.Write(byte_photo,0,byte_photo.Length);
//		fs.Close();
	}

}
