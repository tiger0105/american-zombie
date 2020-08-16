using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushNotification : MonoBehaviour {

	public void OnClosePushNotification()
    {
        gameObject.SetActive(false);
    }
}
