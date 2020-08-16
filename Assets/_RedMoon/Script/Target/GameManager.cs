using System.Collections.Generic;
using UnityEngine;
using Polaris.GameData;
using Polaris.Base;
using UnityEngine.SceneManagement;

public class GameSetting
{
    public static bool isTestMode = false;
    public static bool isLimitMode = false;
    public static bool musicOn = true;
    public static bool soundOn = true;
    public static bool nameOn = true;
    public static int handShade = 0;
    public static int lastSceneIndex = -1;
    public static bool showedOfferDialog = false;
}
public class GameInfo
{
    public static string playerName;
    public static int playerLevel;
    public static int playerGold;
    public static int playerCoin;
    public static int playCounts;
    //to save and load the long type valiable
    public static string lastEnergyIO_TimeTicksString;
    public static string nowTimeTicksString;

    public static int heartLives;

    public static int playCountLimits = 0;
    public static int maxLevel = 4;
    public static int HeartLivesMax = 5;
    public static long OneHeartChargeTimeTicks = 3000000000; // that ticks means 5min :5*60*10,000,000 seconds
    public static int maxMission = 50;
    public static int minLevel = 0;
    public static int minMission = 1;
    public static float minDifficulty = 0.5f;
    public static float maxDifficulty = 1.0f;
    public static float maxMissionTime = 300f;

    public static float upgradeRate = 0.1f;
    public static float upgradeDifficulty = 10f;
    public static float priceRate = 5f; //need 5 coins for increasing 1 power.
    public static float powerRate = 100f;   // damage to power
    public static float maxPowerRate = 13.3f;

    public static int killBonus = 10;
    public static int headshotBonus = 10;
    public static float fullHpBonus = 1f;
    public static int ontimeBonus = 5;

    public static void LoadPlayCount()
    {
        playCounts = PlayerPrefs.GetInt("PlayCounts", 0);
    }
    public static void SavePlayCount()
    {
        PlayerPrefs.SetInt("PlayCounts", playCounts);
    }
    public static void LoadPlayerName()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Larry");
    }
    public static void SavePlayerName()
    {
        PlayerPrefs.SetString("PlayerName", playerName);
    }
    public static void LoadPlayerLevel()
    {
        playerLevel = PlayerPrefs.GetInt("PlayerLevel", 0);
    }
    public static void SavePlayerLevel()
    {
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
    }
    //energy battery system
    public static void LoadHeartLives()
    {
        heartLives = PlayerPrefs.GetInt("HeartLives", 5);
    }
    public static void SaveHeartLives()
    {
        PlayerPrefs.SetInt("HeartLives", heartLives);
    }
    public static void LoadLastEnergyIoTimeTicksString()
    {
        lastEnergyIO_TimeTicksString = PlayerPrefs.GetString("lastEnergyIO_TimeTicksString", "0");
    }
    public static void SaveLastEnergyIoTimeTicksString()
    {
        PlayerPrefs.SetString("lastEnergyIO_TimeTicksString", lastEnergyIO_TimeTicksString);
    }

    public static void LoadPlayerCoin()
    {
        if (GameSetting.isTestMode == true)
        {
            playerCoin = 999999;
        }
        else
        {
            playerCoin = PlayerPrefs.GetInt("PlayerCoin", 0);
        }

        /// DELETE IT
        //playerCoin = 999999;
    }
    public static void SavePlayerCoin()
    {
        PlayerPrefs.SetInt("PlayerCoin", playerCoin);
    }

    public static void LoadPlayerGold()
    {
        if (GameSetting.isTestMode == true)
        {
            playerGold = 999999;
        }
        else
        {
            playerGold = PlayerPrefs.GetInt("PlayerGold", 0);
        }

        /// DELETE IT
        //playerGold = 999999;
    }
    public static void SavePlayerGold()
    {
        PlayerPrefs.SetInt("PlayerGold", playerGold);
    }

    public static void Save()
    {
        SavePlayCount();
        SavePlayerName();
        SavePlayerCoin();
        SavePlayerGold();
        SavePlayerLevel();
        SaveHeartLives();
        SaveLastEnergyIoTimeTicksString();
    }
    public static void Load()
    {
        LoadPlayCount();
        LoadPlayerCoin();
        LoadPlayerGold();
        LoadPlayerLevel();
        LoadPlayerName();
        LoadLastEnergyIoTimeTicksString();
        LoadHeartLives();
    }
    public static double curMobileLongitude = 129d;
    public static double curMobileLatitude = 37.1354d;

}

public class MissionProgress
{
    //variable depending on player progress
    private static List<int> missionCurIndexList;
    private static List<int> missionChallengeIndexList;
    private static int curMissionType;
    private static int curMissionIndex; //matching with <index_i> tag on MissionData.xml
    private static bool isLoaded = false;
    public static bool IsLoaded
    {
        get { return isLoaded; }
        set { isLoaded = value; }
    }
    // public interface
    public static void LoadMissionCurIndexList()
    {
        if (missionChallengeIndexList == null)
        {
            LoadMissionChallengeIndexList();
        }
        missionCurIndexList = new List<int>();
        for (int i = 0; i < missionChallengeIndexList.Count; i++)
        {
            int challenge_index = missionChallengeIndexList[i];
            missionCurIndexList.Add(challenge_index);
        }
    }
    public static void LoadMissionChallengeIndexList()
    {
        string str = PlayerPrefs.GetString("missionChallengeIndexList", string.Empty);
        if (str.Equals(string.Empty))
        {
            int mission_types = MissionData.GetMissionTypeCount();
            missionChallengeIndexList = new List<int>();
            for (int i = 0; i < mission_types; i++)
            {
                missionChallengeIndexList.Add(0);
            }
        }
        else
        {
            string[] strArray = str.Split(' ');
            int mission_types = MissionData.GetMissionTypeCount();
            missionChallengeIndexList = new List<int>();
            for (int i = 0; i < strArray.Length; i++)
            {
                int id = int.Parse(strArray[i]);
                missionChallengeIndexList.Add(id);
            }
            for (int i = strArray.Length; i < mission_types; i++)
            {
                missionChallengeIndexList.Add(0);
            }
        }
        LoadMissionCurIndexList();
    }
    public static void SaveMissionChallengeIndexList()
    {
        string str = string.Empty;
        for (int i = 0; i < missionChallengeIndexList.Count; i++)
        {
            str += missionChallengeIndexList[i].ToString();
            str += " ";
        }
        str = str.Trim();
        PlayerPrefs.SetString("missionChallengeIndexList", str);
    }

