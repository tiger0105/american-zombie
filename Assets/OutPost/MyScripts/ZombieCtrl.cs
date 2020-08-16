using UnityEngine;
using System.Collections;
using Polaris.GameData;
using MLSpace;
using UnityEngine.AI;

public struct DamageHit{
	public RaycastHit hit;
	public float damage;
}	
[RequireComponent(typeof (Animator))]
public class ZombieCtrl : MonoBehaviour {
	public static int PreAtkingZombiCount = 0;
	public MonsterAI monAI;
	public Transform prefabBloodShoot;
	public GameObject nameTextObj;
	protected Transform target;
	protected float initSpeed; //also animation speed
	public vp_DamageHandler damageHandler;
	protected RagdollTest ragdollHandler;
	bool isDead = false;
	bool isAtking = false;
	bool iswalking = false;
	bool isPreAtking = false; // within 5m or not 
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
	public bool IsPreAtking{
		get{ return isPreAtking;}
		set{ isPreAtking = value;}
	}
	public bool IsHurting{
		get{ return isHurting;}
		set{isHurting =  value;}
	}
	public Transform createPoint; //assigned by the createPoint instance when the zombi is created
	protected Animator anim;

	public AudioClip zombieSound;
	public AudioClip zombieDieSound;

	protected Transform attackHand; //The hand of the zombie HAS A TRIGGER COLLIDER
	private AnimatorStateInfo currentBaseState;

	public int headShoot = 100;
	public int upperLegShoot = 10;
	public int LegShoot = 5;
	public int TorsoShoot = 20;

	protected Transform shootRayOrigin; //this would be the camera in a FPS

	//Animation states
	/*
	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int MOVEState = Animator.StringToHash("Base Layer.MOVE");
	static int DieState = Animator.StringToHash("Base Layer.Die");
	*/
	protected static int AttackState = Animator.StringToHash("Base Layer.Attack");
	protected string zombieName;
	protected Vector3 namePos;

    NavMeshAgent agent;

    public void SetZombieName(string name){
		//zombieName = name;
		if(GameSetting.nameOn == true) nameTextObj.GetComponent<TextMesh> ().text = name;
	}

