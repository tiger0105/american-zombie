using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Polaris.GameData;

public enum RadarMapAlignments{
	NONE,
	SMALL_LEFT_TOP,
	LARGE_CENTER
}

public class RadarBehavior : MonoBehaviour
{
    public static RadarBehavior instance;
    public GameObject scanBar;
    public GameObject radarMap;
    public GameObject smallRadarMapTrans;
    public GameObject largeRadarMapTrans;
    public GameObject smallRadarMapButton;
    public GameObject TimerObj;
    public GameObject informTextObj;
    public GameObject largeModeExitButton;
    public float deltaAngle = 1f;

    public float scale;
    public GameObject spotContainer;
    public GameObject spotPrefab;
    int radarRadiusInPixel;

    public Text timeText;
    public Text MissionInstruction;
    public Text distanceText;
    public ArrayList monsterSpotList;
    public ArrayList weaponSpotList;
    public ArrayList itemSpotList;
    TargetManager targetMan;
    RadarMapAlignments align;
    bool ARModeFlag;
    bool triggerFlag;
    float timeStarted;
    int timeLimit = 300;
    TimeSpan timeSpent;
    bool allTargetsScanedFlag;
    public GameObject playerBase;
    float itemFoundDistance = 50f;
    float shortestDistance = 10000f;
    GameObject currentSelectedItem = null;

    public void onRadarSmallMapClick()
    {
        ShowRadarMap(RadarMapAlignments.LARGE_CENTER);
    }
    public void onRadarEnlargeExitButtonClick()
    {
        ShowRadarMap(RadarMapAlignments.SMALL_LEFT_TOP);
    }

    public void onRadarSpotClick(GameObject spot)
    {
        currentSelectedItem = spot;
    }

    public bool AllTargetsScanedFlag
    {
        get { return allTargetsScanedFlag; }
        set { allTargetsScanedFlag = value; }
    }

    public RadarMapAlignments Align
    {
        get { return align; }
        set { align = value; }
    }

    public void ARSwitch()
    {
        if (ARModeFlag == false)
        {
            ARModeFlag = true;
        }
        else
        {
            ARModeFlag = false;
        }
    }

