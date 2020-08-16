using UnityEngine;
using System.Collections;

public class MenuSceneControl : MonoBehaviour {
	// Use this for initialization
	bool isBusy;
	GameObject activeObj;
	public GameObject startObj;
	public GameObject optionObj;
	public GameObject scoreObj;

	float timeSpan;

	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void  TriggerState(GameObject obj){
		if (obj == startObj) {
			TriggerStart ();
		}
		else if (obj == optionObj) {
			TriggerOption ();
		}
		else if(obj == scoreObj) {
			TriggerScore ();
		}
	}

	void TriggerStart(){
		if (isBusy)
			return;
		isBusy = true;
		startObj.GetComponent<CandleControl> ().BlowOut ();
		optionObj.GetComponent<CandleControl> ().BlowOut ();
		scoreObj.GetComponent<CandleControl> ().BlowOut ();
	}
	void TriggerOption(){
		
	}
	void TriggerScore(){
		
	}
	public void ProcEvent(string str, GameObject obj){
		switch(str){
		case "CandleBlowed":
			if(obj == startObj) obj.GetComponent<ChildTransformChange> ().Enter (NextScene);
			break;
		default:
			break;
		}
	}
	void NextScene(){
		Application.LoadLevel (Application.loadedLevel + 1);
	}
	void HideAllCandleLights(){
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("CandleLight")) {
			go.SetActive (false);
		}
	}
}