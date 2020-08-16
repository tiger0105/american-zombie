using UnityEngine;
using System.Collections;

public class ZombieActState : StateMachineBehaviour {
	int count = 0;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		try{
			string clip_str = animator.GetCurrentAnimatorClipInfo (layerIndex)[0].clip.name;
			//string clip_str = "";
			if(stateInfo.IsName ("Scream")) clip_str = "Scream";
			if(stateInfo.IsName ("GetFire")) clip_str = "GetFire";
			string zombi_name = animator.gameObject.name;
			string audio_path = string.Format ("Sound/{0}/{1}", zombi_name, clip_str); 
			//Debug.Log("<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> " + audio_path);
			AudioClip audioClip = Resources.Load<AudioClip> (audio_path);
			AudioSource audioSource = animator.gameObject.GetComponent<AudioSource>();
			if(!audioSource.isPlaying) audioSource.PlayOneShot (audioClip);
		}
		catch{
			;
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(animator.gameObject.name == "BLACKDOG_BOMB" || animator.gameObject.name == "ZOMBIE_DOG"){
			if (++count > 5) {
				animator.gameObject.GetComponent<BlackDogControl> ().IsAtking = true;
			}
		}
		if(stateInfo.IsTag("Hurt")){
			ZombieBehaviour zb = animator.gameObject.GetComponent<ZombieBehaviour>();
			if (zb == null)
				return;
			zb.M_mainState = MainState.IDLE;
		}
		if(stateInfo.IsTag("Scream")){
			ZombieBehaviour zb = animator.gameObject.GetComponent<ZombieBehaviour>();
			if (zb == null)
				return;
			if (++count > 5) {
				count = 0;
				zb.M_mainState = MainState.IDLE;
				zb.ScreamFlag = true;

			}
		}
		if(stateInfo.IsTag("Attack")){
			ZombieBehaviour zb = animator.gameObject.GetComponent<ZombieBehaviour>();
			if (zb == null)
				return;
			if (++count > 2) {
				count = 0;
				zb.AttackFlag = true;
				zb.M_mainState = MainState.MOVE;
				zb.M_subState = Random.Range((int)MoveStates.STRAFE_LEFT, (int)MoveStates.STRAFE_RIGHT + 1);
			}
		}
		if(stateInfo.IsTag("Strafe")){
			ZombieBehaviour zb = animator.gameObject.GetComponent<ZombieBehaviour>();
			if (zb == null)
				return;
			if (++count > 3) {
				count = 0;
				zb.AttackFlag = false;
				zb.M_mainState = MainState.IDLE;
				zb.M_subState = 0;
			}
		}
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
