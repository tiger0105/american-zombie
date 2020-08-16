using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.UI;

public class MissionWindow : MonoBehaviour
{
    public static MissionWindow Instance;
    private int m_CurrentMissionType;
    [SerializeField] private Transform m_CampaignListRoot;
    [SerializeField] private Transform m_SurvivalListRoot;
    [SerializeField] private Transform m_TimeLimitListRoot;

    [SerializeField] private GameObject m_MissionWeaponsWindow;

    public void OnBackBtnClick()
    {
        GameManager.Instance.ProcEventMessages(EventMessages.ENTER_ITEMUPGRADE_WINDOW);
    }

    public void OnMissionStartBtnClick(int missionIndex)
    {
        SetMissionType();
        MissionProgress.SetMissionCurIndex(m_CurrentMissionType, missionIndex);
        if (GameInfo.heartLives == 0)
        {
            UIManager.Instance.Inform(InformTypes.NOT_ENOUGH_TIME, "Inform", "Out of Energy. Please wait for energy to refill or purchase food", "");
            return;
        }

        GameSetting.lastSceneIndex = 1;
        m_MissionWeaponsWindow.SetActive(true);
    }

    void Start()
    {
        Instance = this;
        MissionProgress.CurMissionReset();
        SetMissionStats();
    }

    public void SetMissionType()
    {
        m_CurrentMissionType = GetComponent<PanelTabManager>().currentPanelIndex;
    }

    public void SetMissionStats()
    {
        GameManager.Instance.missionStars = CSVReader.Read("MissionStars");

        for (int i = 0; i < m_CampaignListRoot.childCount; i ++)
        {
            int missionNum = int.Parse(m_CampaignListRoot.GetChild(i).name);
            m_CurrentMissionType = GetComponent<PanelTabManager>().currentPanelIndex;
            int missionCurIndex = MissionProgress.GetMissionCurIndex(m_CurrentMissionType) + 1;
            if (missionNum <= missionCurIndex)
            {
                m_CampaignListRoot.GetChild(i).GetChild(6).gameObject.SetActive(false);
                m_CampaignListRoot.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                m_CampaignListRoot.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
                {
                    OnMissionStartBtnClick(missionNum - 1);
                });
            }

            Transform m_MissionStarTrans = m_CampaignListRoot.GetChild(i).GetChild(5);

            if (missionNum < missionCurIndex)
            {
                int stars = int.Parse(GameManager.Instance.missionStars[missionNum - 1]["stars"].ToString());
                if (stars == 0)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        m_MissionStarTrans.GetChild(j).GetChild(0).gameObject.SetActive(false);
                        m_MissionStarTrans.GetChild(j).GetChild(1).gameObject.SetActive(true);
                    }
                }
                else if (stars == 1)
                {
                    m_MissionStarTrans.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    m_MissionStarTrans.GetChild(0).GetChild(1).gameObject.SetActive(false);

                    for (int j = 1; j < 4; j++)
                    {
                        m_MissionStarTrans.GetChild(j).GetChild(0).gameObject.SetActive(false);
                        m_MissionStarTrans.GetChild(j).GetChild(1).gameObject.SetActive(true);
                    }
                }
                else if (stars == 2)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        m_MissionStarTrans.GetChild(j).GetChild(0).gameObject.SetActive(true);
                        m_MissionStarTrans.GetChild(j).GetChild(1).gameObject.SetActive(false);
                    }

                    for (int j = 2; j < 4; j++)
                    {
                        m_MissionStarTrans.GetChild(j).GetChild(0).gameObject.SetActive(false);
                        m_MissionStarTrans.GetChild(j).GetChild(1).gameObject.SetActive(true);
                    }
                }
                else if (stars == 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        m_MissionStarTrans.GetChild(j).GetChild(0).gameObject.SetActive(true);
                        m_MissionStarTrans.GetChild(j).GetChild(1).gameObject.SetActive(false);
                    }

                    m_MissionStarTrans.GetChild(3).GetChild(0).gameObject.SetActive(false);
                    m_MissionStarTrans.GetChild(3).GetChild(1).gameObject.SetActive(true);
                }
                else if (stars == 4)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        m_MissionStarTrans.GetChild(j).GetChild(0).gameObject.SetActive(true);
                        m_MissionStarTrans.GetChild(j).GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                m_MissionStarTrans.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < m_SurvivalListRoot.childCount; i++)
        {
            int missionNum = int.Parse(m_SurvivalListRoot.GetChild(i).name);
            m_SurvivalListRoot.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            m_SurvivalListRoot.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
            {
                OnMissionStartBtnClick(missionNum - 1);
            });
        }

        for (int i = 0; i < m_TimeLimitListRoot.childCount; i++)
        {
            int missionNum = int.Parse(m_TimeLimitListRoot.GetChild(i).name);
            m_CurrentMissionType = GetComponent<PanelTabManager>().currentPanelIndex;
            int missionCurIndex = MissionProgress.GetMissionCurIndex(m_CurrentMissionType) + 1;
            if (missionNum <= missionCurIndex)
            {
                m_TimeLimitListRoot.GetChild(i).GetChild(5).gameObject.SetActive(false);
                m_TimeLimitListRoot.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                m_TimeLimitListRoot.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
                {
                    OnMissionStartBtnClick(missionNum - 1);
                });
            }
        }
    }
}