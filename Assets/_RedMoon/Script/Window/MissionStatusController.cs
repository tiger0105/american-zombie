using UnityEngine;

public class MissionStatusController : MonoBehaviour {

    [SerializeField] private Transform m_MissionsRoot;
    [SerializeField] private GameObject m_InfoDialog;

    private void OnEnable()
    {
        int missionIndex = MissionProgress.GetMissionChallengeIndex(0);

        GameManager.Instance.missionStars = CSVReader.Read("MissionStars");
        for (int i = 0; i < GameManager.Instance.missionStars.Count; i ++)
        {
            if (i <= missionIndex)
            {
                m_MissionsRoot.GetChild(i).GetChild(1).gameObject.SetActive(true);
                m_MissionsRoot.GetChild(i).GetChild(2).gameObject.SetActive(false);
                int stars = int.Parse(GameManager.Instance.missionStars[i]["stars"].ToString());
                if (stars < 0)
                    stars = 0;

                if (stars == 0)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(0).gameObject.SetActive(false);
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(1).gameObject.SetActive(true);
                    }
                }
                else if (stars == 1)
                {
                    m_MissionsRoot.GetChild(i).GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    m_MissionsRoot.GetChild(i).GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false);

                    for (int j = 1; j < 4; j++)
                    {
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(0).gameObject.SetActive(false);
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(1).gameObject.SetActive(true);
                    }
                }
                else if (stars == 2)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(0).gameObject.SetActive(true);
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(1).gameObject.SetActive(false);
                    }

                    for (int j = 2; j < 4; j++)
                    {
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(0).gameObject.SetActive(false);
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(1).gameObject.SetActive(true);
                    }
                }
                else if (stars == 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(0).gameObject.SetActive(true);
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(1).gameObject.SetActive(false);
                    }

                    m_MissionsRoot.GetChild(i).GetChild(1).GetChild(3).GetChild(0).gameObject.SetActive(false);
                    m_MissionsRoot.GetChild(i).GetChild(1).GetChild(3).GetChild(1).gameObject.SetActive(true);
                }
                else if (stars == 4)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(0).gameObject.SetActive(true);
                        m_MissionsRoot.GetChild(i).GetChild(1).GetChild(j).GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                m_MissionsRoot.GetChild(i).GetChild(1).gameObject.SetActive(false);
                m_MissionsRoot.GetChild(i).GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void DisplayInfoDialog()
    {
        m_InfoDialog.SetActive(true);
    }

    public void HideInfoDialog()
    {
        m_InfoDialog.SetActive(false);
    }
}