    public static void Load()
    {
        LoadMissionChallengeIndexList();
        isLoaded = true;
    }
    public static void Save()
    {
        SaveMissionChallengeIndexList();
    }
    public static int GetMissionCurIndex(int type)
    {
        if (isLoaded.Equals(false))
            return -1;
        else if (type < 0 || type >= MissionData.GetMissionTypeCount())
        {
            return -1;
        }
        else
        {
            curMissionType = type;
            curMissionIndex = missionCurIndexList[type];
            return curMissionIndex;
        }
    }
    public static int GetMissionCurIndex()
    {
        int type = GetCurMissionType();
        int index = GetMissionCurIndex(type);
        return index;
    }
    public static int GetMissionChallengeIndex(int type)
    {
        if (isLoaded.Equals(false))
            return -1;
        else if (type < 0 || type >= MissionData.GetMissionTypeCount())
        {
            return -1;
        }
        else
        {
            return missionChallengeIndexList[type];
        }
    }
    public static int SetMissionCurIndex(int type, int index)
    {
        if (index < 0)
        {
            index = MissionData.GetMissionCountList()[type] - 1;
        }
        else if (index >= MissionData.GetMissionCountList()[type])
        {
            index = 0;
        }
        if (GameSetting.isTestMode == true)
        {
            missionCurIndexList.RemoveAt(type);
            missionCurIndexList.Insert(type, index);
        }
        else
        {
            if (index <= 25)
            {
                missionCurIndexList.RemoveAt(type);
                missionCurIndexList.Insert(type, index);
            }
        }
        curMissionType = type;
        curMissionIndex = index;
        return index;
    }
    public static void SetMissionChallengeIndex(int type, int index)
    {
        if (index < 0)
        {
            index = 0;
        }
        else if (index >= MissionData.GetMissionCountList()[type])
        {
            index = MissionData.GetMissionCountList()[type] - 1;
        }
        missionChallengeIndexList.RemoveAt(type);
        missionChallengeIndexList.Insert(type, index);
        SaveMissionChallengeIndexList();
    }
    public static MissionData GetCurMissionData()
    {
        for (int i = 0; i < MissionData.dataMap.Count; i++)
        {
            MissionData md = MissionData.dataMap[i];
            string str = MissionData.GetMissionTypeList()[curMissionType];
            curMissionIndex = missionCurIndexList[curMissionType];
            if (md.Type.Equals(str) && md.Index.Equals(curMissionIndex))
            {
                return md;
            }
        }
        return null;
    }

    public static int GetCurMissionType()
    {
        return curMissionType;
    }
    public static void CurMissionReset()
    {
        if (missionChallengeIndexList == null)
        {
            LoadMissionChallengeIndexList();
        }
        if (missionCurIndexList == null)
        {
            missionCurIndexList = new List<int>();
        }
        missionCurIndexList.Clear();
        for (int i = 0; i < missionChallengeIndexList.Count; i++)
        {
            int chall_index = missionChallengeIndexList[i];
            missionCurIndexList.Add(chall_index);
        }
        curMissionType = 0;
        curMissionIndex = missionCurIndexList[curMissionType];
    }
}
public class WeaponInventory
{
    public static string idStr; //"1 3 4"
    public static string equipStateStr; //"0 1 1"
    public static string powerStr; //"1 2 2"
    //public static string damageStr;
    //public static string fireRateStr;
    //public static string attackRangeStr;
    //public static string reloadSpeedStr;
    //public static string ammoCapacityStr;
    public static string levelStr;
    public static List<int> idList;
    public static List<int> equipeList;
    public static List<int> powerList;
    //public static List<int> damageList;
    //public static List<float> fireRateList;
    //public static List<int> attackRangeList;
    //public static List<float> reloadSpeedList;
    //public static List<int> ammoCapacityList;
    public static List<int> levelList;

