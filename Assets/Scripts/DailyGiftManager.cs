using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Polaris.GameData;

/// <summary>
/// Written by HKC
/// Daily gift manager.
/// </summary>
public class DailyGiftManager : MonoBehaviour {
	public GameObject panel;
	public GameObject[] collectButtons;
    public GameObject overGUICamera;

	int accessDays;
	int lastAccessDay;
	int currentDay;
	int TotalDays = 7;

	// Use this for initialization
	void Start () {
		accessDays = PlayerPrefs.GetInt ("accessDays", 0);
		lastAccessDay = PlayerPrefs.GetInt ("lastAccessDay", 0);
		int firstPlay = PlayerPrefs.GetInt("FirstPlay", 0);
		currentDay = System.DateTime.Now.Day;

		panel.SetActive (false);

		if (accessDays == 0 || (currentDay != lastAccessDay)) {
			if (firstPlay == 0 || accessDays > 6)
				return;

			panel.SetActive (true);
			Init ();
		}
	}

	void Init() {
		for (int i = 0; i < TotalDays; i++)
			collectButtons [i].SetActive (false);

		collectButtons [accessDays].SetActive (true);
        overGUICamera.SetActive(false);
    }

	/// <summary>
	/// Close this window.
	/// </summary>
	public void Close() {
		DailyBonusData data = DailyBonusData.dataMap [accessDays + 1];
        if (data.Coin > 0)
        {
            GameInfo.playerCoin += data.Coin;
            GameInfo.SavePlayerCoin();
        }

        if (data.Id == 2)
        {
            if (ItemInventory.idList.Contains(1) == false)
            {
                ItemInventory.Append(1);
                ItemInventory.Save();
            }
        }
        else if (data.Id == 3)
        {
            if (ItemInventory.idList.Contains(2) == false)
            {
                ItemInventory.Append(2);
                ItemInventory.Save();
            }

            if (ItemInventory.idList.Contains(8) == false)
            {
                ItemInventory.Append(8);
                ItemInventory.Save();
            }
        }
        else if (data.Id == 4)
        {
            ItemInventory.Append(7);
            ItemInventory.Save();
        }
        else if (data.Id == 5)
        {
            if (ItemInventory.idList.Contains(9) == false)
            {
                ItemInventory.Append(9);
                ItemInventory.Save();
            }
        }
        else if (data.Id == 6)
        {
            if (ItemInventory.idList.Contains(1) == false)
            {
                ItemInventory.Append(1);
                ItemInventory.Save();
            }

            if (ItemInventory.idList.Contains(3) == false)
            {
                ItemInventory.Append(3);
                ItemInventory.Save();
            }
        }
        else if (data.Id == 7)
        {
            if (WeaponInventory.idList.Contains(4) == false)
            {
                WeaponInventory.Append(4);
                WeaponInventory.Save();
            }
        }

        PlayerPrefs.SetInt ("accessDays", ++accessDays);
		PlayerPrefs.SetInt ("lastAccessDay", currentDay);
		Destroy (panel);

    }
}