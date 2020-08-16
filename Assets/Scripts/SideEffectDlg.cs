using Polaris.GameData;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SideEffectDlg : MonoBehaviour {
	[SerializeField] private GameObject m_Player;
	[SerializeField] private GameObject m_SideEffectDlg;
	[SerializeField] private string[] m_SideEffectTexts;
	[SerializeField] private string[] m_MoodTexts;

	private IEnumerator ShowSideEffectDlg(int delayedTime)
	{
		yield return new WaitForSeconds(delayedTime);
		m_SideEffectDlg.SetActive(true);
		m_Player.GetComponent<vp_SimpleCrosshair>().Hide = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_SideEffectDlg.GetComponent<RectTransform>());
		m_SideEffectDlg.GetComponent<Animator>().enabled = false;
		m_SideEffectDlg.GetComponent<Animator>().enabled = true;
	}

	private IEnumerator HideSideEffectDlg(int delayedTime)
	{
		yield return new WaitForSeconds(delayedTime);
		m_SideEffectDlg.SetActive(false);
		m_Player.GetComponent<vp_SimpleCrosshair>().Hide = false;
	}

	public void SetSideEffectText(int index)
	{
		m_SideEffectDlg.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
			= m_SideEffectTexts[index].Replace("<br>", "\n");
	}

	public void SetMoodText(int index)
	{
		m_SideEffectDlg.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = m_MoodTexts[index];
	}

	private void Start()
	{
		MissionData missionData = MissionProgress.GetCurMissionData();
		int dataIndex;
		switch (missionData.Index)
		{
			case 0:
				dataIndex = 0;
				break;
			case 3:
				dataIndex = 1;
				break;
			case 6:
				dataIndex = 2;
				break;
			case 11:
				dataIndex = 3;
				break;
			case 15:
				dataIndex = 4;
				break;
			case 18:
				dataIndex = 5;
				break;
			case 24:
				dataIndex = 6;
				break;
			default:
				return;
		}

		SetSideEffectText(dataIndex);
		SetMoodText(dataIndex);
		StartCoroutine(ShowSideEffectDlg(1));
		StartCoroutine(HideSideEffectDlg(7));
	}
}
