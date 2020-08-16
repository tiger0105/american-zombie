/// <summary>
/// write by 52cwalk,if you have some question ,please contract lycwalk@gmail.com
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DeviceCameraController : MonoBehaviour {

	public enum CameraMode
	{
		FACE_C,
		DEFAULT_C,
		NONE
	}
	[HideInInspector]
	public WebCamTexture cameraTexture; 

	private bool isPlay = false;
	//public CameraMode e_CameraMode;
	GameObject e_CameraPlaneObj;
//	int matIndex = 0;

	ScreenOrientation orientation;
	public bool isPlaying
	{
		get{
			return isPlay;
		}
	}
	// Use this for initialization  
	void Awake()  
	{  
		StartCoroutine(CamCon());  
		e_CameraPlaneObj = transform.Find ("CameraPlane").gameObject;

	}
	
	// Update is called once per frame  
	void Update()  
	{  
		if (isPlay) {  
			if(e_CameraPlaneObj.activeSelf)
			{
				e_CameraPlaneObj.GetComponent<Renderer>().material.mainTexture = cameraTexture;
			}


		}
	
	}

	IEnumerator CamCon()  
	{  
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);  
		if (Application.HasUserAuthorization(UserAuthorization.WebCam))  
		{  
			#if UNITY_EDITOR 
			cameraTexture = new WebCamTexture();  	
			#elif UNITY_IOS
			cameraTexture = new WebCamTexture(960,640);
			#elif UNITY_ANDROID
			cameraTexture = new WebCamTexture(960,640);  
			#else
			cameraTexture = new WebCamTexture();  
			#endif
			cameraTexture.Play();
			isPlay = true;  
		}  
	}
	
	public void StopWork()
	{
		this.cameraTexture.Stop();
	}

}

