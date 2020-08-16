using UnityEngine;
using System.Collections;
using Polaris.GameData;
public class BehemothControl : ZombieBehaviour {

	bool isDead = false;
	bool isAtking = false;
	bool iswalking = false;
	bool isHurting = false;

	public bool IsDead{
		get{ return isDead;}
		set{ isDead = value;}
	}

	public bool IsAtking{
		get{ return isAtking;}
		set{ isAtking = value;}
	}
	public bool Iswalking{
		get{ return iswalking;}
		set{ iswalking = value;}
	}
	public bool IsHurting{
		get{ return isHurting;}
		set{isHurting =  value;}
	}

	protected override void Start () 
	{
		//increate the speed of zombie as the game progresses
		int mission_index = MissionProgress.GetCurMissionType ();
		float mission_rate =(float) MissionProgress.GetMissionCurIndex (mission_index) / MissionData.GetMissionCount(mission_index);
		initSpeed = 3f + Random.Range(0f,7f) * mission_rate;

		//target = GameManager.Instance.GetTargetMan().playerBase.transform;
		anim = GetComponent<Animator>();					  
		anim.speed = initSpeed;

		//attackHand = transform.Find("Hips/Spine/Spine1/Spine2/LeftArm/LeftForeArm/LeftHand");
		//attackHand = GameObject.FindWithTag("AttackHand").transform;

		GetComponent<AudioSource>().clip = zombieSound;
		if(GameManager.Instance.CurState != GameStates.PAUSE)	GetComponent<AudioSource>().Play();
		ragdollHandler = GetComponent<RagdollTest> ();
		damageHandler = GetComponent<vp_DamageHandler> ();
		//		#if UNITY_EDITOR
		//		SetZombieName ("Zombie");
		//		#elif UNITY_ANDROID
		//		if(MobileContactManager.IsReady == false){
		//		SetZombieName ("Zombie");
		//		}else{
		//		int contact_count = MobileContactManager.GetContactCount ();
		//		Contact contact = MobileContactManager.GetContactInfo (Random.Range (0, contact_count));
		//		SetZombieName (contact.Name);				
		//		}
		//		#elif UNITY_IOS
		//		if(MobileContactManager.IsReady == false){
		//		SetZombieName ("Zombie");
		//		}else{
		//		int contact_count = MobileContactManager.GetContactCount ();
		//		Contact contact = MobileContactManager.GetContactInfo (Random.Range (0, contact_count));
		//		SetZombieName (contact.Name);				
		//		}
		//		#endif

	}	

	protected override void UpdateAnimation(){
		//chceck death
		DoCheckDeath();
		//Check Game is play/pause
		if(GameManager.Instance.CurState == GameStates.PAUSE){
			if(anim.speed >0) anim.speed = 0f;
			return;
		}

		if(GameManager.Instance.CurState == GameStates.PLAY)
			anim.speed = initSpeed;

		if(GameManager.Instance.CurState == GameStates.SLOWDOWN)
			anim.speed = initSpeed / GameManager.Instance.SLOWDOWN_RATE;
		
		////////////**********zombie tatics************//////////
		SetTarget();
		transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
		if (IsDead == true) {
			anim.SetBool ("die", true);
			anim.SetBool ("walk", false);
			anim.SetBool ("attack", false);
			anim.SetBool ("hurt", false);
			anim.SetBool ("idle", false);
			int die_type = Random.Range (0, 3);
			anim.SetInteger ("die_type", die_type);
		} else if (IsHurting == true) {
			//Check death
			DoCheckDeath ();
			anim.SetBool ("die", false);
			anim.SetBool ("walk", false);
			anim.SetBool ("attack", false);
			anim.SetBool ("idle", false);
			anim.SetBool ("hurt", true);
			int hurt_type = Random.Range (0, 2);
			anim.SetInteger ("hurt_type", hurt_type);
			//for not repeaing hurt animation
			IsHurting = false;
		} else if (isFreezed) {
			anim.speed = 0;
			return;
		} else if (Vector3.Distance(target.position ,transform.position) > 10f)
		{
			anim.speed = initSpeed * 5;
			transform.Translate (0.1f * Vector3.forward, Space.Self);
			anim.SetBool ("die", false);
			anim.SetBool ("walk", true);
			anim.SetBool ("attack", false);
			anim.SetBool ("hurt", false);
			anim.SetBool ("idle", false);
		}
		else if (Vector3.Distance (target.position, transform.position) > 2f) {
			anim.speed = initSpeed;
			transform.Translate (0.02f * Vector3.forward, Space.Self);
			anim.SetBool ("die", false);
			anim.SetBool ("walk", true);
			anim.SetBool ("attack", false);
			anim.SetBool ("hurt", false);
			anim.SetBool ("idle", false);
		} else {
			anim.SetBool ("die", false);
			anim.SetBool ("walk", false);
			anim.SetBool ("attack", true);
			anim.SetBool ("hurt", false);
			anim.SetBool ("idle", false);
			int attack_type = Random.Range (0, 2);
			anim.SetInteger ("attack_type", attack_type);
		}
	}
	protected override void OnHitFromUFPS(DamageHit damagehit)
	{
		RaycastHit hit = damagehit.hit;
		float damage = damagehit.damage;
		Transform clone; // for the FX
		Debug.Log ("hit.transform.tag>>>>>>>>>>>>>>>" + hit.transform.gameObject.name);

		Debug.Log ("Hurted...............................................");
		IsHurting = true;
		float damage_amount;
		switch (hit.collider.transform.name) {
		case "CanisterL": 
		case "CanisterR": 
			damage_amount = headShoot; 
			GameObject.FindGameObjectWithTag ("CombatObserver").GetComponent<CombatObserver> ().SendMessage ("OnTakeHeadShot", transform);
			ParticleAnimator[] particleAnims = hit.collider.gameObject.GetComponentsInChildren<ParticleAnimator> ();
			for (int i = 0; i < particleAnims.Length; i++) {
				particleAnims [i].GetComponent<ParticleRenderer> ().enabled = true;
			}
			break;
//		case "Spine":
//			damage_amount = TorsoShoot;
//			break;
//		case "LeftUpLeg":
//			damage_amount = upperLegShoot;
//			break;
//		case "LeftLeg":
//			damage_amount = LegShoot;
//			break;
//		case "RightUpLeg":
//			damage_amount = upperLegShoot;
//			break;					
//		case "RightLeg":
//			damage_amount = LegShoot;
//			break;
		default:
			damage_amount = upperLegShoot;
			break;
		}		
		if (damageHandler!= null)
		{
			damageHandler.Damage (damage_amount * damage);
		}
		shootRayOrigin = Camera.main.transform;
		clone = Instantiate (prefabBloodShoot, hit.point, shootRayOrigin.rotation) as Transform;
		clone.LookAt (shootRayOrigin.position);
		Destroy (clone.gameObject, 0.3f);
	}
}