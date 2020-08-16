using Polaris.GameData;
using System.Collections;
using UnityEngine;

public class SEffectManager : MonoBehaviour
{
	[SerializeField] private AudioSource m_CoughAudioSource;
	[SerializeField] private AudioSource m_ScreamAudioSource;

	[SerializeField] private AudioClip[] m_PlayerCoughClips;
	[SerializeField] private AudioClip m_PlayerScreamClip;

	private void Start()
	{ 
		MissionData missionData = MissionProgress.GetCurMissionData();
		switch (missionData.Index)
		{
			case 6:
			case 11:
			case 17:
			case 21:
			case 22:
			case 23:
				// Cough Effect
				StartCoroutine(PlayCoughClip(5));
				break;
			case 4:
				StartCoroutine(PlayScreamClip(20));
				break;
			case 8:
			case 14:
			case 20:
				StartCoroutine(PlayCoughClip(5));
				StartCoroutine(PlayScreamClip(20));
				break;
			default:
				return;
		}
	}

	private IEnumerator PlayCoughClip(int delayedTime)
	{
		yield return new WaitForSeconds(delayedTime);
		m_CoughAudioSource.clip = m_PlayerCoughClips[Random.Range(0, m_PlayerCoughClips.Length)];
		m_CoughAudioSource.loop = false;
		m_CoughAudioSource.Play();
	}

	private IEnumerator PlayScreamClip(int delayedTime)
	{
		yield return new WaitForSeconds(delayedTime);
		m_ScreamAudioSource.clip = m_PlayerScreamClip;
		m_ScreamAudioSource.loop = false;
		m_ScreamAudioSource.Play();
	}
}
