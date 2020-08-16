using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class SliderHealth : MonoBehaviour {

	public Slider mainSlider;
	public Text currentWeaponAmmoCountText;
	public Text currentWeaponClipCountText;
	public Image ammoImage; 
	public GameObject Player;
	vp_PlayerDamageHandler playerhealth;
	vp_FPPlayerEventHandler playerEventHandler;
	//public Text valuehealth;

	// Use this for initialization
	void Awake () {
		playerhealth = Player.GetComponent<vp_PlayerDamageHandler>();
		playerEventHandler = Player.GetComponent<vp_FPPlayerEventHandler>();
	}

	// Update is called once per frame
	void Update () {
		//health slider
		mainSlider.minValue = 0f;
		mainSlider.maxValue = playerhealth.MaxHealth;
		mainSlider.value = playerhealth.CurrentHealth;
		//ammo labels
		currentWeaponAmmoCountText.text = playerEventHandler.CurrentWeaponAmmoCount.Get().ToString() + "/";
		currentWeaponClipCountText.text = playerEventHandler.CurrentWeaponClipCount.Get ().ToString ();
		Texture2D texture = playerEventHandler.CurrentAmmoIcon.Get ();
		if(texture){
			Rect rec = new Rect(0, 0, texture.width, texture.height);
			ammoImage.sprite = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
		}
	}
}