using System.Collections;
using UnityEngine;

public class ReportManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_Reports;
    [SerializeField] private GameObject m_ReportDialog;
    [SerializeField] private Transform m_ReportInstantiateTrans;
    [SerializeField] private GameObject overGuiCamera;
    [SerializeField] private GameObject itemBuy;
    [SerializeField] private GameObject itemUpgrade;
    [SerializeField] private GameObject mission;

    public void HideReportDialog()
    {
        m_ReportDialog.SetActive(false);
    }

    private void ShowReportDialog()
    {
        itemBuy.SetActive(false);
        itemUpgrade.SetActive(false);
        overGuiCamera.SetActive(false);
        mission.SetActive(true);
        m_ReportDialog.SetActive(true);
    }

    private void SetReportIndex(int index)
    {
        foreach (Transform trans in m_ReportInstantiateTrans)
        {
            if (trans.gameObject.name != "Title")
            {
                trans.gameObject.SetActive(false);
                Destroy(trans.gameObject);
            }
        }

        GameObject report = Instantiate(m_Reports[index] as GameObject);
        report.transform.SetParent(m_ReportInstantiateTrans, false);
        report.transform.SetSiblingIndex(1);

    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        int mission_type = MissionProgress.GetCurMissionType();
        int mission_index = MissionProgress.GetMissionChallengeIndex(mission_type);

        if (mission_index == 1 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(0);
            ShowReportDialog();
        }
        else if (mission_index == 3 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(1);
            ShowReportDialog();
        }
        else if (mission_index == 4 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(2);
            ShowReportDialog();
        }
        else if (mission_index == 6 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(3);
            ShowReportDialog();
        }
        else if (mission_index == 8 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(4);
            ShowReportDialog();
        }
        else if (mission_index == 9 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(5);
            ShowReportDialog();
        }
        else if (mission_index == 10 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(6);
            ShowReportDialog();
        }
        else if (mission_index == 11 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(7);
            ShowReportDialog();
        }
        else if (mission_index == 17 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(8);
            ShowReportDialog();
        }
        else if (mission_index == 22 && GameSetting.lastSceneIndex == 3)
        {
            SetReportIndex(9);
            ShowReportDialog();
        }
        else
            HideReportDialog();
    }
}
