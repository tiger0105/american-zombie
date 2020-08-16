
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using LonelySharp;
using Microsoft.MapPoint;
using Polaris.GameData;

public enum TargetKinds{
	MONSTER = 0,
	WEAPON,
	OTHER
}
//public enum MonsterTypes{
//	BLACKDOG = 0,
//	GHOST,
//	VAMPIRE,
//	WEREWOLF,
//	WITCH,
//	ZOMBIE,
//	COUNT
//}
//public enum WeaponTypes{
//	PISTOL = 0,
//	SHOTGUN,
//	AUTORIFLE,
//	FLAMETHROWER,
//	PLAZMAGUN,
//	MOLOTOV_COCKTAIL,
//	TORCH,
//	COUNT
//}
//public enum ItemTypes{
//	AMMOBOX = 0,
//	FLOODLIGHT,
//	COINBAG,
//	COUNT
//}
public	class Target{
	public static double max_range_radius = 150d; //150m; 
	public static double min_range_radius = 75d; //50m;   
	public double latitude;
	public	double longitude;
	public float distance;
	public float angle;
	public int pixelX;
	public int pixelY;
	public GameObject virtualObject;//object in global pixel coordinate 
	public GameObject realObject;//object in relative coordinate to player virtual object
	public GameObject playObject;
	/// <summary>
	/// The pixel detail. pixel is not ordinary pixel, that means tile.....
	/// </summary>
	public static int pixelDetail = 16;
	public static float groundResoulution; //0.592m / pixel
	public static int maxDeltaPixelX;
	public static int maxDeltaPixelY;
	public static int map_width;
	public string Type{
		get{ return type; }
		set{ type = value; }
	}
	public bool IsFound{
		get{ return isFound; }
		set{ isFound = value;}
	}
	public bool IsDisappeared{
		get{ return isDisappeared;}
		set{ isDisappeared = value;}
	}
	public bool IsPlaying{
		get{ return isPlaying;}
		set{ isPlaying = value;}
	}
	bool isPlaying = false;
	bool isDisappeared = false;
	bool isFound = false;
	string type;
}
public class Monster:Target{
//	MonsterTypes monsterType;
//	public MonsterTypes MonsterType{
//		get{ return monsterType;}
//		set{ monsterType = value;}
//	}

}
public class Weapon:Target{
//	WeaponTypes weaponType;
//	public WeaponTypes WeaponType{
//		get{ return weaponType; }
//		set{ weaponType = value;}
//	}
//	int id;
//	string name;
//	float power;
//	float price;
//	float fireRate;
//	float damageAmount;
//	string describe;
}
public class Item:Target{
//	ItemTypes itemType;
//	public ItemTypes ItemType{
//		get{ return itemType; }
//		set{ itemType = value;}
//	}
//	int id;
//	string name;
}
//public enum MonsterActStates{
//	NONE,
//	IDLE,
//	WALK,
//	ATTACK,
//	FALL
//}
public enum TargetMessages{
	TARGET_READAY,
	TARGET_PLAYING,
	TARGET_DISAPPEARED
}
public class TargetManager : MonoBehaviour
{
	public static TargetManager Instance;
	public GameObject playerBase;
	public GameObject playMap;
	public GameObject earthMap;
	//public ArrayList targetList;

	public ArrayList monsterList;
	public ArrayList weaponList;
	public ArrayList itemList;

	public Target player;

	public Text ModelLocation;
	public Text CurrentLocation;

