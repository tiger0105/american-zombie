using UnityEngine;
using UnityEngine.UI;
using Polaris.GameData;

public class TitleBar : MonoBehaviour
{
    [SerializeField] private Text m_CashAmount;
    [SerializeField] private Text m_GoldAmount;
    [SerializeField] private Slider m_EnergySlider;
    [SerializeField] private Slider m_MissionProgressSlider;
    [SerializeField] private Text m_StarStatusText;

    void Update()
    {
        m_EnergySlider.value = GameInfo.heartLives;
    }

    public void UpdatePlayInfo()
    {
        int mission_typ = MissionProgress.GetCurMissionType();
        int max_mission = MissionData.GetMissionCount(mission_typ) * 4;

        int stars = 0;

        GameManager.Instance.missionStars = CSVReader.Read("MissionStars");

        for (int i = 0; i < GameManager.Instance.missionStars.Count; i++)
        {
            stars += int.Parse(GameManager.Instance.missionStars[i]["stars"].ToString());
        }

        m_StarStatusText.text = stars.ToString() + " Stars Won  /  " + (100 - stars).ToString() + " Stars Left";

        m_CashAmount.text = GameInfo.playerCoin.ToString();
        m_GoldAmount.text = GameInfo.playerGold.ToString();

        m_MissionProgressSlider.minValue = 0;
        m_MissionProgressSlider.maxValue = max_mission;
        m_MissionProgressSlider.value = stars;

        m_EnergySlider.minValue = 0;
        m_EnergySlider.maxValue = GameInfo.HeartLivesMax;
        m_EnergySlider.value = GameInfo.heartLives;
    }
}