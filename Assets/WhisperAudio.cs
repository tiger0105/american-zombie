using UnityEngine;

public class WhisperAudio : MonoBehaviour {

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
