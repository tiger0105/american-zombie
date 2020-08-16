using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ItemUpgradeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (OnClicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnClicked(){
		ItemUpgradeWindow.Instance.OnItemSelectBtnClick (transform.parent.gameObject);
	}
}
