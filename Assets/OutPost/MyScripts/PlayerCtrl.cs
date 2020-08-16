
using UnityEngine;
using System.Collections;
using Polaris.GameData;
using MLSpace;
public class PlayerCtrl : MonoBehaviour {
	public vp_FPPlayerDamageHandler playerDamageHandler;
	public GameObject weaponCamera;
	public float life = 1f;
	public Transform blood; //PNG FX prefabs
	public Transform endBlood; 
	public Material handMat;

	Transform endBloodTran;
	public CombatObserver observer;

    public AudioSource srcAudio;
	// Use this for initialization
	void Start () 
	{
		float handColor = (float)(2 - GameSetting.handShade) / 2f;
		handMat.color = new Color (handColor, handColor, handColor);

        if (GameSetting.soundOn == false)
        {
            AudioSource[] sources = GetComponentsInChildren<AudioSource>();
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i].mute = true;
            }
            GetComponent<AudioSource>().mute = true;
            if (srcAudio != null)
                srcAudio.mute = true;
        }
        else
        {
            AudioSource[] sources = GetComponentsInChildren<AudioSource>();
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i].mute = false;
            }
            GetComponent<AudioSource>().mute = false;
            if (srcAudio != null)
                srcAudio.mute = false;
        }
    }

	void Injured(float recv_damage)
	{		
		if (playerDamageHandler) {
			playerDamageHandler.Damage (recv_damage / 10);
			//observer.SendMessage ("OnGetHurt", recv_damage*10f);
			//Instantiate(blood);
		}

//		if (life == 0)
//		{
//			if (endBloodTran == null) {
//				//endBloodTran = Instantiate (endBlood);
//			} else {
//				return;
//			}
//
//		}
//		else if (life>0)
//		{
//			life = life-20;
//			//Instantiate(blood);	
//		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.name.Contains ( "AttackPart") )
		{
			MonsterData md = MonsterData.GetMonsterData (other.gameObject.tag);
            if (md != null)
			    Injured(md.Attack);
		}
		if (other.gameObject.GetComponent<BodyColliderScript>()!=null )
		{
			MonsterData md = MonsterData.GetMonsterData (other.GetComponent<BodyColliderScript>().ParentObject.name);
            if (md != null)
			    Injured(md.Attack);
		}
	}
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<BodyColliderScript>()!=null )
		{
			MonsterData md = MonsterData.GetMonsterData (other.gameObject.tag);
			Injured(md.Attack);
		}
	}

//	void Shoot()
//	{
//		Transform clone2 = Instantiate(prefabShoot, shootOrigin.transform.position, shootOrigin.transform.rotation) as Transform;
//		clone2.LookAt(shootRayOrigin.position);
//		clone2.RotateAround(clone2.position,clone2.forward,Random.Range(0,360));
//
//		RaycastHit hitInfo;
//		if (Physics.Raycast (shootRayOrigin.position, shootRayOrigin.forward, out hitInfo)) {
//			Transform clone; // for the FX
//			Debug.Log ("hitinfo.transform.tag>>>>>>>>>>>>>>>" + hitInfo.transform.gameObject.name);
//			if (hitInfo.transform.tag == "Monster") {
//				
//				ZombieCtrl ScriptAccess = hitInfo.transform.GetComponent<ZombieCtrl> ();
//				switch (hitInfo.collider.transform.name) {
//				case "Head": 		
//					ScriptAccess.life -= ScriptAccess.headShoot; 
//					break;
//				case "Spine":
//					ScriptAccess.life -= ScriptAccess.TorsoShoot;
//					break;
//				case "LeftUpLeg":
//					ScriptAccess.life -= ScriptAccess.upperLegShoot;
//					break;
//				case "LeftLeg":
//					ScriptAccess.life -= ScriptAccess.LegShoot;
//					break;
//				case "RightUpLeg":
//					ScriptAccess.life -= ScriptAccess.upperLegShoot;
//					break;					
//				case "RightLeg":
//					ScriptAccess.life -= ScriptAccess.LegShoot;
//					break;
//				default:
//					ScriptAccess.life -= ScriptAccess.LegShoot;
//					break;
//				}		
//					
//				clone = Instantiate (prefabBloodShoot, hitInfo.point, shootRayOrigin.rotation) as Transform;
//				clone.LookAt (shootRayOrigin.position);
//				Destroy (clone.gameObject, 0.3f);
//			} else {
//				clone = Instantiate (prefabDustShoot, hitInfo.point, shootRayOrigin.rotation) as Transform;
//				clone.LookAt (shootRayOrigin.position);
//				Destroy (clone.gameObject, 0.3f);
//			}	
//		} else {
//			Debug.Log ("raycast fail");
//		}
//
//		Destroy (clone2.gameObject, 0.03f);			
//		GetComponent<AudioSource>().PlayOneShot(weaponShoot,1);
//	}

//	public void StartShoot(){
//		curGun.GetComponent<GunController> ().SetGunState (GunStates.Shoot);
//	}
//	public void EndShoot(){
//		curGun.GetComponent<GunController> ().SetGunState (GunStates.Idle);
//	}

	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnGUI()
	{
		if(endBloodTran == null){
			return;
		}
		if (GUI.Button(new Rect(Screen.width/2 - 150, Screen.height/2 - 40, 300, 80),"Restart"))
		{
			GameManager.Instance.ProcEventMessages (EventMessages.ENTER_SMALL_RADAR_WINDOW);
			Application.LoadLevel(Application.loadedLevel);
		}
		//GUI.DrawTexture(new Rect((Screen.width/2)-(sightTexture.width*sightSize/2),(Screen.height/2)-(sightTexture.height*sightSize/2), sightTexture.width*sightSize, sightTexture.height*sightSize), sightTexture, ScaleMode.ScaleToFit);
	}
		
}