using UnityEngine;
using System.Collections;

public class ItemModelBase : MonoBehaviour {
	public static ItemModelBase Instance;
	GameObject curModel;

	public void ShowModel(GameObject obj){
		DestroyCurModel ();
		curModel = obj;
		curModel.SetActive (true);
	}
	public void DestroyCurModel(){
		if (curModel != null) {
			GameObject.DestroyImmediate (curModel);
			curModel = null;
		}
	}
	void Awake(){
		Instance = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
