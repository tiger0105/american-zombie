using UnityEngine;

public class FlameMuzzleFlashController : MonoBehaviour
{
	void FixedUpdate()
	{
		if (GlobalReferences._IsAttacking == true
			&& GetComponent<ParticleSystem>().isPlaying == false)
		{
			GetComponent<ParticleSystem>().Play(true);
			GetComponent<AudioSource>().Play();
		}
		if (GlobalReferences._IsAttacking == false
			&& GetComponent<ParticleSystem>().isStopped == false)
		{
			GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			GetComponent<AudioSource>().Stop();
		}
	}
}
