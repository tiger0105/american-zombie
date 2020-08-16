using UnityEngine;
using UnityEngine.UI;
using Polaris.GameData;
using TMPro;
using System.Collections.Generic;

public class MissionWeaponWindow : MonoBehaviour
{
    [SerializeField] public GameObject m_WeaponItemPrefab;

    [SerializeField] private GameObject m_WeaponListContent;
    [SerializeField] private Button m_MissionStartButton;
    [SerializeField] private GameObject m_ActionNeededDialog;
    [SerializeField] private TextMeshProUGUI m_Description;

    [SerializeField] private GameObject m_MissionInformDlg;
    [SerializeField] private TextMeshProUGUI m_Title;
    [SerializeField] private TextMeshProUGUI m_Message;

    private MissionData m_MissionData;

    private void OnEnable()
    {
        m_MissionInformDlg.SetActive(false);

        m_MissionData = MissionProgress.GetCurMissionData();
        
        UpdateWeaponList();
        ValidateStartButton();
        CheckGooglesStatus();
    }

    private void CheckGooglesStatus()
    {
        int hasSpectacles = PlayerPrefs.GetInt("Spectacles", 0);
        int spectaclesLevel = PlayerPrefs.GetInt("SpectaclesLevel", 1);
        int batteryCount = PlayerPrefs.GetInt("Battery", 0);

        if (m_MissionData.Index >= 6 && m_MissionData.Index < 11)
        {
            if (hasSpectacles == 0)
            {
                ShowNotify("INFORM", "You have to purchase spectacles item.");
            }
            else if (batteryCount < 1)
            {
                ShowNotify("INFORM", "Out of batteries. Please purchase batteries to use spectacles.");
            }
        }
        else if (m_MissionData.Index >= 11 && m_MissionData.Index < 14)
        {
            if (hasSpectacles == 0)
            {
                ShowNotify("INFORM", "You have to purchase spectacles item.");
            }
            else if (batteryCount < 1)
            {
                ShowNotify("INFORM", "Out of batteries. Please purchase batteries to use spectacles.");
            }
            else if (spectaclesLevel < 2)
            {
                ShowNotify("INFORM", "Please upgrade your spectacles level to 2.");
            }
        }
        else if (m_MissionData.Index >= 14)
        {
            if (hasSpectacles == 0)
            {
                ShowNotify("INFORM", "You have to purchase spectacles item.");
            }
            else if (batteryCount < 1)
            {
                ShowNotify("INFORM", "Out of batteries. Please purchase batteries to use spectacles.");
            }
            else if (spectaclesLevel < 3)
            {
                ShowNotify("INFORM", "Please upgrade your spectacles level to 3.");
            }
        }
    }

    private void ShowNotify(string title, string messageText)
    {
        m_Title.text = title;
        m_Message.text = messageText;
        m_MissionInformDlg.SetActive(true);
    }

    private void UpdateWeaponList()
    {
        for (int i = 0; i < m_WeaponListContent.transform.childCount; i ++)
        {
            Destroy(m_WeaponListContent.transform.GetChild(i).gameObject);
        }

        m_WeaponListContent.transform.DetachChildren();

        for (int i = 0; i < WeaponInventory.idList.Count; i++)
        {
            int weaponId = WeaponInventory.idList[i];
            GameObject weaponItem = Instantiate(m_WeaponItemPrefab);
            weaponItem.name = WeaponInventory.GetWeaponData(WeaponInventory.GetInventoryID(weaponId)).Name;
            weaponItem.transform.SetParent(m_WeaponListContent.transform, false);
            weaponItem.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(WeaponData.dataMap[weaponId + 1].Image);
            weaponItem.transform.GetChild(1).GetComponent<Text>().text = WeaponData.dataMap[weaponId + 1].Name;
            weaponItem.transform.GetChild(2).GetComponent<Text>().text = weaponId > 7 ? "MELEE" : "RANGE";
            int inventoryId = WeaponInventory.GetInventoryID(weaponId);
            int weaponLevel = WeaponInventory.levelList[inventoryId] - 1;
            for (int j = 0; j < weaponItem.transform.GetChild(3).childCount; j++)
            {
                weaponItem.transform.GetChild(3).GetChild(j).GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f);
            }
            for (int j = 0; j <= weaponLevel; j++)
            {
                weaponItem.transform.GetChild(3).GetChild(j).GetComponent<Image>().color = new Color(1, 1, 1);
            }
            Toggle toggle = weaponItem.transform.GetChild(4).GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate {
                OnValidateWeaponUse(toggle, weaponLevel);
                OnWeaponItemToggleChanged(toggle);
                ValidateStartButton();
            });
            if (weaponItem.transform.GetChild(2).GetComponent<Text>().text == "MELEE")
            {
                weaponItem.transform.GetChild(4).GetComponent<Toggle>().group = m_WeaponListContent.GetComponent<ToggleGroup>();
            }
        }
    }

    private void OnWeaponItemToggleChanged(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            toggle.transform.GetChild(2).GetComponent<Text>().text = "In Use";
        }
        else
        {
            toggle.transform.GetChild(2).GetComponent<Text>().text = "Use";
        }
    }

    private void OnValidateWeaponUse(Toggle toggle, int weaponLevel)
    {
        if (toggle.isOn == true)
        {
            if (m_MissionData.Index >= 8 && weaponLevel < 1 
                || m_MissionData.Index >= 15 && weaponLevel < 2)
            {
                toggle.isOn = false;
                ShowActionNeededDialog(m_MissionData.Index + 1);
            }
        }
    }

    private void ShowActionNeededDialog(int missionIndex)
    {
        int weaponLevel = 1;
        
        if (missionIndex > 8)
            weaponLevel = 2;
        
        if (missionIndex > 15)
            weaponLevel = 3;
        
        m_Description.text = "You need to upgrade your weapon to level " + weaponLevel + " to use it in mission " + missionIndex;
        m_ActionNeededDialog.SetActive(true);
    }

    private void ValidateStartButton()
    {
        int inUseWeaponsCount = 0;
        int meleeWeapons = 0;

        for (int i = 0; i < m_WeaponListContent.transform.childCount; i ++)
        {
            if (m_WeaponListContent.transform.GetChild(i).GetChild(4).GetComponent<Toggle>().isOn == true)
            {
                inUseWeaponsCount++;
                if (m_WeaponListContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text == "MELEE")
                    meleeWeapons++;
            }
        }

        int rangeWeapons = inUseWeaponsCount - meleeWeapons;

        if (inUseWeaponsCount == 0 || rangeWeapons > 3 || meleeWeapons > 1)
            m_MissionStartButton.interactable = false;
        else
            m_MissionStartButton.interactable = true;
    }

    public void StartMission()
    {
        GameManager.Instance.m_MissionWeapons = new List<string>();
        for (int i = 0; i < m_WeaponListContent.transform.childCount; i++)
        {
            if (m_WeaponListContent.transform.GetChild(i).GetChild(4).GetComponent<Toggle>().isOn == true)
            {
                GameManager.Instance.m_MissionWeapons.Add(m_WeaponListContent.transform.GetChild(i).name);
            }
        }
            
        GameManager.Instance.ProcEventMessages(EventMessages.ENTER_NEXT_SCENE);
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }
}