    public static void LoadIdStr()
    {
        idStr = PlayerPrefs.GetString("Weapon_IdStr", "0");
        idStr.Trim();
        string[] idStrArray = idStr.Split();
        idList = new List<int>();
        if (idStrArray[0].Equals(string.Empty))
        {
            idList.Clear();
        }
        else
        {
            for (int i = 0; i < idStrArray.Length; i++)
            {
                int item = int.Parse(idStrArray[i]);
                idList.Add(item);
            }
        }
    }
    public static void SaveIdStr()
    {
        if (idList.Count.Equals(0))
        {
            idStr = string.Empty;
        }
        else
        {
            string str = string.Empty;
            for (int i = 0; i < idList.Count; i++)
            {
                str += idList[i].ToString();
                str += " ";
            }
            idStr = str.Trim();
        }
        PlayerPrefs.SetString("Weapon_IdStr", idStr);
    }
    public static void LoadEquipeStateStr()
    {
        equipStateStr = PlayerPrefs.GetString("Weapon_EquipeStateStr", "1");
        equipStateStr.Trim();
        string[] equipeStateStrArray = equipStateStr.Split();
        equipeList = new List<int>();
        if (equipeStateStrArray[0].Equals(string.Empty))
        {
            equipeList.Clear();
        }
        else
        {
            for (int i = 0; i < equipeStateStrArray.Length; i++)
            {
                int item = int.Parse(equipeStateStrArray[i]);
                equipeList.Add(item);
            }
        }
    }
    public static void SaveEquipeStateStr()
    {
        if (equipeList.Count.Equals(0))
        {
            equipStateStr = string.Empty;
        }
        else
        {
            string str = string.Empty;
            for (int i = 0; i < equipeList.Count; i++)
            {
                str += equipeList[i].ToString();
                str += " ";
            }
            equipStateStr = str.Trim();
        }
        PlayerPrefs.SetString("Weapon_EquipeStateStr", equipStateStr);
    }
    public static void LoadpowerStr()
    {
        powerStr = PlayerPrefs.GetString("Weapon_PowerStr", "250"); //defalt pistol power.
        powerStr.Trim();
        string[] powerStrArray = powerStr.Split();
        powerList = new List<int>();
        if (powerStrArray[0].Equals(string.Empty))
        {
            powerList.Clear();
        }
        else
        {
            for (int i = 0; i < powerStrArray.Length; i++)
            {
                int item = int.Parse(powerStrArray[i]);
                powerList.Add(item);
            }
        }
    }
    public static void SavepowerStr()
    {
        if (powerList.Count.Equals(0))
        {
            powerStr = string.Empty;
        }
        else
        {
            string str = string.Empty;
            for (int i = 0; i < powerList.Count; i++)
            {
                str += powerList[i].ToString();
                str += " ";
            }
            powerStr = str.Trim();
        }
        PlayerPrefs.SetString("Weapon_PowerStr", powerStr);
    }
    //public static void LoadDamageStr()
    //{
    //    damageStr = PlayerPrefs.GetString("Weapon_DamageStr", "2");
    //    damageStr.Trim();
    //    string[] damageStrArray = damageStr.Split();
    //    damageList = new List<int>();
    //    if (damageStrArray[0].Equals(string.Empty))
    //    {
    //        damageList.Clear();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < damageStrArray.Length; i++)
    //        {
    //            int item = int.Parse(damageStrArray[i]);
    //            damageList.Add(item);
    //        }
    //    }
    //}
    //public static void SaveDamageStr()
    //{
    //    if (damageList.Count.Equals(0))
    //    {
    //        damageStr = string.Empty;
    //    }
    //    else
    //    {
    //        string str = string.Empty;
    //        for (int i = 0; i < damageList.Count; i++)
    //        {
    //            str += damageList[i].ToString();
    //            str += " ";
    //        }
    //        damageStr = str.Trim();
    //    }
    //    PlayerPrefs.SetString("Weapon_DamageStr", damageStr);
    //}
    //public static void LoadFireRateStr()
    //{
    //    fireRateStr = PlayerPrefs.GetString("Weapon_FireRateStr", "2");
    //    fireRateStr.Trim();
    //    string[] fireRateStrArray = fireRateStr.Split();
    //    fireRateList = new List<float>();
    //    if (fireRateStrArray[0].Equals(string.Empty))
    //    {
    //        fireRateList.Clear();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < fireRateStrArray.Length; i++)
    //        {
    //            float item = float.Parse(fireRateStrArray[i]);
    //            fireRateList.Add(item);
    //        }
    //    }
    //}
    //public static void SaveFireRateStr()
    //{
    //    if (fireRateList.Count.Equals(0))
    //    {
    //        fireRateStr = string.Empty;
    //    }
    //    else
    //    {
    //        string str = string.Empty;
    //        for (int i = 0; i < fireRateList.Count; i++)
    //        {
    //            str += fireRateList[i].ToString();
    //            str += " ";
    //        }
    //        fireRateStr = str.Trim();
    //    }
    //    PlayerPrefs.SetString("Weapon_FireRateStr", fireRateStr);
    //}
    //public static void LoadAttackRangeStr()
    //{
    //    attackRangeStr = PlayerPrefs.GetString("Weapon_AttackRangeStr", "100");
    //    attackRangeStr.Trim();
    //    string[] attackRangeStrArray = attackRangeStr.Split();
    //    attackRangeList = new List<int>();
    //    if (attackRangeStrArray[0].Equals(string.Empty))
    //    {
    //        attackRangeList.Clear();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < attackRangeStrArray.Length; i++)
    //        {
    //            int item = int.Parse(attackRangeStrArray[i]);
    //            attackRangeList.Add(item);
    //        }
    //    }
    //}
    //public static void SaveAttackRangeStr()
    //{
    //    if (attackRangeList.Count.Equals(0))
    //    {
    //        attackRangeStr = string.Empty;
    //    }
    //    else
    //    {
    //        string str = string.Empty;
    //        for (int i = 0; i < attackRangeList.Count; i++)
    //        {
    //            str += attackRangeList[i].ToString();
    //            str += " ";
    //        }
    //        attackRangeStr = str.Trim();
    //    }
    //    PlayerPrefs.SetString("Weapon_AttackRangeStr", attackRangeStr);
    //}
    //public static void LoadReloadSpeedStr()
    //{
    //    reloadSpeedStr = PlayerPrefs.GetString("Weapon_ReloadSpeedStr", "2");
    //    reloadSpeedStr.Trim();
    //    string[] reloadSpeedStrArray = reloadSpeedStr.Split();
    //    reloadSpeedList = new List<float>();
    //    if (reloadSpeedStrArray[0].Equals(string.Empty))
    //    {
    //        reloadSpeedList.Clear();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < reloadSpeedStrArray.Length; i++)
    //        {
    //            float item = float.Parse(reloadSpeedStrArray[i]);
    //            reloadSpeedList.Add(item);
    //        }
    //    }
    //}
    //public static void SaveReloadSpeedStr()
    //{
    //    if (reloadSpeedList.Count.Equals(0))
    //    {
    //        reloadSpeedStr = string.Empty;
    //    }
    //    else
    //    {
    //        string str = string.Empty;
    //        for (int i = 0; i < reloadSpeedList.Count; i++)
    //        {
    //            str += reloadSpeedList[i].ToString();
    //            str += " ";
    //        }
    //        reloadSpeedStr = str.Trim();
    //    }
    //    PlayerPrefs.SetString("Weapon_ReloadSpeedStr", reloadSpeedStr);
    //}
    //public static void LoadAmmoCapacityStr()
    //{
    //    ammoCapacityStr = PlayerPrefs.GetString("Weapon_AmmoCapacityStr", "20");
    //    ammoCapacityStr.Trim();
    //    string[] ammoCapacityStrArray = ammoCapacityStr.Split();
    //    ammoCapacityList = new List<int>();
    //    if (ammoCapacityStrArray[0].Equals(string.Empty))
    //    {
    //        ammoCapacityList.Clear();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < ammoCapacityStrArray.Length; i++)
    //        {
    //            int item = int.Parse(ammoCapacityStrArray[i]);
    //            ammoCapacityList.Add(item);
    //        }
    //    }
    //}
    //public static void SaveAmmoCapacityStr()
    //{
    //    if (ammoCapacityList.Count.Equals(0))
    //    {
    //        ammoCapacityStr = string.Empty;
    //    }
    //    else
    //    {
    //        string str = string.Empty;
    //        for (int i = 0; i < ammoCapacityList.Count; i++)
    //        {
    //            str += ammoCapacityList[i].ToString();
    //            str += " ";
    //        }
    //        ammoCapacityStr = str.Trim();
    //    }
    //    PlayerPrefs.SetString("Weapon_AmmoCapacityStr", ammoCapacityStr);
    //}
    public static void LoadLevelStr()
    {
        levelStr = PlayerPrefs.GetString("Weapon_LevelStr", "1");
        levelStr.Trim();
        string[] levelStrArray = levelStr.Split();
        levelList = new List<int>();
        if (levelStrArray[0].Equals(string.Empty))
        {
            levelList.Clear();
        }
        else
        {
            for (int i = 0; i < levelStrArray.Length; i++)
            {
                int item = int.Parse(levelStrArray[i]);
                levelList.Add(item);
            }
        }
    }
    public static void SaveLevelStr()
    {
        if (levelList.Count.Equals(0))
        {
            levelStr = string.Empty;
        }
        else
        {
            string str = string.Empty;
            for (int i = 0; i < levelList.Count; i++)
            {
                str += levelList[i].ToString();
                str += " ";
            }
            levelStr = str.Trim();
        }
        PlayerPrefs.SetString("Weapon_LevelStr", levelStr);
    }
    public static int GetInventoryID(int weapon_id)
    {
        for (int i = 0; i < idList.Count; i++)
        {
            if (idList[i].Equals(weapon_id))
            {
                return i;
            }
        }
        return 0;
    }
    public static int GetCount()
    {
        if (idList == null)
            return -1;
        return idList.Count;
    }
    public static int GetWeaponID(int num)
    {
        if (idList == null)
        {
            return -1;
        }
        else if (num < 0 || num >= idList.Count)
        {
            return -1;
        }
        return idList[num];
    }
    public static WeaponData GetWeaponData(int num)
    {
        int weapon_id = GetWeaponID(num);
        WeaponData wd = WeaponData.dataMap[weapon_id + 1];
        return wd;
    }
    public static float GetDamage(int weapon_id)
    {
        int inventory_id = WeaponInventory.GetInventoryID(weapon_id);
        int power = WeaponInventory.powerList[inventory_id];
        WeaponData wd = WeaponData.dataMap[weapon_id + 1];
        float damage = float.Parse(wd.GetProperty("damage"));
        return damage;
    }
    public static void Append(int id)
    {
        if (idList.Contains(id))
        {
            return;
        }
        WeaponData wd = WeaponData.dataMap[id + 1];
        int power = wd.GetMinPower();
        idList.Add(id);
        equipeList.Add(1);
        powerList.Add(power);
        //damageList.Add(wd.GetDamage());
        //fireRateList.Add(wd.GetFireRate());
        //attackRangeList.Add(wd.GetRange());
        //reloadSpeedList.Add(wd.GetReloadSpeed());
        //ammoCapacityList.Add(wd.GetAmmoCapacity());
        levelList.Add(1);
    }
    public static void Load()
    {
        LoadIdStr();
        LoadEquipeStateStr();
        LoadpowerStr();
        //LoadDamageStr();
        //LoadFireRateStr();
        //LoadAttackRangeStr();
        //LoadReloadSpeedStr();
        //LoadAmmoCapacityStr();
        LoadLevelStr();
    }
    public static void Save()
    {
        SaveIdStr();
        SavepowerStr();
        SaveEquipeStateStr();
        //SaveDamageStr();
        //SaveFireRateStr();
        //SaveAttackRangeStr();
        //SaveReloadSpeedStr();
        //SaveAmmoCapacityStr();
        SaveLevelStr();
    }
}

