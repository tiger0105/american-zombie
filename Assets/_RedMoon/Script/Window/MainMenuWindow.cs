using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    [SerializeField] private GameObject m_CureDialog;
    [SerializeField] private Transform m_PlantsRoot;
    [SerializeField] private GameObject m_PlantsInfoDialog;
    [SerializeField] private Text m_UnlockedCount;

    public class Plants
    {
        public int missionId;
        public int zombiePlant;
        public int bossPlant;
    }

    public void OnStartBtnClick()
    {
        GameManager.Instance.ProcEventMessages(EventMessages.ENTER_ITEMUPGRADE_WINDOW);
    }
    public void OnOptionBtnClick()
    {
        GameManager.Instance.ProcEventMessages(EventMessages.ENTER_SETTING_WINDOW);
    }
    public void OnLevelBtnClick()
    {
        GameManager.Instance.ProcEventMessages(EventMessages.ENTER_MISSION_WINDOW);
    }

    public void OpenShopUrl()
    {
        Application.OpenURL("https://maniacogames.kincustom.com/");
    }

    public void OpenCureDialog()
    {
        string dataAsJson = FileTool.ReadFile("Data", false);
        GameManager.Instance.persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);
        for (int i = 0; i < GameManager.Instance.persistantData.plantsNum.Count; i++)
        {
            m_PlantsRoot.GetChild(GameManager.Instance.persistantData.plantsNum[i]).GetChild(0).gameObject.SetActive(true);
        }
        m_UnlockedCount.text = GameManager.Instance.persistantData.plantsNum.Count.ToString();
        m_CureDialog.SetActive(true);
    }

    public void CloseCureDialog()
    {
        m_CureDialog.SetActive(false);
    }

    public void OpenPlantsInfoDialog()
    {
        m_PlantsInfoDialog.SetActive(true);
    }

    public void ClosePlantsInfoDialog()
    {
        m_PlantsInfoDialog.SetActive(false);
    }

    public void OpenFacebook()
    {
        Application.OpenURL("https://www.facebook.com/AmericanZombieAR/");
    }

    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/american_zombie_game/");
    }

    public void OpenLocallyNews()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.local.newsly");
    }
}