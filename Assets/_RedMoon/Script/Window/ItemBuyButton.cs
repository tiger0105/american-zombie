using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ItemBuyButton : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (OnClicked);
	}

	// Update is called once per frame
	void Update () {

	}
	void OnClicked(){
		ItemBuyWindow.Instance.OnItemSelectBtnClick (gameObject);
	}
}
