using UnityEngine;
using System.Collections.Generic;
using Polaris.GameData;
using Polaris.Base;
using UnityEngine.UI;
using System.Collections;

public enum MissionResults {
	MISSION_SUCCESS,
	MISSION_FAILED,
	COUNT
}
public class ScoreManager : MonoBehaviour {
	public static MissionResults missionResult;
	//scores in current round ...
	public static int killedCount;
    public static int killedWithMelee;
	public static int pickupCount;
	//public static List<int> killCountList;
	public static int headshotCount; 
	public static float performTime; // seconds
	public static float hP;	 //health percent 0-100%
    [SerializeField] private Sprite[] m_Sprites;

    //mission goal scores... 
    public static int missionKills;
	public static int missionPickups;
	public static float missionHP;
	public static int missionHeadShots;
	public static float missionTime;

	public ScoreUIControl scoreUIControl;

    private List<Dictionary<string, object>> data;
    [SerializeField] private Transform m_NotifyParent;
    [SerializeField] private Sprite[] m_PlantImages;
    [SerializeField] private string[] m_PlantNames;
    // Use this for initialization
    private IEnumerator Start()
    {
        string dataAsJson = FileTool.ReadFile("Data", false);
        PersistantData persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);
        
        MissionData md = MissionProgress.GetCurMissionData();

        if (md.Type == "Time Limit" && md.Name == "Easy")
            missionKills = 15;
        else if (md.Type == "Time Limit" && md.Name == "Medium")
            missionKills = 40;
        else if (md.Type == "Time Limit" && md.Name == "Hard")
            missionKills = 80;
        else
            missionKills = md.GetMissionKills();
        missionPickups = md.GetMissionPickups();
        missionHeadShots = missionKills / 2;
        missionHP = md.GetMissionHP();
        missionTime = md.GetMissionTime();
        //initialize current round score

        //killCountList = new List<int> ();
        //killCountList.Clear ();
        //evaluate
        Evaluate();

        if (GameSetting.musicOn == true)
        {
            GetComponent<AudioSource>().mute = false;
        }
        else
        {
            GetComponent<AudioSource>().mute = true;
        }

        yield return new WaitForSeconds(1);

