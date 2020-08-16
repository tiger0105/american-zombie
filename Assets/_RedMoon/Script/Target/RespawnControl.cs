using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Polaris.GameData;
public class RespawnControl : MonoBehaviour {
	public static RespawnControl Instance;
	[HideInInspector] public List<ZombieBehaviour> playingZombieList;
	private List<MonsterAI> readyZombieList;
	public int m_zombieLimits;
	public float m_killDuration = 5.5f;
	bool isBusy;
	// Use this for initialization
	void Awake () {
		Instance = this;
		int mission_index = MissionProgress.GetCurMissionType ();
		float mission_rate =(float) MissionProgress.GetMissionCurIndex (mission_index) / MissionData.GetMissionCount(mission_index);
		//m_zombieLimits =(int)( 10 + 40 * mission_rate);
		m_zombieLimits = 5;
	}
	/// <summary>
	/// Raises the one zombie die event.
	/// </summary>
	/// <param name="zombie">Zombie.</param>
	void OnOneZombieDie(ZombieBehaviour zombie){
		// StartCoroutine (DoKillZombie(zombie, m_killDuration));
		DoKillZombie(zombie);
	}
	/// <summary>
	/// Zombie is waiting for spawn order. 
	/// </summary>
	/// <param name="zombie">Zombie.</param>
	void OnOneZombieReady(MonsterAI zombie){
		StartCoroutine (TryJoinPlay(zombie));
	}
	IEnumerator TryJoinPlay(MonsterAI zombie){
		if(readyZombieList == null){
			readyZombieList = new List<MonsterAI> ();
		}
		while (isBusy) {
			yield return new WaitForFixedUpdate();
			//Debug.Log ("yield for busying..........");
		}
		//Debug.Log ("exit yield for busying..........");
		isBusy = true;
		readyZombieList.Add (zombie);
		TryRespawnZombies ();	
	}
	/// <summary>
	/// when zombie is die or ready to spawn
	/// </summary>
	void TryRespawnZombies(){
		isBusy = true;
		if(playingZombieList == null){
			playingZombieList = new List<ZombieBehaviour> ();
		}
//		if(!playingZombieList.Count.Equals(0)){
//			//after all playing zombies killed,then use this func!
//			return;
//		}
		if(playingZombieList.Count > m_zombieLimits){
			//prevent over respawning
			return;
		}
		int join_count = m_zombieLimits - playingZombieList.Count;
		int ready_count = readyZombieList.Count;
		int valid_count = Mathf.Min (join_count, ready_count);
		for(int i = 0; i<valid_count; i++){
			MonsterAI readyZombie = readyZombieList[i];
			readyZombie.IsPlaying = true;
			MonsterData md = MonsterData.GetMonsterData (readyZombie.gameObject.name);
			GameObject prefab = Resources.Load<GameObject> (md.StreetModel);
			GameObject clone = GameObject.Instantiate (prefab);
			clone.layer = 0;
			foreach (Transform child in clone.transform) {
				child.gameObject.layer = 0;//default layer
			}
			clone.name = readyZombie.gameObject.name;
			clone.transform.position = new Vector3(readyZombie.transform.position.x, 1.5f, readyZombie.transform.position.z);

			if (clone.GetComponent<ZombieBehaviour>() != null)
			{
				ZombieBehaviour zombie = clone.GetComponent<ZombieBehaviour>();	
				zombie.monAI = readyZombie;
				//zombie.GetComponent<vp_DamageHandler> ().MaxHealth = md.Hp;
				zombie.GetComponent<vp_DamageHandler> ().CurrentHealth = zombie.GetComponent<vp_DamageHandler>().MaxHealth;
				if(zombie != null) playingZombieList.Add (zombie);
			}
		}
		readyZombieList.RemoveRange (0, valid_count);
		isBusy = false;
	}
	IEnumerator DoKillZombie(ZombieBehaviour zombie, float duration){
		GameObject obj = zombie.gameObject;
		//yield return new WaitForSeconds (duration);
		yield return new WaitForSeconds (zombie.gameObject.GetComponent<vp_DamageHandler>().MaxDeathDelay);
		playingZombieList.Remove (zombie);
		zombie.monAI.IsDead = true;
		if (zombie.IsPreAtking == true) {
			ZombieBehaviour.PreAtkingZombiCount--;
		}
		if(obj != null)	GameObject.DestroyImmediate(zombie.gameObject);
		TryRespawnZombies ();
	}
	void DoKillZombie(ZombieBehaviour zombie){
		GameObject obj = zombie.gameObject;
		playingZombieList.Remove (zombie);
		zombie.monAI.IsDead = true;
		if (zombie.IsPreAtking == true) {
			ZombieBehaviour.PreAtkingZombiCount--;
		}
		TryRespawnZombies ();
	}
	//Generate weapon target in Supply Run missions
	void OnOneWeaponFound(Weapon wp){
		float deltaX = wp.pixelX - TargetManager.Instance.player.pixelX;
		float deltay = wp.pixelY - TargetManager.Instance.player.pixelY;
		WeaponData wd = WeaponData.GetWeaponData (wp.Type);
		GameObject obj = Resources.Load<GameObject> (wd.PickUpModel);
		GameObject clone = Instantiate(obj);
		string str = string.Format ("w{0}", wd.id-1);//0,1,2 ordering
		clone.name = str;
		clone.transform.position = new Vector3(deltaX, 5f, deltay);
		wp.playObject = clone;
		wp.IsPlaying = true;
	}
	//Generate item target in Supply Run missions
	void OnOneItemFound(Item im){
		float deltaX = im.pixelX - TargetManager.Instance.player.pixelX;
		float deltay = im.pixelY - TargetManager.Instance.player.pixelY;
		ItemData idat = ItemData.GetDataFrom(im.Type);
		GameObject obj = Resources.Load<GameObject> (idat.PickUpModel);
		GameObject clone = Instantiate(obj);
		string str = string.Format ("i{0}", idat.id -1);
		clone.name = str;
		clone.transform.position = new Vector3(deltaX, 5f, deltay);
		im.playObject = clone;
		im.IsPlaying = true;
	}
}