	//initialization
	protected virtual void Start () 
	{
		//increate the speed of zombie as the game progresses
		int mission_index = MissionProgress.GetCurMissionType ();
		float mission_rate =(float) MissionProgress.GetMissionCurIndex (mission_index) / MissionData.GetMissionCount(mission_index);
		initSpeed = 3f + Random.Range(0f,7f) * mission_rate;

		anim = GetComponent<Animator>();					  
		anim.speed = initSpeed;

        agent = GetComponent<NavMeshAgent>();
			
		//attackHand = transform.Find("Hips/Spine/Spine1/Spine2/LeftArm/LeftForeArm/LeftHand");
		//attackHand = GameObject.FindWithTag("AttackHand").transform;

		GetComponent<AudioSource>().clip = zombieSound;
		if(GameManager.Instance.CurState != GameStates.PAUSE)	GetComponent<AudioSource>().Play();
			
		damageHandler = GetComponent<vp_DamageHandler> ();
		ragdollHandler = GetComponent<RagdollTest> ();
		Collider[] colliders = GetComponentsInChildren<Collider>();
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].isTrigger = false;
		}
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

	protected virtual void Update()
	{
		//Update approching sp
		UpdateSpeed();
		//Update Animation
		UpdateAnimation ();
        if (Time.timeScale == 0)
        {
            if (agent == null)
                return;

            agent.speed = 0;
            agent.angularSpeed = 0;
            agent.acceleration = 0;
        }
        else
        {
            if (agent == null)
                return; 

            agent.speed = 3.5f;
            agent.angularSpeed = 120;
            agent.acceleration = 8;
        }
	}
	//	void OnGUI(){
	//		namePos =	Camera.main.WorldToScreenPoint (nameTextObj.transform.position);
	//		GUI.skin.font = Font.CreateDynamicFontFromOSFont("Arial", 70);
	//		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
	//		GUI.Label (new Rect (namePos.x - 150, Screen.height - namePos.y + 50, 300, 100), zombieName);
	//	}
	public float normalY;
	protected virtual void UpdateSpeed(){

	}
	float openWayTime = 0f;
	protected virtual void UpdateAnimation(){
        if (Time.timeScale == 0)
            return;
		//check death
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
		//		//check  the forwad zombies exist or not. 
		//		if (Vector3.Distance (target.position, transform.position) > 5f) {
		//			iswalking = true;
		//			isAtking = false;
		//		}
		//		else if (Vector3.Distance (target.position, transform.position) > 1.3f) {
		//			Vector3 origin = Camera.main.transform.position;
		//			Vector3 direction = transform.position - origin;
		//			RaycastHit[] hits = Physics.RaycastAll (origin, direction, 11f); 
		//			int forwards = 0;
		//			foreach (RaycastHit hit in hits) {
		//				if (hit.transform.gameObject == gameObject) {
		//					continue;
		//				}
		//				if (hit.transform.gameObject.tag == "soldier")
		//					forwards++;
		//			}
		//			if (forwards > 0) {
		//				openWayTime = 0f;
		//				iswalking = false;
		//				isAtking = false;
		//			} else {
		//				openWayTime += Time.unscaledDeltaTime;
		//				if(openWayTime > 1f){
		//					iswalking = true;
		//					isAtking = false;
		//				}
		//			}
		//		} else {
		//			iswalking = false;
		//			isAtking = true;
		//		}
		//		if(transform.position.y > normalY + 2f && transform.position.x - target.transform.position.x < 2f && transform.position.z - target.transform.position.z < 2f ){
		//			Vector3 force_pos = new Vector3 (Random.Range(10f,20f), normalY, Random.Range(10f, 20f));
		//			transform.position = force_pos;
		//		}

		////////////**********zombie tatics************//////////
		target = GameManager.Instance.GetTargetMan().playerBase.transform;

		transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
		if(isDead == true ){
			//anim.SetBool ("die", true);
		}
		else if(isHurting == true){
			//Check death
			DoCheckDeath();
			anim.SetBool("walking",false);				
			anim.SetBool("attack",false);
			anim.SetBool ("hurt", true);
			isHurting = false;
		}
		else if (Vector3.Distance(target.position ,transform.position) > 10f)
		{
			anim.speed = initSpeed * 3;
			transform.Translate (0.06f * Vector3.forward, Space.Self);
			anim.SetBool("walking",true);								
			anim.SetBool("attack",false);
			anim.SetBool ("hurt", false);
			isHurting = false;
		}
		//		else
		//		{
		//			anim.speed = initSpeed;
		//			normalY = transform.position.y;
		//			//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
		//		}
		//
		else if (Vector3.Distance (target.position, transform.position) > 1.3f) {
			anim.speed = initSpeed;
			//normalY = transform.position.y;
			anim.SetBool("walking",true);								
			anim.SetBool("attack",false);
			anim.SetBool ("hurt", false);
			transform.Translate (0.02f * Vector3.forward, Space.Self);
		} else {
			anim.SetBool("walking",false);										
			anim.SetBool("attack",true);					
			anim.SetBool ("hurt", false);
		}
		//Update the animator Parameters

		//		//--fixing floating zombie over the player's head
		//		if (Vector3.Distance (target.position, transform.position) > 5f) {
		//			iswalking = true;
		//			isAtking = false;			
		//		} else if(Vector3.Distance (target.position, transform.position) > 1.5f) {
		//			if (isPreAtking)
		//				return;
		//			if (ZombieCtrl.PreAtkingZombiCount > 5) {
		//				iswalking = false;
		//				isAtking = false;
		//				isPreAtking = false;
		//			} else if(isPreAtking == false){
		//				ZombieCtrl.PreAtkingZombiCount++;
		//				isPreAtking = true;
		//				iswalking = true;
		//				isAtking = false;
		//			}
		//		}else {
		//			if (isAtking == false) {
		//				isAtking = true;
		//			}
		//		}


		//		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		//		if (currentBaseState.nameHash == AttackState)
		//		{
		//			attackHand.GetComponent<Collider>().enabled = true;	// activate the hand trigger to attack
		//		}
		//		else
		//		{
		//			attackHand.GetComponent<Collider>().enabled = false;	
		//		}

	}

	protected virtual void OnHitFromUFPS(DamageHit damagehit)
	{
        RaycastHit hit = damagehit.hit;
		float damage = damagehit.damage;
		Transform clone; // for the FX
		Debug.Log ("hit.transform.tag>>>>>>>>>>>>>>>" + hit.transform.gameObject.name);

		Debug.Log ("Hurted...............................................");
		//isHurting = true;
		float damage_amount;
        switch (hit.collider.transform.name)
        {
            case "Head":
                damage_amount = headShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeHeadShot", transform);
                break;
            case "Spine":
                damage_amount = TorsoShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            case "LeftUpLeg":
                damage_amount = upperLegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            case "LeftLeg":
                damage_amount = LegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            case "RightUpLeg":
                damage_amount = upperLegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            case "RightLeg":
                damage_amount = LegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            default:
                damage_amount = upperLegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
        }

		//			health healthComponent = GetComponent<health>();
		//			if (healthComponent)
		//			{
		//				healthComponent.SetHitDirection(-hit.normal);
		//				healthComponent.OnDamage(damage_amount * damage);
		//				healthComponent.SetLastHitTime();
		//				life = (int)healthComponent.GetHealth ();
		//				if (healthComponent.GetHealth() <= 0)
		//				{
		//					GetComponent<CharacterController>().enabled = false;
		//					foreach(Collider child in transform.GetComponentsInChildren<Collider>()){
		//						child.enabled = false;
		//					}
		//					//nameTextObj.SetActive (false);
		//		//score......
		//					GameObject.FindGameObjectWithTag ("CombatObserver").GetComponent<CombatObserver> ().SendMessage ("OnKillMonster");
		//					RespawnControl.Instance.SendMessage ("OnOneZombieDie", this, SendMessageOptions.DontRequireReceiver);
		////					RespawnSoldier respawer = GameObject.FindObjectOfType<RespawnSoldier>();
		////					if (respawer)
		////						respawer.OnDieSoldier(gameObject);
		//
		//				}
		//				else
		//				{
		//					;
		//				}
		//			}
		if (damageHandler!= null)
		{
			damageHandler.Damage (damage_amount * damage / 2);
		}
		shootRayOrigin = Camera.main.transform;
		clone = Instantiate (prefabBloodShoot, hit.point, shootRayOrigin.rotation) as Transform;
		clone.LookAt (shootRayOrigin.position);
		Destroy (clone.gameObject, 0.3f);
		//adapt ragdoll,,,,,,,,,,,
		if(ragdollHandler!=null){
			Vector3 dir = hit.point - Camera.main.transform.position;
			Ray ray = new Ray (Camera.main.transform.position, dir * 1000);
			ragdollHandler.CurrentRay = ray;
			if (damageHandler.CurrentHealth > 0)
				ragdollHandler.doHitReaction ();
			else {
				ragdollHandler.doRagdoll ();

			}
		}
	}
	protected virtual void DoCheckDeath(){
		if (damageHandler.CurrentHealth <= 0 && isDead == false)
		{
			isDead = true;
			//anim.SetBool ("die", true);
			GetComponent<CharacterController>().enabled = false;
			foreach(Collider child in transform.GetComponentsInChildren<Collider>()){
				//child.enabled = false;
			}
			if(nameTextObj)	nameTextObj.SetActive (false);
			//score......
			GameObject.FindGameObjectWithTag ("CombatObserver").GetComponent<CombatObserver> ().SendMessage ("OnKillMonster");
			RespawnControl.Instance.SendMessage ("OnOneZombieDie", this, SendMessageOptions.DontRequireReceiver);

			//					RespawnSoldier respawer = GameObject.FindObjectOfType<RespawnSoldier>();
			//					if (respawer)
			//						respawer.OnDieSoldier(gameObject);

			GetComponent<AudioSource>().clip = null;
			if(GameManager.Instance.CurState != GameStates.PAUSE)	GetComponent<AudioSource>().PlayOneShot(zombieDieSound);
			isDead = true;
			//GetComponent<Rigidbody>().isKinematic = true;

			//GameManager.Instance.ProcEventMessages (EventMessages.MONSTER_DEAD, gameObject);
			//createPoint.GetComponent<ZombieBirthPointCtrl>().myZombieIsDead = true; // Then a new zombie will born if you're not near the born point
			//createPoint = null;
		}
		//		else if (!isDead)
		//		{
		//			
		//		}
	}
}