	Vector3 playerInitPos;
	bool isTargetGenerated;
	/// <summary>
	/// ///////////////////public elements
	/// </summary>
	/// <value><c>true</c> if this instance is weapon appeared; otherwise, <c>false</c>.</value>
	public bool IsTargetGenerated
	{
		get { return isTargetGenerated; }
		set { isTargetGenerated = value; }
	}
	public void OnMonsterDead(GameObject monster)
	{
		for (int i = 0; i < monsterList.Count; i++)
		{
			Monster mon = (Monster)monsterList[i];

			if (mon.realObject.Equals(monster))
				mon.IsDisappeared = true;
		}
	}
	public void UpdatePlayerGeoLocation()
	{
		if (player == null)
			player = new Target();

		player.latitude = GameInfo.curMobileLatitude;
		player.longitude = GameInfo.curMobileLongitude;

		TileSystem.LatLongToPixelXY(player.latitude, player.longitude, Target.pixelDetail, out player.pixelX, out player.pixelY);

		if (player.virtualObject == null)
		{
			GameObject obj = new GameObject();
			obj.name = "player";
			obj.transform.parent = earthMap.transform;
			obj.transform.position = new Vector3(player.pixelX, 0, player.pixelY);
			//playerInitPos = obj.transform.position;
			player.virtualObject = obj;
			//playerBase.SetActive (true);
			player.realObject = playerBase;
			GenerateTargets();
			//GenerateWeapons ();
		}
		else
		{
			Vector3 velocity = Vector3.zero;
			player.virtualObject.transform.position = new Vector3(player.pixelX, 0, player.pixelY);
			//playerBase.transform.position = Vector3.SmoothDamp (playerBase.transform.position, new Vector3(player.pixelX - playerInitPos.x, playerBase.transform.position.y, player.pixelY - playerInitPos.z), ref velocity, 0.3f);
		}
	}
	public void ProcItemMessage(TargetMessages msg, GameObject param)
	{
		switch (msg)
		{
			case TargetMessages.TARGET_DISAPPEARED:
				for (int i = 0; i < itemList.Count; i++)
				{
					Item im = (Item)itemList[i];
					if (im.playObject.Equals(param))
					{
						im.IsDisappeared = true;
						return;
					}
				}
				for (int i = 0; i < weaponList.Count; i++)
				{
					Weapon wp = (Weapon)weaponList[i];
					if (wp.playObject.Equals(param))
					{
						wp.IsDisappeared = true;
						return;
					}
				}
				break;
			default:
				break;
		}
	}

	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		UpdatePlayerGeoLocation();
	}
	void GenerateTargets()
	{
		/////////////******get boundingbox with max_range_radius
		Target.groundResoulution = (float)TileSystem.GroundResolution(GameInfo.curMobileLatitude, Target.pixelDetail);
		//Debug.Log ("resolutionnnnnnnnnnnnnnnnnnnnnn " + Target.groundResoulution);
		MapPoint mp = new MapPoint();
		mp.Latitude = GameInfo.curMobileLatitude;
		mp.Longitude = GameInfo.curMobileLongitude;

		// monster
		MapPoint.BoundingBox bb = MapPoint.GetBoundingBox(mp, 40d / 1000d);
		int minPixelX, minPixelY, maxPixelX, maxPixelY;
		TileSystem.LatLongToPixelXY(bb.MinPoint.Latitude, bb.MinPoint.Longitude, Target.pixelDetail, out minPixelX, out minPixelY);
		TileSystem.LatLongToPixelXY(bb.MaxPoint.Latitude, bb.MaxPoint.Longitude, Target.pixelDetail, out maxPixelX, out maxPixelY);

		//string str = string.Format ("minLat:{0}  maxLat:{2} minLog:{1} maxLog:{3} ", bb.MinPoint.Latitude, bb.MinPoint.Longitude, bb.MaxPoint.Latitude, bb.MaxPoint.Longitude);
		//Debug.Log (str);
		MissionData md = MissionProgress.GetCurMissionData();

		////////////////////////////////////////////////////////////////////generate monsters on the earthmap round the player 
		if (monsterList != null)
			monsterList.Clear();
		monsterList = new ArrayList();
		//		for(int i = 0; i< (int)MonsterTypes.COUNT; i++){
		//			int count = md.GetMonsterCountList()[i];
		//			for(int j = 0; j < count; j++){
		//				AppendToMonsterList ((MonsterTypes)i, minPixelX, minPixelY, maxPixelX, maxPixelY);
		//			}
		//		}
		for (int i = 0; i < md.MonsterIdList.Count; i++)
		{
			string type = md.MonsterIdList[i];
			int count = md.MonsterCountList[i];
			for (int j = 0; j < count; j++)
			{
				AppendToMonsterList(type, minPixelX, minPixelY, maxPixelX, maxPixelY);
			}
		}
		//////////////////////////////////////////////////////////////////generate weapons
		bb = MapPoint.GetBoundingBox(mp, 40d / 1000d);
		TileSystem.LatLongToPixelXY(bb.MinPoint.Latitude, bb.MinPoint.Longitude, Target.pixelDetail, out minPixelX, out minPixelY);
		TileSystem.LatLongToPixelXY(bb.MaxPoint.Latitude, bb.MaxPoint.Longitude, Target.pixelDetail, out maxPixelX, out maxPixelY);

		if (weaponList != null)
			weaponList.Clear();

		weaponList = new ArrayList();
		//		for(int i = 0; i<md.GetWeaponCountList ().Count; i++){
		//			int count = md.GetWeaponCountList()[i];
		//			for(int j = 0; j < count; j++){
		//				AppendToWeaponList ((WeaponTypes)i,minPixelX, minPixelY, maxPixelX, maxPixelY);
		//			}
		//		}
		for (int i = 0; i < md.WeaponIdList.Count; i++)
		{
			string type = md.WeaponIdList[i];
			int count = md.WeaponCountList[i];
			for (int j = 0; j < count; j++)
			{
				AppendToWeaponList(type, minPixelX, minPixelY, maxPixelX, maxPixelY);
			}
		}

		//////////////////////////////////////////////////////////////////generate items
		if (itemList != null)
			itemList.Clear();

		itemList = new ArrayList();
		//		for(int i = 0; i<md.GetItemCountList ().Count; i++){
		//			int count = md.GetItemCountList()[i];
		//			for(int j = 0; j < count; j++){
		//				AppendToItemList ((ItemTypes)i,minPixelX, minPixelY, maxPixelX, maxPixelY);
		//			}
		//		}
		for (int i = 0; i < md.ItemIdList.Count; i++)
		{
			string type = md.ItemIdList[i];
			int count = md.ItemCountList[i];
			for (int j = 0; j < count; j++)
			{
				AppendToItemList(type, minPixelX, minPixelY, maxPixelX, maxPixelY);
			}
		}
		///////finally run the radar
		isTargetGenerated = true;
	}

	void AppendToMonsterList(string mon_type, int minPixelX, int minPixelY, int maxPixelX, int maxPixelY)
	{
		//Debug.Log ("mobilat" + myMobileGeoLoaction.getLatitudeInDegrees() + "mobilon" + myMobileGeoLoaction.getLongitudeInDegrees());
		//string str = string.Format ("minLat:{0}  maxLat:{2} minLog:{1} maxLog:{3} ", boundingCoords[0].getLatitudeInDegrees(), boundingCoords[0].getLongitudeInDegrees(), boundingCoords[1].getLatitudeInDegrees(), boundingCoords[1].getLongitudeInDegrees());

		Monster monster = new Monster();
		monster.Type = mon_type;
		monster.pixelX = UnityEngine.Random.Range(minPixelX, maxPixelX);
		monster.pixelY = UnityEngine.Random.Range(minPixelY, maxPixelY);
		GameObject obj = new GameObject();
		obj.name = mon_type.ToString();
		obj.tag = "soldier";

		obj.transform.position = new Vector3(monster.pixelX, 0, monster.pixelY);
		obj.transform.parent = earthMap.transform;
		//obj.AddComponent<MonsterAI> ().ApplyAI(player.virtualObject);
		monster.virtualObject = obj;

		//GameObject realObj = GameObject.Instantiate (monsterPrefabs[(int)mon_type]);
		GameObject realObj = new GameObject();
		realObj.SetActive(true);
		realObj.name = mon_type.ToString();
		//realObj.tag = mon_type.ToString ();
		realObj.tag = "Monster";
		//		Debug.Log (monster.virtualObject.transform.position);
		//		Debug.Log (player.virtualObject.transform.position);
		//		Debug.Log("monsterprefab y " + monsterPrefabs[(int)mon_type].transform.position.y);
		realObj.transform.position = new Vector3(monster.virtualObject.transform.position.x - player.virtualObject.transform.position.x,
			//monsterPrefabs[(int)mon_type].transform.position.y,
			player.realObject.transform.position.y + 5,
			monster.virtualObject.transform.position.z - player.virtualObject.transform.position.z);
		realObj.transform.parent = playMap.transform;
		realObj.transform.LookAt(player.realObject.transform.position);
		realObj.AddComponent<MonsterAI>();
		////////////////can't understand??????????????????????
		//realObj.GetComponent<Animator> ().SetInteger ("ActState", (int)MonsterActStates.ATTACK);
		monster.realObject = realObj;
		monsterList.Add(monster);
	}

	void AppendToWeaponList(string wea_type, int minPixelX, int minPixelY, int maxPixelX, int maxPixelY)
	{
		Weapon weapon = new Weapon();
		weapon.Type = wea_type;
		weapon.pixelX = UnityEngine.Random.Range(minPixelX, maxPixelX);
		weapon.pixelY = UnityEngine.Random.Range(minPixelY, maxPixelY);

		float deltaX = weapon.pixelX - player.pixelX;
		float deltay = weapon.pixelY - player.pixelY;
		WeaponData wd = WeaponData.GetWeaponData(weapon.Type);
		GameObject obj = Resources.Load<GameObject>(wd.PickUpModel);
		GameObject clone = Instantiate(obj);
		string str = string.Format("w{0}", wd.id - 1);//0,1,2 ordering
		clone.name = str;
		clone.transform.position = new Vector3(deltaX, 4f, deltay);
		weapon.playObject = clone;
		weapon.IsPlaying = true;

		weaponList.Add(weapon);
	}
	void AppendToItemList(string ite_type, int minPixelX, int minPixelY, int maxPixelX, int maxPixelY)
	{
		Item item = new Item();
		item.Type = ite_type;
		item.pixelX = UnityEngine.Random.Range(minPixelX, maxPixelX);
		item.pixelY = UnityEngine.Random.Range(minPixelY, maxPixelY);

		float deltaX = item.pixelX - player.pixelX;
		float deltay = item.pixelY - player.pixelY;
		ItemData idat = ItemData.GetDataFrom(item.Type);
		GameObject obj = Resources.Load<GameObject>(idat.PickUpModel);
		GameObject clone = Instantiate(obj);
		string str = string.Format("i{0}", idat.id);
		clone.name = str;
		clone.transform.position = new Vector3(deltaX, 4f, deltay);
		item.playObject = clone;
		item.IsPlaying = true;

		itemList.Add(item);
	}
}