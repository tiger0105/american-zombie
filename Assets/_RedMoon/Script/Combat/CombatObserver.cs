using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Polaris.GameData;
using MLSpace;
using UnityEngine.SceneManagement;

public class CombatObserver : MonoBehaviour
{
    public static CombatObserver Instance;
    public GameObject hero;
    public vp_UnitBankType[] unitBanks;
    public vp_UnitType[] units;
    public AudioSource backmusicAudioSource;
    public Combat_UIManager uiMan;
    public KillEffect multiKillArt;
    [HideInInspector] public int[] arrowCounts;
    private float killSTime = 0;
    readonly float KILL_TIME_INTERVAL = 10;
    private int killsCount = 0;
    private int missionKills;
    private int missionPickups;
    private float missionHP;
    private int missionHeadShots;
    private float missionTime;
    private vp_FPPlayerDamageHandler playerDamageHandler;
    private vp_FPPlayerEventHandler playerEventHandler;
    private MissionData curMission;
    private AudioClip multiKillClip;

    [SerializeField] private GameObject m_HealthPackButton;
    public WebcamControll webCamControl;

    int zombie = 0;
    int boss = 0;

    int randomZombie = -1;

    public GameObject m_Score;
    public GameObject m_Time;

    public Transform m_NotifyParent;
    public Sprite[] m_NotifySprites;

    private List<Dictionary<string, object>> data;