        if (persistantData.isAdsRemoved == false)
            ManiacoAds.m_ManiacoAdsInstance.ShowInterstitialAds();
    }

    private void SaveMissionStarStatus(List<Dictionary<string, object>> data)
    {
        string temp = "";
        temp += "missionId,stars\n";
        for (int i = 0; i < data.Count; i++)
        {
            temp = temp + i.ToString()
                + "," + data[i]["stars"].ToString() + "\n";
        }
        FileTool.createORwriteFile("MissionStars", temp);
    }

    void Evaluate(){

        GlobalReferences.KillCount = PlayerPrefs.GetInt("KillCount", 0);
        GlobalReferences.KillCount += killedCount;
        PlayerPrefs.SetInt("KillCount", GlobalReferences.KillCount);

        GlobalReferences.SingleRoundFiveHeadshots = PlayerPrefs.GetInt("KillCount", 0);
        if (GlobalReferences.SingleRoundFiveHeadshots == 0 && headshotCount >= 5)
        {
            GlobalReferences.SingleRoundFiveHeadshots = 1;
            PlayerPrefs.SetInt("SingleRoundFiveHeadshots", GlobalReferences.SingleRoundFiveHeadshots);
        }

        GlobalReferences.KillsWithMelee = PlayerPrefs.GetInt("KillsWithMelee", 0);
        GlobalReferences.KillsWithMelee += killedWithMelee;
        PlayerPrefs.SetInt("KillsWithMelee", GlobalReferences.KillsWithMelee);

        GlobalReferences.RoundCompleteOneMin = PlayerPrefs.GetInt("RoundCompleteOneMin", 0);
        if (GlobalReferences.RoundCompleteOneMin == 0 && performTime < 60)
        {
            GlobalReferences.RoundCompleteOneMin = 1;
            PlayerPrefs.SetInt("RoundCompleteOneMin", GlobalReferences.RoundCompleteOneMin);
        }

        GlobalReferences.RoundCompleteMaxHealth = PlayerPrefs.GetInt("RoundCompleteMaxHealth", 0);
        if (GlobalReferences.RoundCompleteMaxHealth == 0 && hP == 100)
        {
            GlobalReferences.RoundCompleteMaxHealth = 1;
            PlayerPrefs.SetInt("RoundCompleteMaxHealth", GlobalReferences.RoundCompleteMaxHealth);
        }

        int kills_flag = 0;
        int headshot_flag = 0;
		int ontime_flag = 0;
		int hp_flag = 0;
		int success_count = 0;

		if(killedCount >= missionKills){
			kills_flag = 1;
			success_count++;
		}
		if(headshotCount >= missionHeadShots){
			headshot_flag = 1;
			success_count++;
		}
		if(hP >= 50) {
			hp_flag = 1;
			success_count++;
		}
		if(performTime <= missionTime){
			ontime_flag = 1;
			success_count++;
		}

		MissionData md = MissionProgress.GetCurMissionData();
		if (md.performTime > 0) {
            missionResult = MissionResults.MISSION_SUCCESS;
            GameInfo.playerLevel = GameInfo.maxLevel;
            GameInfo.SavePlayerLevel();
        } else {
			//main mission
			if (success_count < 3) {
				missionResult = MissionResults.MISSION_FAILED;
			} else {
				missionResult = MissionResults.MISSION_SUCCESS;
			}
			GameInfo.playerLevel = success_count;
			GameInfo.SavePlayerLevel ();
            data = new List<Dictionary<string, object>>();
            data = CSVReader.Read("MissionStars");
            int prevStars = int.Parse(data[md.Index]["stars"].ToString());
            if (prevStars < success_count)
            {
                data[md.Index]["stars"] = success_count.ToString();
                Debug.Log("SaveMissionStarStatus");
                SaveMissionStarStatus(data);
            }

            if (success_count == 4)
            {
                data = CSVReader.Read("MissionStars");
                if (md.Index / 5 > 0)
                {
                    int pId = md.Index / 5;
                    string dataAsJson;
                    GameObject notify;
                    switch (pId)
                    {
                        case 1:
                            if (GameManager.Instance.persistantData.plantsNum.Contains(1) == false)
                            {
                                notify = Instantiate(Resources.Load("Prefabs/Notify_Plants") as GameObject);
                                notify.transform.SetParent(m_NotifyParent, false);
                                notify.transform.GetChild(0).GetComponent<Image>().sprite = m_PlantImages[1];
                                notify.transform.GetChild(1).GetComponent<Text>().text = "You've collected " + m_PlantNames[1];
                                Destroy(notify, 5);

                                GameManager.Instance.persistantData.plantsNum.Add(1);
                                dataAsJson = JsonUtility.ToJson(GameManager.Instance.persistantData);
                                FileTool.createORwriteFile("Data", dataAsJson);
                            }
                            break;
                        case 2:
                            if (GameManager.Instance.persistantData.plantsNum.Contains(3) == false)
                            {
                                notify = Instantiate(Resources.Load("Prefabs/Notify_Plants") as GameObject);
                                notify.transform.SetParent(m_NotifyParent, false);
                                notify.transform.GetChild(0).GetComponent<Image>().sprite = m_PlantImages[3];
                                notify.transform.GetChild(1).GetComponent<Text>().text = "You've collected " + m_PlantNames[3];
                                Destroy(notify, 5);

                                GameManager.Instance.persistantData.plantsNum.Add(3);
                                dataAsJson = JsonUtility.ToJson(GameManager.Instance.persistantData);
                                FileTool.createORwriteFile("Data", dataAsJson);
                            }
                            break;
                        case 3:
                            if (GameManager.Instance.persistantData.plantsNum.Contains(5) == false)
                            {
                                notify = Instantiate(Resources.Load("Prefabs/Notify_Plants") as GameObject);
                                notify.transform.SetParent(m_NotifyParent, false);
                                notify.transform.GetChild(0).GetComponent<Image>().sprite = m_PlantImages[5];
                                notify.transform.GetChild(1).GetComponent<Text>().text = "You've collected " + m_PlantNames[5];
                                Destroy(notify, 5);

                                GameManager.Instance.persistantData.plantsNum.Add(5);
                                dataAsJson = JsonUtility.ToJson(GameManager.Instance.persistantData);
                                FileTool.createORwriteFile("Data", dataAsJson);
                            }
                            break;
                        case 4:
                            if (GameManager.Instance.persistantData.plantsNum.Contains(7) == false)
                            {
                                notify = Instantiate(Resources.Load("Prefabs/Notify_Plants") as GameObject);
                                notify.transform.SetParent(m_NotifyParent, false);
                                notify.transform.GetChild(0).GetComponent<Image>().sprite = m_PlantImages[7];
                                notify.transform.GetChild(1).GetComponent<Text>().text = "You've collected " + m_PlantNames[7];
                                Destroy(notify, 5);

                                GameManager.Instance.persistantData.plantsNum.Add(7);
                                dataAsJson = JsonUtility.ToJson(GameManager.Instance.persistantData);
                                FileTool.createORwriteFile("Data", dataAsJson);
                            }
                            break;
                        case 5:
                            if (GameManager.Instance.persistantData.plantsNum.Contains(9) == false)
                            {
                                notify = Instantiate(Resources.Load("Prefabs/Notify_Plants") as GameObject);
                                notify.transform.SetParent(m_NotifyParent, false);
                                notify.transform.GetChild(0).GetComponent<Image>().sprite = m_PlantImages[9];
                                notify.transform.GetChild(1).GetComponent<Text>().text = "You've collected " + m_PlantNames[9];
                                Destroy(notify, 5);

                                GameManager.Instance.persistantData.plantsNum.Add(9);
                                dataAsJson = JsonUtility.ToJson(GameManager.Instance.persistantData);
                                FileTool.createORwriteFile("Data", dataAsJson);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

		int mission_coin = 0;
		int kill_coin = GameInfo.killBonus * killedCount;
        int headshot_coin = 0;
        if (headshot_flag == 1)
		    headshot_coin = 1;

		int hp_coin = 0;
		int ontime_coin = 0;
		if (missionResult.Equals (MissionResults.MISSION_SUCCESS)) {
			mission_coin = MissionProgress.GetCurMissionData().Coin;
			int type = MissionProgress.GetCurMissionType ();
			if(MissionProgress.GetMissionChallengeIndex(type) <= MissionProgress.GetMissionCurIndex(type)){
				int up_index = MissionProgress.GetMissionCurIndex (type);
				MissionProgress.SetMissionChallengeIndex (type, ++up_index);
			}
			hp_coin = (int)(GameInfo.fullHpBonus * hP);
			ontime_coin = (int)(GameInfo.maxMissionTime / performTime);
            GameInfo.playerGold += 1;
        } 
		int coin = 0;
		coin += mission_coin;
		coin += kill_coin;
        if (MissionProgress.GetCurMissionData().Type == "Campaign")
        {
            coin += hp_coin;
            coin += ontime_coin;
        }

        if (GameManager.Instance.b_DoubleCashActivated == true)
        {
            GameInfo.playerCoin += coin * 2;
        }
        else
            GameInfo.playerCoin += coin; 
		GameInfo.SavePlayerCoin ();
        GameInfo.playerGold += headshot_coin;
        GameInfo.SavePlayerGold();
		scoreUIControl.DisplayScore (missionResult, coin, mission_coin, kill_coin, headshot_coin, hp_coin, ontime_coin, kills_flag, headshot_flag, hp_flag, ontime_flag);
		//waste energy
		GameInfo.heartLives = BaseFuncs.Decrease(GameInfo.heartLives, 1, GameInfo.HeartLivesMax, 0);
		GameInfo.SaveHeartLives();
		GameInfo.lastEnergyIO_TimeTicksString = System.DateTime.Now.Ticks.ToString();
	}
}
