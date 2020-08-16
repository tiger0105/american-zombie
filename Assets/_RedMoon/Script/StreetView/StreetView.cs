using UnityEngine;
using System.Collections;
using Microsoft.MapPoint;
using UnityEngine.UI;
using Polaris.GameData;
public class StreetView : MonoBehaviour {
	public GameObject streetObj;
	//public GameObject[] monsterModels;
	//public GameObject[] weaponModels;
	GameObject curModel;
	//SmartWebViewer wv;
	Target curTarget;
	public void OnTrigger(GameObject spot){
//		wv = new SmartWebViewer ();
//		string web;
//		curTarget = mon;
//		web = string.Format ("https://www.instantstreetview.com/@{0},{1},-109.42h,-8.37p,0z", mon.latitude, mon.longitude);
		//web = "http://www.baidu.com";
		//wv.OpenWebView (web, 0, 0, (int)(Screen.width*1.3f/3f), 0);

		if (spot.tag == "monster spot") {
			for (int i = 0; i < RadarBehavior.instance.monsterSpotList.Count; i++) {
				if (spot.Equals (RadarBehavior.instance.monsterSpotList [i])) {
					Monster mon = (Monster)GameManager.Instance.GetTargetMan ().monsterList [i];
					TileSystem.PixelXYToLatLong (GameManager.Instance.GetTargetMan ().player.pixelX + (int)mon.realObject.transform.position.x, GameManager.Instance.GetTargetMan ().player.pixelY + (int)mon.realObject.transform.position.z, Target.pixelDetail, out mon.latitude, out mon.longitude);
					string str = string.Format ("street view triggerd monster model:{0}\tlatitude:{1}\tlongitude:{2} ", mon, mon.latitude, mon.longitude);
					Debug.Log (str);
					streetObj.GetComponent<GetImages> ().Lat = mon.latitude;
					streetObj.GetComponent<GetImages> ().Lon = mon.longitude;
//					streetObj.GetComponent<GetImages> ().Lat = 40d;
//					streetObj.GetComponent<GetImages> ().Lon = -80d;
					streetObj.SetActive (true);
					DisplayMonsterModel (mon);
					return;
				}
			}
		} else if (spot.tag == "weapon spot") {
			for (int i = 0; i < RadarBehavior.instance.weaponSpotList.Count; i++) {
				if (spot.Equals (RadarBehavior.instance.weaponSpotList [i])) {
					Weapon wea = (Weapon)GameManager.Instance.GetTargetMan ().weaponList [i];
//					TileSystem.PixelXYToLatLong (wea.pixelX, wea.pixelY, Target.pixelDetail, out wea.latitude, out mon.longitude);
//					string str = string.Format ("street view triggerd monster model:{0}\tlatitude:{1}\tlongitude:{2} ", mon, mon.latitude, mon.longitude);
//					Debug.Log (str);
					streetObj.GetComponent<GetImages> ().Lat = wea.latitude;
					streetObj.GetComponent<GetImages> ().Lon = wea.longitude;
//					streetObj.GetComponent<GetImages> ().Lat = 40d;
//					streetObj.GetComponent<GetImages> ().Lon = -80d;
					streetObj.SetActive (true);
					DisplayWeaponModel (wea);
					return;
				}
			}
		} else if (spot.tag == "item spot") {
			for (int i = 0; i < RadarBehavior.instance.itemSpotList.Count; i++) {
				if (spot.Equals (RadarBehavior.instance.itemSpotList [i])) {
					Item item = (Item)GameManager.Instance.GetTargetMan ().itemList [i];
					//					TileSystem.PixelXYToLatLong (wea.pixelX, wea.pixelY, Target.pixelDetail, out wea.latitude, out mon.longitude);
					//					string str = string.Format ("street view triggerd monster model:{0}\tlatitude:{1}\tlongitude:{2} ", mon, mon.latitude, mon.longitude);
					//					Debug.Log (str);
					streetObj.GetComponent<GetImages> ().Lat = item.latitude;
					streetObj.GetComponent<GetImages> ().Lon = item.longitude;
					//					streetObj.GetComponent<GetImages> ().Lat = 40d;
					//					streetObj.GetComponent<GetImages> ().Lon = -80d;
					streetObj.SetActive (true);
					DisplayItemModel (item);
					return;
				}
			}
		}
	}
	public void OnExit(){
		//wv.CloseWebView ();
		//HideMonsterModels();
		//HideWeaponModels ();
		DeleteModel();
		Combat_UIManager.Instance.ExitStreetView ();
	}
//	void OnGUI(){
//		GUI.Button (new Rect (0, 0, (int)(Screen.width*1.7f/3f), Screen.height), "WebPage");
//		if(curTarget!=null) curTarget.modelInRadar.SetActive (true);
//	}

	public void DisplayMonsterModel(Monster monster){
//		GameObject model = monsterModels[(int)monster.MonsterType];
//		model.SetActive (true);
//		model.GetComponent<Animator> ().SetBool ("walking", true);MonsterData md = MonsterData.GetMonsterData (monster.Type);
		MonsterData md = MonsterData.GetMonsterData (monster.Type);
		GameObject model = Resources.Load<GameObject>(md.StreetModel);
		GameObject clone = GameObject.Instantiate (model);
		curModel = clone;
//		ZombieCtrl zCtrl = clone.GetComponent<ZombieCtrl>();
//		Rigidbody rigidbody = clone.GetComponent<Rigidbody> ();
//		CharacterController cController = clone.GetComponent<CharacterController> ();
//		DestroyImmediate (zCtrl);
//		DestroyImmediate (rigidbody);
//		DestroyImmediate (cController);
//		Vector3 pos = clone.transform.position;
//		clone.transform.position = new Vector3 (pos.x, -1, pos.z);
	}
	public void DisplayWeaponModel(Weapon weapon){
//		GameObject model = weaponModels[(int)weapon.WeaponType];
//		model.SetActive (true);
		WeaponData data = WeaponData.GetWeaponData(weapon.Type);
		GameObject model = Resources.Load<GameObject>(data.PickUpModel);
		GameObject clone = GameObject.Instantiate (model);
		vp_ItemPickup pickup = clone.GetComponent<vp_ItemPickup> ();
		DestroyImmediate (pickup);
		curModel = clone;
	}
	public void DisplayItemModel(Item item){
//		GameObject model = weaponModels[(int)weapon.WeaponType];
//		model.SetActive (true);
		ItemData data = ItemData.GetDataFrom(item.Type);
		GameObject model = Resources.Load<GameObject>(data.PickUpModel);
		GameObject clone = GameObject.Instantiate (model);
		vp_ItemPickup pickup = clone.GetComponent<vp_ItemPickup> ();
		DestroyImmediate (pickup);
		curModel = clone;
	}
	public void DeleteModel(){
		GameObject.DestroyImmediate (curModel);
	}
	public void HideMonsterModels(){
//		for(int i = 0; i< monsterModels.Length; i++){
//			monsterModels [i].SetActive (false);
//		}
	}
	public void HideWeaponModels(){
//		for(int i = 0; i< weaponModels.Length; i++){
//			weaponModels [i].SetActive (false);
//		}
	}
}