    public Sprite[] m_PlantImages;
    public string[] m_PlantNames;

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag.Contains(tag))
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null;
    }

    public void DoHookHit(RaycastHit hit)
    {
        if (hit.transform.tag == "soldier")
        {
            GameObject go = hit.transform.gameObject;
            DamageHit damhit = new DamageHit();
            damhit.hit = hit;
            vp_ItemIdentifier identifier = GameObject.FindWithTag("Player").GetComponent<vp_PlayerInventory>().CurrentWeaponIdentifier;
            int weapon_id = int.Parse(identifier.name.Substring(0, 1)) - 1;
            int inventory_id = WeaponInventory.GetInventoryID(weapon_id);
            int newLevel = WeaponInventory.levelList[inventory_id] - 1;
            int added = newLevel * WeaponData.dataMap[inventory_id + 1].GetDamage_LevelUp();
            damhit.damage = WeaponData.dataMap[inventory_id + 1].GetDamage() + added;
            go.SendMessage("OnHitFromUFPS", damhit, SendMessageOptions.DontRequireReceiver);
        }
        else if (hit.transform.gameObject.GetComponent<BodyColliderScript>() != null)
        {
            GameObject go = hit.transform.gameObject.GetComponent<BodyColliderScript>().ParentObject;
            DamageHit damhit = new DamageHit
            {
                hit = hit
            };
            vp_ItemIdentifier identifier = GameObject.FindWithTag("Player").GetComponent<vp_PlayerInventory>().CurrentWeaponIdentifier;
            int weapon_id = int.Parse(identifier.name.Substring(0, 1)) - 1;
            int inventory_id = WeaponInventory.GetInventoryID(weapon_id);
            int newLevel = WeaponInventory.levelList[inventory_id] - 1;
            int added = newLevel * WeaponData.dataMap[inventory_id + 1].GetDamage_LevelUp();
            damhit.damage = WeaponData.dataMap[inventory_id + 1].GetDamage() + added;
            go.SendMessage("OnHitFromUFPS", damhit, SendMessageOptions.DontRequireReceiver);
        }
    }
    public void PickupItem(GameObject item)
    {
        StartCoroutine(DoPickupItem(item, 2f));
    }

    void Play()
    {
        GameManager.Instance.CurState = GameStates.PLAY;
    }
    public void ToNextLevel()
    {
        if (curMission.performTime > 0)
        {
            ScoreManager.performTime = curMission.performTime;
        }

        StartCoroutine(NextLevel(2f));
    }
    public void ApplyTimeBonus(bool result)
    {
        if (result == true)
        {
            if (curMission.performTime > 0)
            {
                ScoreManager.performTime = Time.time;
                GameManager.Instance.CurState = GameStates.PLAY;
            }
        }
        else
        {
            GameManager.Instance.CurState = GameStates.PAUSE;
            StartCoroutine(NextLevel(2f));
        }

    }

    void Awake()
    {
        ScoreManager.killedCount = 0;
        ScoreManager.pickupCount = 0;
        ScoreManager.headshotCount = 0;
        ScoreManager.performTime = Time.time;
        ScoreManager.hP = 100f;
        Instance = this;
        curMission = MissionProgress.GetCurMissionData();

        if (curMission.Type == "Time Limit")
        {
            m_Time.SetActive(true);
            m_Time.GetComponentInChildren<Text>().text = "Waiting...";
            StartCoroutine(ReduceTime());
        }
        else
        {
            m_Time.SetActive(false);
        }

        missionKills = curMission.GetMissionKills();
        missionPickups = curMission.GetMissionPickups();
        missionHeadShots = curMission.GetMissionHeadShots();
        missionHP = curMission.GetMissionHP();
        missionTime = curMission.GetMissionTime();
        playerDamageHandler = hero.GetComponent<vp_FPPlayerDamageHandler>();
        playerEventHandler = hero.GetComponent<vp_FPPlayerEventHandler>();

        if (curMission.Index == 5)
        {
            StartCoroutine(DisplayWarningNotification(5, curMission.Index));
        }
        else if (curMission.Index == 12)
        {
            StartCoroutine(DisplayWarningNotification(5, curMission.Index));
        }
        else if (curMission.Index == 19)
        {
            StartCoroutine(DisplayWarningNotification(5, curMission.Index));
        }

        data = new List<Dictionary<string, object>>();
        data = CSVReader.Read("Plants");

        string zombieStr = data[curMission.Index]["zombiePlant"].ToString();
        string bossStr = data[curMission.Index]["bossPlant"].ToString();
        if (zombieStr != "")
        {
            zombie = int.Parse(zombieStr);
            if (zombie >= 0)
            {
                if (boss > 0)
                    randomZombie = Random.Range(1, missionKills - 1);
                else
                    randomZombie = Random.Range(1, missionKills);
            }
        }
        else
            zombie = -1;

        if (bossStr != "")
            boss = int.Parse(bossStr);
        else
            boss = -1;
    }

    private IEnumerator DisplayWarningNotification(float time, int missionIndex)
    {
        yield return new WaitForSeconds(time);

        AudioSource[] components = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < components.Length; i ++)
        {
            components[i].volume = 0.1f;
        }

        GetComponent<AudioSource>().mute = true;

        if (missionIndex == 5)
        {
            GameObject notify = Instantiate(Resources.Load("Prefabs/Notify") as GameObject);
            notify.transform.SetParent(m_NotifyParent, false);
            notify.transform.GetChild(2).GetComponent<Text>().text = "From the Office of the President of the United States of America";
            notify.transform.GetChild(0).GetComponent<Image>().sprite = m_NotifySprites[0];
            Destroy(notify, 43);
            StartCoroutine(NormalizeSounds(43));
        }
        else if (missionIndex == 12)
        {
            GameObject notify = Instantiate(Resources.Load("Prefabs/Notify_Hillary") as GameObject);
            notify.transform.SetParent(m_NotifyParent, false);
            notify.transform.GetChild(2).GetComponent<Text>().text = "From the Office of Federal Emergency Management";
            notify.transform.GetChild(0).GetComponent<Image>().sprite = m_NotifySprites[1];
            Destroy(notify, 79);
            StartCoroutine(NormalizeSounds(79));
        }
        else if (missionIndex == 19)
        {
            GameObject notify = Instantiate(Resources.Load("Prefabs/Notify_Kennedy") as GameObject);
            notify.transform.SetParent(m_NotifyParent, false);
            notify.transform.GetChild(2).GetComponent<Text>().text = "From the Inauguration of President John Fitzgerald Kennedy";
            notify.transform.GetChild(0).GetComponent<Image>().sprite = m_NotifySprites[2];
            Destroy(notify, 86);
            StartCoroutine(NormalizeSounds(86));
        }
    }

    private IEnumerator NormalizeSounds(float time)
    {
        yield return new WaitForSeconds(time);

        AudioSource[] components = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < components.Length; i++)
        {
            components[i].volume = 1;
        }

        GetComponent<AudioSource>().mute = false;
    }

    public void DisplayHealthIcon()
    {
        m_HealthPackButton.SetActive(true);
    }

    void Start()
    {
        arrowCounts = new int[] { 10, 10, 10 };

        GameManager.Instance.CurState = GameStates.PLAY;
        for (int i = 0; i < WeaponInventory.GetCount(); i++)
        {
            WeaponData wd = WeaponInventory.GetWeaponData(i);
            GameObject obj = Resources.Load<GameObject>(wd.PickUpModel);
            GameObject clone = Instantiate(obj);
            clone.tag = "NoScore";
            vp_ItemPickup pick = clone.GetComponent<vp_ItemPickup>();
            if (pick != null)
            {
                Collider col = hero.GetComponent<Collider>();
                pick.TryGiveTo(col);
            }
        }

        for (int i = 0; i < ItemInventory.GetCount(); i++)
        {
            ItemData id = ItemInventory.GetItemData(i);

            if (id.PickUpModel.Contains("health_up") == true)
            {
                DisplayHealthIcon();
            }
            else if (id.PickUpModel.Contains("spectacles") == true)
            {
            }
            else
            {
                GameObject obj = Resources.Load<GameObject>(id.PickUpModel);
                GameObject clone = Instantiate(obj);
                clone.tag = "NoScore";
                vp_ItemPickup pick = clone.GetComponent<vp_ItemPickup>();
                if (pick != null)
                {
                    Collider col = hero.GetComponent<Collider>();
                    pick.TryGiveTo(col);
                }
            }
        }

        if (GameSetting.musicOn == true)
        { 
            int back_music_count = 7;
            string clip_Str = string.Format("Sound/back_music{0}", Random.Range(1, back_music_count));
            AudioClip audio_clip = Resources.Load<AudioClip>(clip_Str);
            backmusicAudioSource.clip = audio_clip;
            backmusicAudioSource.loop = true;
            backmusicAudioSource.Play();
        }

        if (GameSetting.isTestMode == true)
        {
            playerDamageHandler.MaxHealth = 100000;
            playerDamageHandler.CurrentHealth = playerDamageHandler.MaxHealth;
        }

        uiMan.ShowRadarView();
        prevTimeFrame = Time.time;
        hero.GetComponent<vp_FPPlayerDamageHandler>().currentArmor = PlayerPrefs.GetFloat("CurrentArmor", 0);
        multiKillClip = Resources.Load<AudioClip>("Sound/MultiKill");

        if (GameSetting.musicOn == false)
        {
            GetComponent<AudioSource>().mute = true;
        }
        else
        {
            GetComponent<AudioSource>().mute = false;
        }
    }

    private IEnumerator ReduceTime()
    {
        yield return new WaitForSeconds(13);

        if (curMission.Name == "Easy")
            m_Time.GetComponentInChildren<Text>().text = "30";
        else if (curMission.Name == "Medium")
            m_Time.GetComponentInChildren<Text>().text = "60";
        else if (curMission.Name == "Hard")
            m_Time.GetComponentInChildren<Text>().text = "90";

        int currentTimeLeft = int.Parse(m_Time.GetComponentInChildren<Text>().text);
        while (currentTimeLeft >= 0)
        {
            m_Time.GetComponentInChildren<Text>().text = (currentTimeLeft--).ToString();
            yield return new WaitForSeconds(1);
        }

        ScoreManager.performTime = 0;
        StartCoroutine(NextLevel(2f));
    }

    float prevTimeFrame;
    float deltaTimeFrame = 1f;

    void Update()
    {
        ScoreManager.hP = playerDamageHandler.CurrentHealth;

        if (playerDamageHandler.CurrentHealth <= 0f)
        {
            ScoreManager.hP = 0f;
            ScoreManager.performTime = Time.time;
            StartCoroutine(NextLevel(2f));
        }
        if (Time.time - prevTimeFrame > deltaTimeFrame)
        {
            prevTimeFrame = Time.time;
        }
    }

    void OnKillMonster()
    {
        ScoreManager.killedCount++;

        if (zombie >= 0 && randomZombie >= 0 && randomZombie == ScoreManager.killedCount)
        {
            if (GameManager.Instance.persistantData.plantsNum.Contains(zombie) == false)
            {
                GameObject notify = Instantiate(Resources.Load("Prefabs/Notify_Plants") as GameObject);
                notify.transform.SetParent(m_NotifyParent, false);
                notify.transform.GetChild(0).GetComponent<Image>().sprite = m_PlantImages[zombie];
                notify.transform.GetChild(1).GetComponent<Text>().text = "You've collected " + m_PlantNames[zombie];
                Destroy(notify, 5);

                GameManager.Instance.persistantData.plantsNum.Add(zombie);
                string dataAsJson = JsonUtility.ToJson(GameManager.Instance.persistantData);
                FileTool.createORwriteFile("Data", dataAsJson);
            }
        }

        if (ScoreManager.killedCount == missionKills && boss >= 0)
        {
            if (GameManager.Instance.persistantData.plantsNum.Contains(boss) == false)
            {
                GameObject notify = Instantiate(Resources.Load("Prefabs/Notify_Plants") as GameObject);
                notify.transform.SetParent(m_NotifyParent, false);
                notify.transform.GetChild(0).GetComponent<Image>().sprite = m_PlantImages[boss];
                notify.transform.GetChild(1).GetComponent<Text>().text = "You've collected " + m_PlantNames[boss];
                Destroy(notify, 5);

                GameManager.Instance.persistantData.plantsNum.Add(boss);
                string dataAsJson = JsonUtility.ToJson(GameManager.Instance.persistantData);
                FileTool.createORwriteFile("Data", dataAsJson);
            }
        }

        m_Score.GetComponentInChildren<Text>().text = ScoreManager.killedCount.ToString();
        if (curMission.Type == "Campaign")
        {
            if (missionKills <= ScoreManager.killedCount)
            {
                ScoreManager.performTime = Time.time - ScoreManager.performTime;
                StartCoroutine(NextLevel(2f));
            }
        }
        else if (curMission.Type == "Time Limit")
        {
            if (curMission.Name == "Easy")
            {
                if (ScoreManager.killedCount >= 15)
                {
                    ScoreManager.performTime = Time.time - ScoreManager.performTime;
                    StartCoroutine(NextLevel(2f));
                }
            }
            else if (curMission.Name == "Medium")
            {
                if (ScoreManager.killedCount == 40)
                {
                    ScoreManager.performTime = Time.time - ScoreManager.performTime;
                    StartCoroutine(NextLevel(2f));
                }
            }
            else if (curMission.Name == "Hard")
            {
                if (ScoreManager.killedCount == 80)
                {
                    ScoreManager.performTime = Time.time - ScoreManager.performTime;
                    StartCoroutine(NextLevel(2f));
                }
            }
        }

        if (Time.time > killSTime + KILL_TIME_INTERVAL)
            killsCount = 0;

        if (killsCount == 0)
            killSTime = Time.time;

        killsCount++;

        if (killsCount > 1)
            multiKillArt.PopArt(killsCount);

        if (curMission.Type == "Campaign")
        {
            if (killsCount == 5)
                GameInfo.playerCoin += 200;

            if (killsCount == 6)
                GameInfo.playerCoin += 500;

            GameInfo.SavePlayerCoin();
        }
    }

    void OnTakeHeadShot(Transform parent)
    {
        ScoreManager.headshotCount++;
        uiMan.PopUpHeadshotArt(parent);
    }

    void OnTakeBodyShot(Transform parent)
    {
        uiMan.PopUpBodyshotArt(parent);
    }

    IEnumerator NextLevel(float delay_time)
    {
        yield return new WaitForSeconds(delay_time);

        if (webCamControl.rearWebcamTexture != null)
            webCamControl.rearWebcamTexture.Stop();

        if (curMission.Type == "Survival")
        {
            int score = ScoreManager.killedCount;

            if (score > GameManager.Instance.persistantData.highScore)
            {
                GameManager.Instance.persistantData.highScore = score;
                string dataAsJson = JsonUtility.ToJson(GameManager.Instance.persistantData);
                FileTool.createORwriteFile("Data", dataAsJson);
            }

            Debug.Log(GameManager.Instance.persistantData.highScore);
        }

        PlayerPrefs.SetFloat("CurrentArmor", playerDamageHandler.currentArmor);
        ScreenFader fadrScreen = FindObjectOfType<ScreenFader>();
        GameSetting.lastSceneIndex = 2;
        fadrScreen.EndScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    IEnumerator DoPickupItem(GameObject item, float duration)
    {
        bool score_flag = false;
        string item_label = item.name;
        if (item.tag == "NoScore")
        {
        }
        else
        {
            score_flag = true;
        }
        vp_ItemPickup pickup = item.GetComponent<vp_ItemPickup>();
        vp_HealthPowerup healthup = item.GetComponent<vp_HealthPowerup>();
        Collider col = hero.GetComponent<Collider>();
        if (pickup != null)
        {
            pickup.TryGiveTo(col);
        }
        else if (healthup != null)
        {
            Vector3 pos = hero.transform.position;
            hero.transform.position = item.transform.position;
            yield return new WaitForSeconds(duration);
            hero.transform.position = pos;
        }

        if (score_flag.Equals(true))
        {
            TargetManager.Instance.ProcItemMessage(TargetMessages.TARGET_DISAPPEARED, item);
            ScoreManager.pickupCount++;

            if (item_label.Substring(0, 1).Equals("i"))
            {
                string str = item_label.Substring(1);
                ItemData ida = ItemData.GetDataFrom("COIN_BAG");
                if (ida.id == int.Parse(str))
                {
                    GameInfo.playerCoin += (int)ida.Price;
                    GameInfo.SavePlayerCoin();
                }
                else
                {
                    ItemInventory.Append(int.Parse(str));
                    ItemInventory.Save();
                }
            }
            else
            {
                string str = item_label.Substring(1);
                WeaponInventory.Append(int.Parse(str));
                WeaponInventory.Save();
            }
        }
    }
}