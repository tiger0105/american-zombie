using UnityEngine;
using System.Collections;
using Polaris.GameData;

public class MonsterAI : MonoBehaviour {
	Transform player;
	public float speed = 5f; //speed = 10m/s
	public float respawnRange = 70f;

	public bool IsRespawnReady{
		get{ return isRespawnReady;}
		set { isRespawnReady = value; }
	}
	public bool IsPlaying{
		get{ return isPlaying;}
		set{ isPlaying = value;}
	}
	public bool IsDead{
		get{ return isDead;}
		set{ isDead = value;}
	}

	bool isRespawnReady;
	bool isPlaying;
	bool isDead;
	// Use this for initialization
	void Start(){
        MissionData data =MissionProgress.GetCurMissionData();
        if (data.Type == "Time Limit")
        {
            respawnRange = 20;
        }

        isRespawnReady = false;
		//player = GameObject.FindWithTag ("Player");
		player = Camera.main.transform;
		float dis = Vector3.Distance (player.position, transform.position); 
		if (dis*Target.groundResoulution < Target.min_range_radius) {
			//this will prevent the zombies floating right over your head ha ha ha.....................
			transform.position = (transform.position.normalized * (float)Target.min_range_radius/Target.groundResoulution);
		}

	}
	// Update is called once per frame
	void Update(){
		DoUpdate ();
	}

	void DoUpdate () {
		if (player == null)
			return;
		if (isRespawnReady == true)
			return;
		if (Vector3.Distance(player.position, transform.position) < respawnRange) {
			isRespawnReady = true;
			RespawnControl.Instance.SendMessage ("OnOneZombieReady", this, SendMessageOptions.DontRequireReceiver);
			return;	
		} else {
			transform.LookAt (player.position);
			float pixelSpeed = speed / Target.groundResoulution;
			pixelSpeed = pixelSpeed*Time.deltaTime;
			//Debug.Log("pixelspeed>>>>>>>>>>>>>>>>>>>::::::::::::::::::>>>>>>>>>>" + pixelSpeed);
			transform.Translate (pixelSpeed * Vector3.forward, Space.Self);
		}
	}		
}