public class ItemInventory
{
    public static string idStr; //"1 3 4"
    public static string equipStateStr; //"0 1 1"
    public static List<int> idList;
    public static List<int> equipeList;
    public static List<int> unitsList;
    public static void LoadIdStr()
    {
        idStr = PlayerPrefs.GetString("Item_IdStr", "");
        idStr.Trim();
        string[] idStrArray = idStr.Split();
        idList = new List<int>();
        if (idStrArray[0].Equals(string.Empty))
        {
            idList.Clear();
        }
        else
        {
            for (int i = 0; i < idStrArray.Length; i++)
            {
                int item = int.Parse(idStrArray[i]);
                idList.Add(item);
            }
        }
    }
    public static void SaveIdStr()
    {
        if (idList.Count.Equals(0))
        {
            idStr = string.Empty;
        }
        else
        {
            string str = string.Empty;
            for (int i = 0; i < idList.Count; i++)
            {
                str += idList[i].ToString();
                str += " ";
            }
            idStr = str.Trim();
        }
        PlayerPrefs.SetString("Item_IdStr", idStr);
    }
    public static void LoadUnitsStr()
    {
        string unitsStr = PlayerPrefs.GetString("Item_UnitsStr", "");
        unitsStr.Trim();
        string[] unitsStrArray = unitsStr.Split();
        unitsList = new List<int>();
        if (unitsStrArray[0].Equals(string.Empty))
        {
            unitsList.Clear();
        }
        else
        {
            for (int i = 0; i < unitsStrArray.Length; i++)
            {
                int item = int.Parse(unitsStrArray[i]);
                unitsList.Add(item);
            }
        }
    }
    public static void SaveUnitsStr()
    {
        string unitsStr;
        if (unitsList.Count.Equals(0))
        {
            unitsStr = string.Empty;
        }
        else
        {
            string str = string.Empty;
            for (int i = 0; i < idList.Count; i++)
            {
                str += idList[i].ToString();
                str += " ";
            }
            unitsStr = str.Trim();
        }
        PlayerPrefs.SetString("Item_UnitsStr", unitsStr);
    }
    public static void LoadEquipeStateStr()
    {
        equipStateStr = PlayerPrefs.GetString("Item_EquipeStateStr", "");
        equipStateStr.Trim();
        string[] equipeStateStrArray = equipStateStr.Split();
        equipeList = new List<int>();
        if (equipeStateStrArray[0].Equals(string.Empty))
        {
            equipeList.Clear();
        }
        else
        {
            for (int i = 0; i < equipeStateStrArray.Length; i++)
            {
                int item = int.Parse(equipeStateStrArray[i]);
                equipeList.Add(item);
            }
        }
    }
    public static void SaveEquipeStateStr()
    {
        if (equipeList.Count.Equals(0))
        {
            equipStateStr = string.Empty;
        }
        else
        {
            string str = string.Empty;
            for (int i = 0; i < equipeList.Count; i++)
            {
                str += equipeList[i].ToString();
                str += " ";
            }
            equipStateStr = str.Trim();
        }
        PlayerPrefs.SetString("Item_EquipeStateStr", equipStateStr);
    }
    public static int GetInventoryID(int item_id)
    {
        for (int i = 0; i < idList.Count; i++)
        {
            if (idList[i].Equals(item_id))
            {
                return i;
            }
        }
        return 0;
    }
    public static int GetCount()
    {
        if (idList == null)
            return -1;
        return idList.Count;
    }
    public static int GetItemID(int num)
    {
        if (idList == null)
        {
            return -1;
        }
        else if (num < 0 || num >= idList.Count)
        {
            return -1;
        }
        return idList[num];
    }
    public static ItemData GetItemData(int num)
    {
        int item_id = GetItemID(num);
        ItemData id = ItemData.dataMap[item_id + 1];
        return id;
    }
    public static void Append(int id)
    {
        ItemData itemData = ItemData.dataMap[id + 1];
        ItemInventory.idList.Add(id);
        ItemInventory.equipeList.Add(1);
    }
    public static void ApplyRemainUnits(int index, int count)
    {
        int item = Mathf.Max(0, count);
        unitsList.Remove(index);
        unitsList.Insert(index, item);
    }
    public static void Load()
    {
        LoadIdStr();
        LoadEquipeStateStr();
        LoadUnitsStr();
    }
    public static void Save()
    {
        SaveIdStr();
        SaveEquipeStateStr();
        SaveUnitsStr();
    }
}

