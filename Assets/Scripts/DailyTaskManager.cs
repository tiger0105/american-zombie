using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyTaskManager : MonoBehaviour
{
    [SerializeField] private GameObject m_TaskListItem;
    [SerializeField] private Transform m_TaskListContent;
    private List<Dictionary<string, object>> taskListData;

    private void UpdateTaskList()
    {
        string dataAsJson = FileTool.ReadFile("Data", false);
        GameManager.Instance.persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);

        GlobalReferences.KillCount = PlayerPrefs.GetInt("KillCount", 0);
        GlobalReferences.SingleRoundFiveHeadshots = PlayerPrefs.GetInt("SingleRoundFiveHeadshots", 0);
        GlobalReferences.KillsWithMelee = PlayerPrefs.GetInt("KillsWithMelee", 0);
        GlobalReferences.RoundCompleteOneMin = PlayerPrefs.GetInt("RoundCompleteOneMin", 0);
        GlobalReferences.RoundCompleteMaxHealth = PlayerPrefs.GetInt("RoundCompleteMaxHealth", 0);
        GlobalReferences.ShareWithFB = PlayerPrefs.GetInt("ShareWithFB", 0);
        GlobalReferences.ShareWithInsta = PlayerPrefs.GetInt("ShareWithInsta", 0);
        GlobalReferences.RateFiveStar = PlayerPrefs.GetInt("RateFiveStar", 0);

        if (GlobalReferences.KillCount >= 10
            && GameManager.Instance.persistantData.taskNum.Contains(0) == false)
            GameManager.Instance.persistantData.taskNum.Add(0);

        if (GlobalReferences.SingleRoundFiveHeadshots == 1
            && GameManager.Instance.persistantData.taskNum.Contains(1) == false)
            GameManager.Instance.persistantData.taskNum.Add(1);

        if (GlobalReferences.KillCount >= 20
            && GameManager.Instance.persistantData.taskNum.Contains(2) == false)
            GameManager.Instance.persistantData.taskNum.Add(2);

        if (GlobalReferences.KillsWithMelee >= 10
            && GameManager.Instance.persistantData.taskNum.Contains(3) == false)
            GameManager.Instance.persistantData.taskNum.Add(3);

        if (GlobalReferences.KillCount >= 50
            && GameManager.Instance.persistantData.taskNum.Contains(4) == false)
            GameManager.Instance.persistantData.taskNum.Add(4);

        if (GlobalReferences.RoundCompleteOneMin  == 1
            && GameManager.Instance.persistantData.taskNum.Contains(5) == false)
            GameManager.Instance.persistantData.taskNum.Add(5);

        if (GlobalReferences.RoundCompleteMaxHealth == 1
            && GameManager.Instance.persistantData.taskNum.Contains(6) == false)
            GameManager.Instance.persistantData.taskNum.Add(6);

        if (GlobalReferences.KillCount >= 75
            && GameManager.Instance.persistantData.taskNum.Contains(7) == false)
            GameManager.Instance.persistantData.taskNum.Add(7);

        if (GlobalReferences.ShareWithFB == 1
            && GameManager.Instance.persistantData.taskNum.Contains(8) == false)
            GameManager.Instance.persistantData.taskNum.Add(8);

        if (GlobalReferences.ShareWithInsta == 1
            && GameManager.Instance.persistantData.taskNum.Contains(9) == false)
            GameManager.Instance.persistantData.taskNum.Add(9);

        if (GlobalReferences.RateFiveStar == 1
            && GameManager.Instance.persistantData.taskNum.Contains(10) == false)
            GameManager.Instance.persistantData.taskNum.Add(10);
    }

    private void Start()
    {
        taskListData = new List<Dictionary<string, object>>();
        taskListData = CSVReader.Read("TaskList");

        UpdateTaskList();

        for (int i = 0; i < taskListData.Count; i++)
        {
            GameObject item = Instantiate(m_TaskListItem);
            item.transform.SetParent(m_TaskListContent, false);
            item.name = i.ToString();
            item.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "TASK " + (i + 1);

            if (GameManager.Instance == null)
                return;

            item.transform.GetChild(1).GetComponent<Text>().text = taskListData[i]["task"].ToString();
            item.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            item.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            if (i == 3 || i == 7)
            {
                item.transform.GetChild(2).GetChild(0).GetComponent<Image>().enabled = false;
                item.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true);
            }

            item.transform.GetChild(3).GetComponent<Text>().text = taskListData[i]["rewards"].ToString();

            if (GameManager.Instance.persistantData.taskNum.Contains(i) == true)
            {
                Color completedColor = Color.white;
                ColorUtility.TryParseHtmlString("#00A0FFFF", out completedColor);
                item.transform.GetChild(0).GetComponent<Image>().color = completedColor;
                item.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

                if (GameManager.Instance.persistantData.rewardsNum.Contains(i) == true)
                {
                    item.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.clear;
                    item.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = Color.clear;
                    item.transform.GetChild(3).GetComponent<Text>().text = "Claimed";
                }
                else
                {
                    item.transform.GetChild(1).GetComponent<Text>().text = "";
                    item.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                    int index = i;
                    item.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
                    {
                        if (GameManager.Instance.persistantData.rewardsNum.Contains(index) == false)
                        {
                            GameManager.Instance.persistantData.rewardsNum.Add(index);
                            string dataAsJson = JsonUtility.ToJson(GameManager.Instance.persistantData);
                            FileTool.createORwriteFile("Data", dataAsJson);

                            item.transform.GetChild(1).GetComponent<Text>().text = taskListData[index]["task"].ToString();
                            item.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.clear;
                            item.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = Color.clear;
                            item.transform.GetChild(3).GetComponent<Text>().text = "Claimed";

                            GameInfo.Load();
                            int rewards = 0;

                            if (index == 3 || index == 7)
                            {
                                int.TryParse(taskListData[index]["rewards"].ToString(), out rewards);
                                GameInfo.playerGold += rewards;
                                GameInfo.SavePlayerGold();
                            }
                            else
                            {
                                int.TryParse(taskListData[index]["rewards"].ToString(), out rewards);
                                GameInfo.playerCoin += rewards;
                                GameInfo.SavePlayerCoin();
                            }
                        }
                    });
                }
            }
        }
    }
}