using UnityEngine;
using System.Collections;
using Polaris.GameData;

public class FarmerZombie : ZombieBehaviour {
	bool isRunnable = false;
	bool isScreamable = true;
	protected override void Start () 
	{
		//increate the speed of zombie as the game progresses
		missionIndex = MissionProgress.GetCurMissionType ();
		float mission_rate =(float) MissionProgress.GetMissionCurIndex (missionIndex) / MissionData.GetMissionCount(missionIndex);
		initSpeed = 1f + Random.Range(0f,1f) * mission_rate;
		missionNum = MissionProgress.GetMissionCurIndex(missionIndex);

		if (Random.Range(0f, 1f) < 0.2f) {
			initSpeed  = 3f;
			isRunnable = true;
		}

		anim = GetComponent<Animator>();					  
		anim.speed = initSpeed;
		//damage handler
		damageHandler = GetComponent<vp_DamageHandler> ();
	}

	protected override void UpdateAnimation(){
		//Check Game is play/pause
		if(GameManager.Instance.CurState == GameStates.PAUSE) {
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
		if (m_mainState == MainState.DEAD) {
			if (anim.speed > 1f)
				anim.speed = 1f;
			
			if (anim.GetInteger ("MainState") != (int)MainState.DEAD)
				anim.SetInteger ("MainState", (int)MainState.DEAD);
		} else if (m_mainState == MainState.HURT) {
			if (anim.speed > 1f)
				anim.speed = 1f;
			
			anim.SetInteger ("MainState", (int)MainState.HURT);
			anim.SetInteger ("SubState", 0);
		} else if (isFreezed) {
			anim.speed = 0;
			return;
		} else if (m_mainState == MainState.SCREAM)	{
			if(anim.speed >1f)
				anim.speed = 1f;
			
			anim.SetInteger("MainState", (int)MainState.SCREAM);
			anim.SetInteger("SubState", 0);	
		} else if (Vector3.Distance(target.position ,transform.position) > 3f) {
			if (isRunnable == true)	{
				m_mainState = MainState.MOVE;
				transform.Translate (0.03f * Vector3.forward, Space.Self);
				if (anim.GetInteger("MainState") != (int)MainState.MOVE) anim.SetInteger("MainState", (int)MainState.MOVE);
				anim.SetInteger("SubState", 1);
			} else {
				m_mainState = MainState.MOVE;
				transform.Translate (0.01f * Vector3.forward, Space.Self);
				if (anim.GetInteger("MainState") != (int)MainState.MOVE) anim.SetInteger("MainState", (int)MainState.MOVE);
				anim.SetInteger("SubState", 0);
			}
		} else if (Vector3.Distance(target.position ,transform.position) > 1.3f) {
			if(isScreamable == true) {
				isScreamable = false;
				m_mainState = MainState.SCREAM;
				return;
			}

			if (isRunnable == true)	{
				m_mainState = MainState.MOVE;
				transform.Translate (0.03f * Vector3.forward, Space.Self);
				if (anim.GetInteger("MainState") != (int)MainState.MOVE) anim.SetInteger("MainState", (int)MainState.MOVE);
				anim.SetInteger("SubState", 1);
			} else {
				m_mainState = MainState.MOVE;
				transform.Translate (0.01f * Vector3.forward, Space.Self);
				if (anim.GetInteger("MainState") != (int)MainState.MOVE) anim.SetInteger("MainState", (int)MainState.MOVE);
				anim.SetInteger("SubState", 0);
			}
		} else {
			int attack_type = Random.Range(0, 2);
			m_mainState = MainState.ATTACK;
			if (anim.GetInteger("MainState") != (int)MainState.ATTACK) anim.SetInteger("MainState", (int)MainState.ATTACK);
			anim.SetInteger("SubState", attack_type);
		}
	}
}