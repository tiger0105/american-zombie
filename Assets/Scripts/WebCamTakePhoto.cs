using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.IO;

public class WebCamTakePhoto : MonoBehaviour
{
    public RawImage rawimage;
    private WebCamTexture webCamTexture = null;
    private int cameraIndex = 0;

    private void Start()
    {
        cameraIndex = 0;
        SetCamera();
    }

    public void SetCamera()
    {
        if (webCamTexture != null)
            webCamTexture.Stop();

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
            webCamTexture = new WebCamTexture(rearCamera.name, 1920, 1080, 60);
            rawimage.texture = webCamTexture;
            if (webCamTexture != null)
            {
                webCamTexture.Play();
            }
        }
    }

    public void SwitchCamera()
    {
        if (webCamTexture != null)
            webCamTexture.Stop();

        cameraIndex++;

        WebCamDevice[] cam_devices = WebCamTexture.devices;

        if (cam_devices.Length > 0)
        {
            webCamTexture = new WebCamTexture(cam_devices[cameraIndex % cam_devices.Length].name, 640, 400, 60);
            rawimage.texture = webCamTexture;
            if (webCamTexture != null)
            {
                webCamTexture.Play();
                Debug.Log("Web Cam Connected : " + webCamTexture.deviceName + "\n");
            }
        }
    }

    public void TakePhoto()
    {
        StartCoroutine(TakePhotoEnumerator());
    }

    private IEnumerator TakePhotoEnumerator()
    {
        yield return new WaitForEndOfFrame();

        if (webCamTexture == null)
            yield break;

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        byte[] bytes = photo.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/nimei.png", bytes);
    }

    public void StopWebCamTexture()
    {
        if (webCamTexture != null)
            webCamTexture.Stop();
    }
}