    public void ShowRadarMap(RadarMapAlignments align)
    {
        Align = align;
        switch (Align)
        {
            case RadarMapAlignments.SMALL_LEFT_TOP:
                radarMap.SetActive(true);
                radarMap.GetComponent<RectTransform>().offsetMin = smallRadarMapTrans.GetComponent<RectTransform>().offsetMin;
                radarMap.GetComponent<RectTransform>().offsetMax = smallRadarMapTrans.GetComponent<RectTransform>().offsetMax;
                radarMap.GetComponent<RectTransform>().anchorMax = smallRadarMapTrans.GetComponent<RectTransform>().anchorMax;
                radarMap.GetComponent<RectTransform>().anchorMin = smallRadarMapTrans.GetComponent<RectTransform>().anchorMin;
                radarMap.transform.localScale = smallRadarMapTrans.transform.localScale;
                radarMap.GetComponent<RectTransform>().pivot = smallRadarMapTrans.GetComponent<RectTransform>().pivot;
                radarMap.GetComponent<RectTransform>().anchoredPosition3D = smallRadarMapTrans.GetComponent<RectTransform>().anchoredPosition3D;
                radarMap.GetComponent<RectTransform>().sizeDelta = smallRadarMapTrans.GetComponent<RectTransform>().sizeDelta;
                smallRadarMapTrans.SetActive(false);
                largeRadarMapTrans.SetActive(false);
                smallRadarMapButton.GetComponent<Image>().raycastTarget = true;
                informTextObj.SetActive(false);
                largeModeExitButton.SetActive(false);
                MissionData md = MissionProgress.GetCurMissionData();
                if (md.performTime > 0f)
                {
                    TimerObj.SetActive(true);
                }
                else
                {
                    TimerObj.SetActive(false);
                }
                break;
            case RadarMapAlignments.LARGE_CENTER:
                radarMap.SetActive(true);
                radarMap.GetComponent<RectTransform>().offsetMin = largeRadarMapTrans.GetComponent<RectTransform>().offsetMin;
                radarMap.GetComponent<RectTransform>().offsetMax = largeRadarMapTrans.GetComponent<RectTransform>().offsetMax;
                radarMap.GetComponent<RectTransform>().anchorMax = largeRadarMapTrans.GetComponent<RectTransform>().anchorMax;
                radarMap.GetComponent<RectTransform>().anchorMin = largeRadarMapTrans.GetComponent<RectTransform>().anchorMin;
                radarMap.transform.localScale = largeRadarMapTrans.transform.localScale;
                radarMap.GetComponent<RectTransform>().pivot = largeRadarMapTrans.GetComponent<RectTransform>().pivot;
                radarMap.transform.GetComponent<RectTransform>().anchoredPosition = largeRadarMapTrans.transform.GetComponent<RectTransform>().anchoredPosition;
                radarMap.GetComponent<RectTransform>().sizeDelta = largeRadarMapTrans.GetComponent<RectTransform>().sizeDelta;
                smallRadarMapTrans.SetActive(false);
                largeRadarMapTrans.SetActive(false);
                smallRadarMapButton.GetComponent<Image>().raycastTarget = false;
                informTextObj.SetActive(true);
                largeModeExitButton.SetActive(true);
                md = MissionProgress.GetCurMissionData();
                if (md.performTime > 0f)
                {
                    TimerObj.SetActive(true);
                }
                else
                {
                    TimerObj.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    public void Init(RadarMapAlignments align)
    {
        timeStarted = Time.time;
        instance = this;
        MissionData md = MissionProgress.GetCurMissionData();
        MissionInstruction.text = md.Description;

        triggerFlag = true;
        ShowRadarMap(align);
        targetMan = GameManager.Instance.GetTargetMan();
    }
    void Update()
    {
        UpdateTime();
        UpdateRadarForward();

        if (GameManager.Instance.GetTargetMan().IsTargetGenerated)
            UpdateTargetPos();
    }

    void UpdateTime()
    {
        MissionData md = MissionProgress.GetCurMissionData();
        if (md.performTime <= 0f)
        {
            return;
        }
        if (GameManager.Instance.CurState == GameStates.PAUSE)
        {
            return;
        }
        float wasting = Time.time - ScoreManager.performTime;
        int left_sec = (int)md.performTime - (int)wasting;
        if (left_sec > 0)
        {
            timeSpent = TimeSpan.FromSeconds(left_sec);
            string str = timeSpent.ToString();
            str = str.Substring(3);
            timeText.text = str;
        }
        else
        {
            timeSpent = TimeSpan.FromSeconds(left_sec);
            string str = timeSpent.ToString();
            str = str.Substring(3);
            timeText.text = str;
        }
    }
    void UpdateTargetPos()
    {
        UpdateMonsterPos();
        UpdateWeaponPos();
        UpdateItemPos();
        UpdateDistance();
    }
    void UpdateMonsterPos()
    {
        radarRadiusInPixel = (int)radarMap.GetComponent<RectTransform>().sizeDelta.x / 2;
        scale = (float)radarRadiusInPixel / ((float)Target.max_range_radius / Target.groundResoulution);
        if (monsterSpotList == null)
        {
            monsterSpotList = new ArrayList();
            for (int i = 0; i < targetMan.monsterList.Count; i++)
            {
                Monster monster = (Monster)targetMan.monsterList[i];
                monster.pixelX = (int)monster.virtualObject.transform.position.x;
                monster.pixelY = (int)monster.virtualObject.transform.position.z;
                float deltaX = monster.realObject.transform.position.x - targetMan.player.realObject.transform.position.x;
                float deltaY = monster.realObject.transform.position.z - targetMan.player.realObject.transform.position.z;
                Vector3 devi = new Vector3(deltaX, deltaY, 0);
                GameObject monsterSpot = GameObject.Instantiate(spotPrefab);
                monsterSpot.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 160);
                MonsterData md = MonsterData.GetMonsterData(monster.Type);
                string path = md.Image;
                monsterSpot.name = monster.Type;
                monsterSpot.tag = "monster spot";
                monsterSpot.transform.SetParent(spotContainer.transform);
                monsterSpot.transform.localScale = Vector3.one;
                devi = scale * devi;
                monsterSpot.GetComponent<RectTransform>().anchoredPosition3D = devi;
                monsterSpotList.Add(monsterSpot);
            }
        }
        else
        {
            for (int i = 0; i < targetMan.monsterList.Count; i++)
            {
                Monster monster = (Monster)targetMan.monsterList[i];
                if (monster.realObject.GetComponent<MonsterAI>().IsDead == true)
                {
                    GameObject monsterSpot = (GameObject)monsterSpotList[i];
                    monsterSpot.SetActive(false);
                }
                else
                {
                    monster.pixelX = (int)monster.virtualObject.transform.position.x;
                    monster.pixelY = (int)monster.virtualObject.transform.position.z;

                    float deltaX = monster.realObject.transform.position.x - targetMan.player.realObject.transform.position.x;
                    float deltaY = monster.realObject.transform.position.z - targetMan.player.realObject.transform.position.z;
                    Vector3 devi = new Vector3(deltaX, deltaY, 0);

                    devi = scale * devi;
                    GameObject monsterSpot = (GameObject)monsterSpotList[i];
                    monsterSpot.GetComponent<RectTransform>().anchoredPosition3D = devi;
                }
            }
        }
    }

    void UpdateWeaponPos()
    {
        radarRadiusInPixel = (int)radarMap.GetComponent<RectTransform>().sizeDelta.x / 2;
        scale = (float)radarRadiusInPixel / ((float)Target.max_range_radius / Target.groundResoulution);

        if (weaponSpotList == null)
        {
            weaponSpotList = new ArrayList();
            for (int i = 0; i < targetMan.weaponList.Count; i++)
            {
                Weapon weapon = (Weapon)targetMan.weaponList[i];

                float deltaX = weapon.pixelX - targetMan.player.pixelX;
                float deltaY = weapon.pixelY - targetMan.player.pixelY;
                Vector3 devi = new Vector3(deltaX, deltaY, 0);
                GameObject obj = GameObject.Instantiate(spotPrefab);
                obj.GetComponent<RectTransform>().sizeDelta = Vector2.one * 30f;
                WeaponData wd = WeaponData.GetWeaponData(weapon.Type);
                string path = wd.Image;
                obj.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
                obj.tag = "weapon spot";
                obj.transform.SetParent(spotContainer.transform);
                obj.transform.localScale = Vector3.one;
                devi = scale * devi;
                obj.GetComponent<RectTransform>().anchoredPosition3D = devi;
                weaponSpotList.Add(obj);
            }
        }
        else
        {
            for (int i = 0; i < targetMan.weaponList.Count; i++)
            {
                GameObject spot;
                Weapon weapon = (Weapon)targetMan.weaponList[i];
                if (weapon.IsDisappeared)
                {
                    spot = (GameObject)weaponSpotList[i];
                    spot.SetActive(false);
                    return;
                }
                float deltaX = weapon.pixelX - targetMan.player.pixelX;
                float deltaY = weapon.pixelY - targetMan.player.pixelY;
                Vector3 devi = new Vector3(deltaX, deltaY, 0);
                float dis = Vector3.Magnitude(devi);
                dis = dis * Target.groundResoulution;

                devi = scale * devi;
                spot = (GameObject)weaponSpotList[i];
                spot.GetComponent<RectTransform>().anchoredPosition3D = devi;

                if (dis < shortestDistance)
                    shortestDistance = dis;
            }
        }
    }

    void UpdateItemPos()
    {
        radarRadiusInPixel = (int)radarMap.GetComponent<RectTransform>().sizeDelta.x / 2;
        scale = (float)radarRadiusInPixel / ((float)Target.max_range_radius / Target.groundResoulution);

        if (itemSpotList == null)
        {
            itemSpotList = new ArrayList();
            for (int i = 0; i < targetMan.itemList.Count; i++)
            {
                Item item = (Item)targetMan.itemList[i];

                float deltaX = item.pixelX - targetMan.player.pixelX;
                float deltaY = item.pixelY - targetMan.player.pixelY;
                Vector3 devi = new Vector3(deltaX, deltaY, 0);

                GameObject obj = GameObject.Instantiate(spotPrefab);
                obj.GetComponent<RectTransform>().sizeDelta = Vector2.one * 30f;
                ItemData data = ItemData.GetDataFrom(item.Type);
                string path = data.Image;
                obj.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
                obj.tag = "item spot";
                obj.transform.SetParent(spotContainer.transform);
                obj.transform.localScale = Vector3.one;
                devi = scale * devi;
                obj.GetComponent<RectTransform>().anchoredPosition3D = devi;
                itemSpotList.Add(obj);
            }
        }
        else
        {
            for (int i = 0; i < targetMan.itemList.Count; i++)
            {
                GameObject spot;
                Item item = (Item)targetMan.itemList[i];
                if (item.IsDisappeared)
                {
                    spot = (GameObject)itemSpotList[i];
                    spot.SetActive(false);
                    return;
                }
                float deltaX = item.pixelX - targetMan.player.pixelX;
                float deltaY = item.pixelY - targetMan.player.pixelY;
                Vector3 devi = new Vector3(deltaX, deltaY, 0);
                float dis = Vector3.Magnitude(devi);
                dis = dis * Target.groundResoulution;

                devi = scale * devi;
                spot = (GameObject)itemSpotList[i];
                spot.GetComponent<RectTransform>().anchoredPosition3D = devi;

                if (dis < shortestDistance)
                    shortestDistance = dis;
            }
        }
    }
    void UpdateCompass()
    {
        scanBar.transform.Rotate(Vector3.forward, deltaAngle, Space.Self);
    }
    void UpdateRadarForward()
    {
        float angle = playerBase.transform.rotation.eulerAngles.y;
        spotContainer.transform.localEulerAngles = new Vector3(0f, 0f, angle);
    }
    void UpdateShortestDistance()
    {
        distanceText.text = string.Format("dis:{0}m", (int)shortestDistance);
    }

    void UpdateDistance()
    {
        if (currentSelectedItem == null)
            return;

        if (currentSelectedItem.tag == "weapon spot")
        {
            for (int i = 0; i < weaponSpotList.Count; i++)
                if (currentSelectedItem.Equals(weaponSpotList[i]))
                {
                    Weapon weapon = (Weapon)GameManager.Instance.GetTargetMan().weaponList[i];
                    float deltaX = weapon.pixelX - targetMan.player.pixelX;
                    float deltaY = weapon.pixelY - targetMan.player.pixelY;
                    Vector3 devi = new Vector3(deltaX, deltaY, 0);
                    float dis = Vector3.Magnitude(devi);

                    dis = dis * Target.groundResoulution;
                    distanceText.text = dis + "m";
                    return;
                }
        }
        else if (currentSelectedItem.tag == "item spot")
        {
            for (int i = 0; i < RadarBehavior.instance.itemSpotList.Count; i++)
            {
                if (currentSelectedItem.Equals(RadarBehavior.instance.itemSpotList[i]))
                {
                    Item item = (Item)GameManager.Instance.GetTargetMan().itemList[i];
                    float deltaX = item.pixelX - targetMan.player.pixelX;
                    float deltaY = item.pixelY - targetMan.player.pixelY;
                    Vector3 devi = new Vector3(deltaX, deltaY, 0);
                    float dis = Vector3.Magnitude(devi);

                    dis = dis * Target.groundResoulution;
                    distanceText.text = dis + "m";
                    return;
                }
            }
        }
    }
}