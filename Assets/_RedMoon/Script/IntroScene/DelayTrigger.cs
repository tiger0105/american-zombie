using UnityEngine;
using System.Collections;


public class DelayTrigger : MonoBehaviour {
	public GameObject starImage;
	public GameObject redRumText;
	float timeSpan;
	public float limit = 3f;
	public GameObject trigger;




	// Use this for initialization
	void Start () {
		timeSpan = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeSpan <3f){
			
		}
		if (timeSpan > 3f) {
			if(trigger == false) trigger.SetActive (true);
		}
	}
}
