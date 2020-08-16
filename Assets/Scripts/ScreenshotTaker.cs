using NatShareU;
using System.Collections;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    public void CaptureScreenshot()
    {
        StartCoroutine(TakeScreenshotAndSave());
    }

    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
        NatShare.SaveToCameraRoll(ss);
        bool canShare = NatShare.Share(ss);
        if (canShare == true)
        {
            GlobalReferences.ShareWithFB = PlayerPrefs.GetInt("ShareWithFB", 0);
            GlobalReferences.ShareWithInsta = PlayerPrefs.GetInt("ShareWithInsta", 0);

            if (GlobalReferences.ShareWithFB == 0 && GlobalReferences.ShareWithInsta == 0)
                PlayerPrefs.SetInt("ShareWithFB", 1);
            else if (GlobalReferences.ShareWithFB == 0 && GlobalReferences.ShareWithInsta == 1)
                PlayerPrefs.SetInt("ShareWithFB", 1);
            else if (GlobalReferences.ShareWithFB == 1 && GlobalReferences.ShareWithInsta == 0)
                PlayerPrefs.SetInt("ShareWithInsta", 1);


        }
    }
}