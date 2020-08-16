using UnityEngine;
using UnityEngine.UI;
public class CoinBuyButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (OnClicked);
	}

	// Update is called once per frame
	void Update () {

	}
	void OnClicked(){
		CoinBuyWindow.Instance.OnCoinBuyButtonClick(gameObject);
	}
}