public class GameDataManager
{
    public static void LoadGameData()
    {
        GameInfo.Load();
        WeaponInventory.Load();
        ItemInventory.Load();
        MissionProgress.Load();
    }
    public static void SaveGameData()
    {
        GameInfo.Save();
        WeaponInventory.Save();
        ItemInventory.Save();
        MissionProgress.Save();
    }
    public static void DeleteAllProgressData()
    {
        PlayerPrefs.DeleteAll();
    }
    public static void UpgradeWeapon(int id)
    {
        WeaponData wp = WeaponData.dataMap[id + 1];
        int inventory_id = WeaponInventory.GetInventoryID(id);
        float cur_power = WeaponInventory.powerList[inventory_id];
        int add_price = 0;
        if (wp.Name == "Shotgun")
            add_price = 5000;
        else if (wp.Name == "M16 Carbine Assault Rifle")
            add_price = 9500;
        else if (wp.Name == "M4 Carbon Rifle")
            add_price = 15000;
        else if (wp.Name == "Grenade Launcher")
            add_price = 25000;
        else
            add_price = (int)(cur_power * GameInfo.upgradeRate * GameInfo.priceRate * GameInfo.upgradeDifficulty);
        int add_gold = add_price / 200;
        int newpowerval = (int)(cur_power * (1 + GameInfo.upgradeRate));
        int newLevel = WeaponInventory.levelList[inventory_id] + 1;

        if (GameInfo.playerCoin >= add_price && newLevel <= 2)
        {
            GameInfo.playerCoin -= add_price;
            GameInfo.SavePlayerCoin();
            WeaponInventory.powerList.RemoveAt(inventory_id);
            WeaponInventory.powerList.Insert(inventory_id, newpowerval);
            WeaponInventory.levelList.RemoveAt(inventory_id);
            WeaponInventory.levelList.Insert(inventory_id, newLevel);
            WeaponInventory.Save();
            GameManager.Instance.GetUIMan().itemUpgradeWindow.GetComponent<ItemUpgradeWindow>().OnUpgradeSuccess();
        }
        else if (GameInfo.playerGold >= add_gold && newLevel == 3)
        {
            GameInfo.playerGold -= add_gold;
            GameInfo.SavePlayerGold();
            WeaponInventory.powerList.RemoveAt(inventory_id);
            WeaponInventory.powerList.Insert(inventory_id, newpowerval);
            WeaponInventory.levelList.RemoveAt(inventory_id);
            WeaponInventory.levelList.Insert(inventory_id, newLevel);
            WeaponInventory.Save();
            GameManager.Instance.GetUIMan().itemUpgradeWindow.GetComponent<ItemUpgradeWindow>().OnUpgradeSuccess();
        }
        else
        {
            GameManager.Instance.GetUIMan().itemUpgradeWindow.GetComponent<ItemUpgradeWindow>().OnUpgradeFailed();
        }
    }
    public static void UpgradeSpectacles()
    {
        int upgradeGold = 0;
        int spectaclesLevel = PlayerPrefs.GetInt("SpectaclesLevel", 1);
        if (spectaclesLevel == 1)
            upgradeGold = 20;
        else if (spectaclesLevel == 2)
            upgradeGold = 30;

        if (GameInfo.playerGold >= upgradeGold && spectaclesLevel < 3)
        {
            GameInfo.playerGold -= (int)upgradeGold;
            GameInfo.SavePlayerGold();

            PlayerPrefs.SetInt("SpectaclesLevel", spectaclesLevel + 1);

            GameManager.Instance.GetUIMan().itemUpgradeWindow.GetComponent<ItemUpgradeWindow>().OnUpgradeSuccess();
        }
        else
        {
            GameManager.Instance.GetUIMan().itemUpgradeWindow.GetComponent<ItemUpgradeWindow>().OnUpgradeFailed();
        }
    }
    public static void BuyItem(int id)
    {
        ItemData itemData = ItemData.dataMap[id + 1];
        int price = (int)itemData.Price;
        if (id >= 12 && id < 15)
        {
            if (GameInfo.playerGold >= price)
            {
                if (itemData.Name == "Unbreakable")
                {
                    int unbreakableCount = PlayerPrefs.GetInt("Unbreakable", 0);
                    PlayerPrefs.SetInt("Unbreakable", unbreakableCount + 1);
                }
                else if (itemData.Name == "Double Cash")
                {
                    int doubleCashCount = PlayerPrefs.GetInt("DoubleCash", 0);
                    PlayerPrefs.SetInt("DoubleCash", doubleCashCount + 1);
                }
                else if (itemData.Name == "Maniaco Spectacles")
                {
                    if (ItemInventory.idList.Contains(id))
                    {
                        GameManager.Instance.GetUIMan().Inform(InformTypes.ALREADY_PURCHASE, "Already purchased", "You already purchased spectacles.", itemData.Image);
                        return;
                    }

                    PlayerPrefs.SetInt("Spectacles", 1);
                    PlayerPrefs.SetInt("SpectaclesLevel", 1);

                    ItemInventory.idList.Add(id);
                    ItemInventory.Save();

                    GameInfo.playerGold -= price;
                    GameInfo.SavePlayerGold();

                    GameManager.Instance.GetUIMan().itemBuyWindow.GetComponent<ItemBuyWindow>().OnBuySuccess();
                    GameManager.Instance.GetUIMan().Inform(InformTypes.SUCCESS_PURCHASE, "Buy Success", "You have Maniaco Spectacles.\nPlease buy Spectacles Battery\nin order to use this device.", itemData.Image);
                    return;
                }

                GameInfo.playerGold -= price;
                GameInfo.SavePlayerGold();

                GameManager.Instance.GetUIMan().itemBuyWindow.GetComponent<ItemBuyWindow>().OnBuySuccess();
                GameManager.Instance.GetUIMan().Inform(InformTypes.SUCCESS_PURCHASE, "Buy Success", "Your Boost Item Activated.", itemData.Image);
            }
            else
            {
                GameManager.Instance.GetUIMan().Inform(InformTypes.NOT_ENOUGH_COIN, "Not Enough Gold", "Want to Buy Gold?", itemData.Image);
            }
        }
        else
        {
            if (GameInfo.playerCoin >= price)
            {
                if (itemData.Name == "Energy Drink")
                {
                    if (GameInfo.heartLives == GameInfo.HeartLivesMax)
                    {
                        GameManager.Instance.GetUIMan().Inform(InformTypes.ALREADY_PURCHASE, "Already purchased", "You have reached MAX energy.", itemData.Image);
                        return;
                    }

                    int updatedHeartLives = GameInfo.heartLives + 2;
                    GameInfo.heartLives = GameInfo.HeartLivesMax >= updatedHeartLives ? updatedHeartLives : GameInfo.HeartLivesMax;
                    GameInfo.SaveHeartLives();
                    GameManager.Instance.UpdateHeartLives();
                }
                else if (itemData.Name == "Chicken Sandwich")
                {
                    if (GameInfo.heartLives == GameInfo.HeartLivesMax)
                    {
                        GameManager.Instance.GetUIMan().Inform(InformTypes.ALREADY_PURCHASE, "Already purchased", "You have reached MAX energy.", itemData.Image);
                        return;
                    }

                    int updatedHeartLives = GameInfo.heartLives + 5;
                    GameInfo.heartLives = GameInfo.HeartLivesMax >= updatedHeartLives ? updatedHeartLives : GameInfo.HeartLivesMax;
                    GameInfo.SaveHeartLives();
                    GameManager.Instance.UpdateHeartLives();
                }
                else if (itemData.Name == "Bulletproof Vest")
                {
                    if (PlayerPrefs.GetFloat("CurrentArmor") > 50)
                    {
                        GameManager.Instance.GetUIMan().Inform(InformTypes.ALREADY_PURCHASE, "Already purchased", "You already purchased Bulletproof Vest.", itemData.Image);
                        return;
                    }

                    PlayerPrefs.SetFloat("CurrentArmor", 100);
                }
                else if (itemData.Name == "Spectacles Battery")
                {
                    int currentAmount = PlayerPrefs.GetInt("Battery", 0);
                    if (currentAmount == 0)
                        PlayerPrefs.SetFloat("BatteryLife", 180);
                    PlayerPrefs.SetInt("Battery", currentAmount + 1);
                }
                else
                {
                    if (ItemInventory.idList.Contains(id))
                    {
                        GameManager.Instance.GetUIMan().Inform(InformTypes.ALREADY_PURCHASE, "Already purchased", "You already have 2 boxes of this ammo.\nCome back when you are empty.", itemData.Image);
                        return;
                    }

                    ItemInventory.idList.Add(id);
                    ItemInventory.equipeList.Add(1);
                    ItemInventory.Save();
                }

                GameInfo.playerCoin -= price;
                GameInfo.SavePlayerCoin();

                GameManager.Instance.GetUIMan().itemBuyWindow.GetComponent<ItemBuyWindow>().OnBuySuccess();
                GameManager.Instance.GetUIMan().Inform(InformTypes.SUCCESS_PURCHASE, "Success", "Purchased", itemData.Image);
            }
            else
            {
                GameManager.Instance.GetUIMan().Inform(InformTypes.NOT_ENOUGH_COIN, "Not Enough Cash", "Want to Buy Cash?", itemData.Image);
            }
        }
    }
    public static void BuyWeapon(int id)
    {
        WeaponData wd = WeaponData.dataMap[id + 1];
        int price = wd.Price;
        if (WeaponInventory.idList.Contains(id))
        {
            GameManager.Instance.GetUIMan().Inform(InformTypes.ALREADY_PURCHASE, "Already Purchased", "You already purchased this weapon.", wd.Image);
        }
        else if (GameInfo.playerCoin >= price)
        {
            WeaponInventory.Append(id);
            WeaponInventory.Save();
            GameInfo.playerCoin -= price;
            GameInfo.SavePlayerCoin();
            GameManager.Instance.GetUIMan().itemBuyWindow.GetComponent<ItemBuyWindow>().OnBuySuccess();
            GameManager.Instance.GetUIMan().Inform(InformTypes.SUCCESS_PURCHASE, "Success", "Purchased", wd.Image);
        }
        else
        {
            GameManager.Instance.GetUIMan().Inform(InformTypes.NOT_ENOUGH_COIN, "Not Enough Cash", "Want to Buy Cash?", wd.Image);
        }
    }
}

