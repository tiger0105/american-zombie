using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.IO;

public class WebCamGamePlay : MonoBehaviour
{
    public Material renderMaterial;
    private WebCamTexture webCamTexture = null;
    
    private void Start()
    {
        SetCamera();
    }

    public void SetCamera()
    {
        if (webCamTexture != null)
            webCamTexture.Stop();
        
        WebCamDevice[] cam_devices = WebCamTexture.devices;

        if (cam_devices.Length > 0)
        {
            webCamTexture = new WebCamTexture(cam_devices[0].name, 1920, 1080, 60);
            renderMaterial.mainTexture = webCamTexture;
            if (webCamTexture != null)
            {
                webCamTexture.Play();
                Debug.Log("Web Cam Connected : " + webCamTexture.deviceName + "\n");
            }
        }
    }

    public void StopWebCamTexture()
    {
        webCamTexture.Stop();
    }
}
