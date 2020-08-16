using UnityEngine;
using System.Collections;

public class CandleControl : MonoBehaviour {
	public MenuSceneControl menuSceneControl;
	public GameObject fire;
	public GameObject light; 
	public AudioSource blowOutAudio;
	public float blowTime;
	bool blowFlag;
	float timeSpan;
	// Update is called once per frame
	void Update () {
		if (blowFlag) {
			timeSpan += Time.fixedDeltaTime;
		} else {
			return;
		}
		if(timeSpan > blowTime){
			blowFlag = false;
			light.SetActive (false);
			menuSceneControl.ProcEvent ("CandleBlowed", gameObject);
		}
	}
	public void BlowOut(){
		timeSpan = 0f;
		blowFlag = true;
		if(blowOutAudio != null) blowOutAudio.Play ();
		fire.GetComponent<ParticleSystem> ().loop = false;
	}
}
