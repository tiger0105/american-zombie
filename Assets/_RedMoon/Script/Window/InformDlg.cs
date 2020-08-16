using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public enum InformTypes{
	NOT_ENOUGH_COIN,
	ALREADY_PURCHASE,
	SUCCESS_PURCHASE,
	NOT_ENOUGH_TIME,
	COUNT
}
public class InformDlg : MonoBehaviour {
	public TextMeshProUGUI title;
	public TextMeshProUGUI content;
	public Image image;
	InformTypes type;
	public GameObject okButton;
	public GameObject cancelButton;

	public InformTypes Type{
		get{ return type; }
		set{ type = value;}
	}
	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnOk(){
		GameManager.Instance.GetUIMan ().OnInformAccept ();
	}
	public void OnCancel(){
		GameManager.Instance.GetUIMan ().OnInformCancel ();
	}
	public void PromptDlg(string _title, string _content, Image _image){
		StartCoroutine (DoPrompt (_title, _content, _image));
	}
	public void ModalDlg(InformTypes _type, string _title, string _content, string _image){
		type = _type;
		switch(type){
		case InformTypes.NOT_ENOUGH_COIN:
			okButton.SetActive (true);
			cancelButton.SetActive (true);
			break;
		default:			
			okButton.SetActive (false);
			cancelButton.SetActive (true);
			break;
		}
		title.text = _title;
		content.text = _content;

		if (_image == "")
			image.gameObject.SetActive (false);
		else {
			image.gameObject.SetActive (true);
			image.sprite = Resources.Load<Sprite> (_image);
		}
	}
	IEnumerator DoPrompt(string _title, string _content, Image _image){
		title.text = _title;
		content.text = _content;
		image = _image;
		image.gameObject.SetActive (true);
		yield return new WaitForSeconds (3f);
		gameObject.SetActive (false);
	}
}
