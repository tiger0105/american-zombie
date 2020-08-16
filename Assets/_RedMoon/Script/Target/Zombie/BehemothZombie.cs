using UnityEngine;
using System.Collections;
using Polaris.GameData;

public class BehemothZombie : ZombieBehaviour {
	
	protected override void Start () 
	{
		//increate the speed of zombie as the game progresses
		missionIndex = MissionProgress.GetCurMissionType ();
		float mission_rate =(float) MissionProgress.GetMissionCurIndex (missionIndex) / MissionData.GetMissionCount(missionIndex);
		initSpeed = 1f + Random.Range(0f,1f) * mission_rate;
		missionNum = MissionProgress.GetMissionCurIndex(missionIndex);

		if (Random.Range(0f, 1f) < 0.2f)
		{
			initSpeed  = 3f;
			runFlag = true;
		}
		anim = GetComponent<Animator>();					  
		anim.speed = initSpeed;
		//damage handler
		damageHandler = GetComponent<vp_DamageHandler> ();
	}

	protected override void UpdateAnimation(){
		//Check Game is play/pause
		if(GameManager.Instance.CurState == GameStates.PAUSE){
			if(anim.speed >0) anim.speed = 0f;
			return;
		}

		if(GameManager.Instance.CurState == GameStates.PLAY)
			anim.speed = initSpeed;

		if(GameManager.Instance.CurState == GameStates.SLOWDOWN)
			anim.speed = initSpeed / GameManager.Instance.SLOWDOWN_RATE;
		
		//////// update state flags...
		UpdateState();
		////////////**********zombie tatics************//////////
		switch(m_mainState){
			case MainState.DEAD:
				anim.SetInteger("MainState", (int)MainState.DEAD);
				anim.SetInteger("SubState", 0);
				break;
			case MainState.HURT:
				//APPLY hurt
				int hurt_type = Random.Range(0, 2);
				anim.SetInteger("MainState", (int)MainState.HURT);
				anim.SetInteger("SubState", hurt_type);
				ResetFlags();
				break;
			case MainState.SCREAM:
				anim.SetInteger("MainState", (int)MainState.SCREAM);
				anim.SetInteger("SubState", 0);	
				break;
			case MainState.IDLE:
				anim.SetInteger("MainState", (int)MainState.IDLE);
				anim.SetInteger("SubState", 0);	
				break;
			case MainState.MOVE:
				if (isFreezed) {
					anim.speed = 0;
					return;
				}
				if (m_subState == (int)MoveStates.WALK)
				{
					transform.Translate(0.01f * Vector3.forward, Space.Self);
					anim.SetInteger("MainState", (int)MainState.MOVE);
					anim.SetInteger("SubState", m_subState);	
				}
				else if (m_subState == (int)MoveStates.RUN)
				{
					transform.Translate(0.03f * Vector3.forward, Space.Self);
					anim.SetInteger("MainState", (int)MainState.MOVE);
					anim.SetInteger("SubState", m_subState);	
				}
				else if (m_subState == (int)MoveStates.STRAFE_LEFT)
				{
					transform.Translate(0.01f * Vector3.left, Space.Self);
					anim.SetInteger("MainState", (int)MainState.MOVE);
					anim.SetInteger("SubState", m_subState);	
				}
				else if (m_subState == (int)MoveStates.STRAFE_RIGHT)
				{
					transform.Translate(0.01f * Vector3.right, Space.Self);
					anim.SetInteger("MainState", (int)MainState.MOVE);
					anim.SetInteger("SubState", m_subState);	
				}
				break;
			case MainState.ATTACK:
				if (isFreezed) {
					anim.speed = 0;
					return;
				}
				anim.SetInteger("MainState", (int)MainState.ATTACK);
				anim.SetInteger("SubState", m_subState);
				break;
			default:
				break;
		}
	}

	void UpdateState(){
		SetTarget ();
		transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
		if(m_mainState == MainState.DEAD){
			return;
		}else if(m_mainState == MainState.HURT){
			return;
		}
		else if (Vector3.Distance(target.position ,transform.position) > 10f)
		{
			if (m_mainState != MainState.MOVE)
			{
				m_mainState = MainState.MOVE;
				if (runFlag == true)
				{
					m_subState = (int)MoveStates.RUN;
				}
				else
				{
					m_subState = (int)MoveStates.WALK;
				}
			}
			screamFlag = false;
			attackFlag = false;
		}
		else if (Vector3.Distance(target.position ,transform.position) > 2f){
			if(screamFlag == false && m_mainState!= MainState.SCREAM){
				m_mainState = MainState.SCREAM;
				attackFlag = false;
			}
			if(screamFlag == false){
				return;
			}
			if (m_mainState != MainState.MOVE)
			{
				m_mainState = MainState.MOVE;
				attackFlag = false;
				if (runFlag == true)
				{
					m_subState = (int)MoveStates.RUN;
				}
				else
				{
					m_subState = (int)MoveStates.WALK;
				}
			}
		}
		else {
			if (attackFlag == false)
			{
				attackFlag = true;
				m_subState = Random.Range(0, 5);
				m_mainState = MainState.ATTACK;
			}
		}
	}
	protected override void OnHitFromUFPS(DamageHit damagehit)
	{
		RaycastHit hit = damagehit.hit;
		float damage = damagehit.damage;
		Transform clone; // for the FX
		Debug.Log ("hit.transform.tag>>>>>>>>>>>>>>>" + hit.transform.gameObject.name);

		Debug.Log ("Hurted...............................................");
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
			
		//Check death
		DoCheckDeath();
	}
}