public enum GameStates
{
    NONE,
    PLAY,
    PAUSE,
    STOP,
    COUNT,
    SLOWDOWN
}
public enum EventMessages
{
    NONE,
    ENTER_INTROWINDOW,
    ENTER_GHOSTHALL,
    ENTER_SMALL_RADAR_WINDOW,
    ENTER_LARGE_RADAR_WINDOW,
    ENTER_STREETVIEW_WINDOW,
    POINTENTER_MONSTER_RADARSPOT,
    POINTEXIT_MONSTER_RADARSPOT,
    MONSTER_DEAD,
    ENTER_MAINMENU_WINDOW,
    ENTER_SETTING_WINDOW,
    ENTER_ITEMBUY_WINDOW,
    ENTER_ITEMUPGRADE_WINDOW,
    ENTER_COINBUY_WINDOW,
    ITEM_UPGADE,
    ITEM_BUY,
    WEAPON_BUY,
    ENTER_MISSION_WINDOW,
    ENTER_NEXT_SCENE,
    GAME_PLAYING,
    INFORM_SHOW,
    TRY_BILLING_INIT,
    TRY_BILLING_CONNECT,
    TRY_PURCHASE,
    CANCEL_PURCHASE,
    FAIL_PURCHASE,
    SUCCESS_PURCHASE,
    BILLING_INIT_SUCCESS,
    BILLING_INIT_FAIL,
    BILLING_CONNECT_SUCCESS,
    BILLING_CONNECT_FAIL,
    OTHER
}
public class GameManager : MonoBehaviour
{
    [HideInInspector] public float SLOWDOWN_RATE = 4;

