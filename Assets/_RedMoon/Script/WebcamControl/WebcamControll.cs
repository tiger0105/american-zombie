using UnityEngine;
using System.Collections;

public class WebcamControll : MonoBehaviour {
	public WebCamTexture rearWebcamTexture = null;
	GameObject e_CameraPlaneObj;
    private bool isPlaying = false;

    public bool GetIsPlaying()
    {
        return isPlaying;
    }

    private void SetIsPlaying(bool value)
    {
        isPlaying = value;
    }

    IEnumerator Start() {
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);  

		if (Application.HasUserAuthorization(UserAuthorization.WebCam))  
		{
            Debug.Log("HasUserAuthorization");
            WebCamDevice[] devices = WebCamTexture.devices;
            WebCamDevice rearCamera = new WebCamDevice();
            bool hasARearCamera = false;
#if !UNITY_EDITOR && UNITY_ANDROID || !UNITY_EDITOR && UNITY_IOS
            foreach (WebCamDevice cam in devices)
            {
                if (!cam.isFrontFacing)
                {
                    rearCamera = cam;
                    hasARearCamera = true;
                    break;
                }
            }
#elif UNITY_EDITOR
            foreach (WebCamDevice cam in devices)
            {
                rearCamera = cam;
                hasARearCamera = true;
                break;
            }
#endif

            if (hasARearCamera == true)
            {
                if (rearWebcamTexture != null)
                    rearWebcamTexture.Stop();

#if UNITY_EDITOR
                rearWebcamTexture = new WebCamTexture(rearCamera.name, Screen.width, Screen.height);
#elif UNITY_IOS
			    rearWebcamTexture = new WebCamTexture(rearCamera.name, 960, 640);
#elif UNITY_ANDROID
			    rearWebcamTexture = new WebCamTexture(rearCamera.name, 960, 640);
#else
			    rearWebcamTexture = new WebCamTexture(rearCamera.name, 960, 640);
#endif
                rearWebcamTexture.Play();
                SetIsPlaying(true);
            }
		}

		e_CameraPlaneObj = transform.Find ("CameraPlane").gameObject;

		yield return new WaitForEndOfFrame ();
	}

	void Update()  
	{  
		if (GetIsPlaying() && e_CameraPlaneObj.activeSelf)
			e_CameraPlaneObj.GetComponent<Renderer>().material.mainTexture = rearWebcamTexture;		
	}

    public void StopWebCamTexture()
    {
        if (rearWebcamTexture != null)
            rearWebcamTexture.Stop();
    }
}