    UIManager uiMan;
    TargetManager targetMan;
    GameObject modelShowObject;
    public PersistantData persistantData;
    public List<string> m_MissionWeapons = new List<string>();
    public bool b_DoubleCashActivated = false;
    public bool b_UnbreakableActivated = false;
    public List<Dictionary<string, object>> missionStars;

    public TargetManager GetTargetMan()
    {
        targetMan = GameObject.FindWithTag("TargetMan").GetComponent<TargetManager>();
        return targetMan;
    }
    public UIManager GetUIMan()
    {
        uiMan = GameObject.FindWithTag("UIMan").GetComponent<UIManager>();
        return uiMan;
    }
    public GameObject GetModelShowObj()
    {
        modelShowObject = GameObject.FindWithTag("OverGuiBase");
        return modelShowObject;
    }

    static EventMessages lastEventMessage;
    public EventMessages LastEventMessage
    {
        get { return lastEventMessage; }
        set { lastEventMessage = value; }
    }
    static GameStates curState;
    public GameStates CurState
    {
        get { return curState; }
        set { curState = value; }
    }
    static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GameManager)FindObjectOfType(typeof(GameManager));
            }
            return instance;
        }
    }

    public void ProcEventMessages(EventMessages e, object param = null, bool isWeapon = true)
    {
        lastEventMessage = e;
        switch (e)
        {
            case EventMessages.ENTER_MAINMENU_WINDOW:
                GetUIMan().ShowMainMenuWindow();
                break;
            case EventMessages.ENTER_SETTING_WINDOW:
                GetUIMan().ShowSettingWindow();
                break;
            case EventMessages.ENTER_MISSION_WINDOW:
                ItemModelBase.Instance.DestroyCurModel();
                GetUIMan().ShowMisionWindow();
                break;
            case EventMessages.ENTER_ITEMBUY_WINDOW:
                GetUIMan().ShowItemBuyWindow();
                break;
            case EventMessages.ENTER_ITEMUPGRADE_WINDOW:
                GetUIMan().ShowItemUpgradeWindow();
                break;
            case EventMessages.ENTER_COINBUY_WINDOW:
                GetUIMan().ShowCoinBuyWindow();
                break;
            case EventMessages.ITEM_UPGADE:
                if (isWeapon == true)
                {
                    string item_str = (string)param;
                    int wp_id = int.Parse(item_str);
                    GameDataManager.UpgradeWeapon(wp_id);
                }
                else
                {
                    GameDataManager.UpgradeSpectacles();
                }
                break;
            case EventMessages.ITEM_BUY:
                {
                    string item_str = (string)param;
                    int wp_id = int.Parse(item_str);
                    GameDataManager.BuyItem(wp_id);
                }
                break;
            case EventMessages.WEAPON_BUY:
                {
                    string item_str = (string)param;
                    int wp_id = int.Parse(item_str);
                    GameDataManager.BuyWeapon(wp_id);
                }
                break;
            case EventMessages.ENTER_NEXT_SCENE:
                ScreenFader fadeScr = GameObject.FindObjectOfType<ScreenFader>();
                int endScene = SceneManager.GetActiveScene().buildIndex + 1;
                fadeScr.EndScene(endScene);
                break;
            default:
                break;
        }
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("DestroyedObjectPersist");
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        GameDataController.Init(null, null);
        GameDataManager.LoadGameData();

        if (FileTool.IsFileExists("Data") == false)
        {
            string dataAsJson = JsonUtility.ToJson(persistantData);
            FileTool.createORwriteFile("Data", dataAsJson);
        }
        else
        {
            string dataAsJson = FileTool.ReadFile("Data", false);
            persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);
        }

        if (FileTool.IsFileExists("MissionStars") == false)
        {
            string dataAsRaw = "missionId,stars\n" +
                                "0,0\n" +
                                "1,0\n" +
                                "2,0\n" +
                                "3,0\n" +
                                "4,0\n" +
                                "5,0\n" +
                                "6,0\n" +
                                "7,0\n" +
                                "8,0\n" +
                                "9,0\n" +
                                "10,0\n" +
                                "11,0\n" +
                                "12,0\n" +
                                "13,0\n" +
                                "14,0\n" +
                                "15,0\n" +
                                "16,0\n" +
                                "17,0\n" +
                                "18,0\n" +
                                "19,0\n" +
                                "20,0\n" +
                                "21,0\n" +
                                "22,0\n" +
                                "23,0\n" +
                                "24,0";
            FileTool.createORwriteFile("MissionStars", dataAsRaw);
        }

        missionStars = CSVReader.Read("MissionStars");

        if (FileTool.IsFileExists("Plants") == false)
        {
            string dataAsRaw = "missionId,zombiePlant,bossPlant\n" +
                                "0,,\n" +
                                "1,,\n" +
                                "2,0,\n" +
                                "3,,\n" +
                                "4,,\n" +
                                "5,,\n" +
                                "6,,\n" +
                                "7,2,\n" +
                                "8,,\n" +
                                "9,,\n" +
                                "10,,\n" +
                                "11,4,\n" +
                                "12,,\n" +
                                "13,,6\n" +
                                "14,,\n" +
                                "15,,\n" +
                                "16,,\n" +
                                "17,8,\n" +
                                "18,,\n" +
                                "19,,\n" +
                                "20,10,\n" +
                                "21,,\n" +
                                "22,,\n" +
                                "23,,\n" +
                                "24,,11";
            FileTool.createORwriteFile("Plants", dataAsRaw);
        }

        if (FileTool.IsFileExists("TaskList") == false)
        {
            string dataAsRaw = "id,task,rewards\n" +
                                "1,Kill 10 zombies,200\n" +
                                "2,5 headshots in a row,400\n" +
                                "3,Kill 20 zombies,600\n" +
                                "4,Kill 10 zombies using only melee weapon,8\n" +
                                "5,Kill 50 zombies,1000\n" +
                                "6,Complete a round in under 1 minute,500\n" +
                                "7,Complete a day without getting injured,1000\n" +
                                "8,Kill 75 zombies,15\n" +
                                "9,Share game with a friend on Facebook,500\n" +
                                "10,Share game with a friend on Instagram,500\n" +
                                "11,Rate the game 5 stars,1000";
            FileTool.createORwriteFile("TaskList", dataAsRaw);
        }
    }
    void Update()
    {
        UpdateHeartLives();
        //Debug.Log ("())))))))))))))))))))))))))))))))))))" + GameInfo.heartLives);
    }
    public void UpdateHeartLives()
    {
        if (GameInfo.heartLives == GameInfo.HeartLivesMax)
        {
            //Debug.Log ("())))))))))))))))))))))))))))))))))))" + GameInfo.heartLives);
            return;
        }
        //		if(GameInfo.lastEnergyIO_TimeTicksString == ""){
        //			GameInfo.heartLives = GameInfo.HeartLivesMax;
        //		}
        long curTimeTicks = System.DateTime.Now.Ticks;
        long lastEnergyIO_TimeTicks = long.Parse(GameInfo.lastEnergyIO_TimeTicksString);
        long passedTimeTicks = curTimeTicks - lastEnergyIO_TimeTicks;
        if (passedTimeTicks >= GameInfo.OneHeartChargeTimeTicks)
        {
            GameInfo.heartLives = BaseFuncs.Increase(GameInfo.heartLives, (int)(passedTimeTicks / GameInfo.OneHeartChargeTimeTicks), GameInfo.HeartLivesMax, 0, false);
            GameInfo.lastEnergyIO_TimeTicksString = curTimeTicks.ToString();
            GameInfo.SaveLastEnergyIoTimeTicksString();
            //Debug.Log ("())))))))))))))))))))))))))))))))))))" + GameInfo.heartLives);
        }
    }

    bool urglyLabelShow;

    void DoPlayTestMode()
    {
        GameSetting.isTestMode = true;
    }
    void DoLimitPlayer()
    {
        GameSetting.isTestMode = true;
        GameSetting.isLimitMode = false;
        if (GameSetting.isLimitMode)
        {
            ++GameInfo.playCounts;
            GameInfo.SavePlayCount();
            if (GameInfo.playCounts > GameInfo.playCountLimits)
            {
                urglyLabelShow = true;
            }
        }
    }
}
