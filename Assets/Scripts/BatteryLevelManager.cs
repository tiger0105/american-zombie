using Polaris.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SpectaclesType
{
	NONE = -1, 
	LEVEL1 = 0, 
	LEVEL2 = 1, 
	LEVEL3 = 2
}

public class BatteryLevelManager : MonoBehaviour
{
	[SerializeField] private GameObject m_SpectaclesIcon;
	[SerializeField] private GameObject m_BatteryIcon;

	[SerializeField] private Sprite[] m_BatteryLevelImages;

	[SerializeField] private GameObject m_WeaponCamera;

	[SerializeField] private CameraFilterPack_Atmosphere_Fog m_FogFilter;
	[SerializeField] private CameraFilterPack_Blur_GaussianBlur m_BlurFilter;

	[SerializeField] private CameraFilterPack_Real_VHS m_NightmareFilter;

	[SerializeField] private CameraFilterPack_Gradients_Therma m_SFilterLevel1;
	[SerializeField] private CameraFilterPack_Oculus_NightVision2 m_SFilterLevel2;
	[SerializeField] private CameraFilterPack_AAA_SuperHexagon m_SFilterLevel3;

	[SerializeField] private GameObject m_HUDRoot;
	[SerializeField] private GameObject m_HUD_Red;
	[SerializeField] private GameObject m_HUD_Blue;

	[SerializeField] private TextMeshProUGUI m_BatteryCountText;

	private bool isHUD_Blue = true;
	private MissionData missionData;
	private SpectaclesType spectaclesType = SpectaclesType.NONE;

	public void DischargeBattery()
	{
		float batteryLife = PlayerPrefs.GetFloat("BatteryLife", 0);
		if (batteryLife > 0)
		{
			batteryLife--;
			PlayerPrefs.SetFloat("BatteryLife", batteryLife);
			Debug.Log("Battery Life: " + batteryLife);
			CheckFilter();
		}
		else
		{
			CancelInvoke("DischargeBattery");
			Debug.Log("Battery Life Ended, please replace battery");
			int battery = PlayerPrefs.GetInt("Battery", 0);
			if (battery > 0)
				battery--;
			m_BatteryCountText.text = "x" + battery;
			PlayerPrefs.SetInt("Battery", battery);
			if (battery > 0)
				UserAnotherBattery();
			else
			{
				DisableAllFilters();
				if (missionData.Index >= 6 && missionData.Index < 11)
				{
					m_FogFilter.enabled = false;
					m_BlurFilter.enabled = true;
				}
				if (missionData.Index >= 11 && missionData.Index < 14)
				{
					m_FogFilter.enabled = true;
					m_BlurFilter.enabled = false;
				}
				else if (missionData.Index >= 14)
				{
					m_FogFilter.enabled = true;
					m_BlurFilter.enabled = true;
				}

				spectaclesType = SpectaclesType.NONE;
				
				m_HUDRoot.SetActive(false);
				m_SpectaclesIcon.GetComponent<Toggle>().isOn = false;
				m_SpectaclesIcon.GetComponent<Toggle>().interactable = false;
			}
			CheckFilter();
		}
	}

	public void UserAnotherBattery()
	{
		PlayerPrefs.SetFloat("BatteryLife", 180);
		InvokeRepeating("DischargeBattery", 1, 1);

		CheckFilter();
	}

	private void Start()
	{
		missionData = MissionProgress.GetCurMissionData();

		float filterPercentValue = 1 - Mathf.Pow(0.9f, missionData.Index);

		m_FogFilter.Fade = filterPercentValue < 1 ? filterPercentValue : 1;

		DisableAllFilters();

		if (missionData.Index >= 6 && missionData.Index < 11)
		{
			if (missionData.Index != 8)
			{
				m_BlurFilter.enabled = true;
			}
			else
			{
				m_NightmareFilter.enabled = true;
			}
		}
		if (missionData.Index >= 11 && missionData.Index < 14)
		{
			m_FogFilter.enabled = true;
		}
		else if (missionData.Index > 14)
		{
			if (missionData.Index != 20)
			{
				m_FogFilter.enabled = true;
				m_BlurFilter.enabled = true;
			}
			else
			{
				m_NightmareFilter.enabled = true;
			}
		}
		else if (missionData.Index == 14 || missionData.Index == 4)
		{
			m_NightmareFilter.enabled = true;
		}

		spectaclesType = SpectaclesType.NONE;

		InvokeRepeating("ChangeHUD", 10, 10);
		
		CheckFilter();
	}

	public void CheckFilter()
	{
		if (missionData.Index == 4
			|| missionData.Index == 8
			|| missionData.Index == 14
			|| missionData.Index == 20)
		{
			m_SpectaclesIcon.GetComponent<Toggle>().interactable = false;
			m_SpectaclesIcon.SetActive(false);
			m_BatteryIcon.SetActive(false);
			return;
		}

		if (PlayerPrefs.GetInt("Spectacles", 0) == 1)
		{
			m_SpectaclesIcon.SetActive(true);

			m_BatteryCountText.text = "x" + PlayerPrefs.GetInt("Battery", 0);

			if (PlayerPrefs.GetInt("Battery", 0) > 0)
			{
				float batteryLife = PlayerPrefs.GetFloat("BatteryLife", 0);
				if (batteryLife > 0 && batteryLife < 45)
				{
					if (missionData.Index >= 6)
						m_SpectaclesIcon.GetComponent<Toggle>().interactable = true;
					else
						m_SpectaclesIcon.GetComponent<Toggle>().interactable = false;
					m_BatteryIcon.GetComponent<Image>().sprite = m_BatteryLevelImages[3];
				}
				else if (batteryLife >= 45 && batteryLife < 90)
				{
					if (missionData.Index >= 6)
						m_SpectaclesIcon.GetComponent<Toggle>().interactable = true;
					else
						m_SpectaclesIcon.GetComponent<Toggle>().interactable = false;
					m_BatteryIcon.GetComponent<Image>().sprite = m_BatteryLevelImages[2];
				}
				else if (batteryLife >= 90 && batteryLife < 135)
				{
					if (missionData.Index >= 6)
						m_SpectaclesIcon.GetComponent<Toggle>().interactable = true;
					else
						m_SpectaclesIcon.GetComponent<Toggle>().interactable = false;
					m_BatteryIcon.GetComponent<Image>().sprite = m_BatteryLevelImages[1];
				}
				else if (batteryLife >= 135)
				{
					if (missionData.Index >= 6)
						m_SpectaclesIcon.GetComponent<Toggle>().interactable = true;
					else
						m_SpectaclesIcon.GetComponent<Toggle>().interactable = false;
					m_BatteryIcon.GetComponent<Image>().sprite = m_BatteryLevelImages[0];
				}
				else
				{
					m_SpectaclesIcon.GetComponent<Toggle>().interactable = false;
					m_BatteryIcon.GetComponent<Image>().sprite = m_BatteryLevelImages[4];
				}
			}
			else
			{
				m_BatteryIcon.GetComponent<Image>().sprite = m_BatteryLevelImages[4];
				m_SpectaclesIcon.GetComponent<Toggle>().interactable = false;
			}
			m_BatteryIcon.SetActive(true);
		}
		else
		{
			m_SpectaclesIcon.GetComponent<Toggle>().interactable = false;
			m_SpectaclesIcon.SetActive(false);
			m_BatteryIcon.SetActive(false);
		}
	}

	private void DisableAllFilters()
	{
		m_FogFilter.enabled = false;
		m_BlurFilter.enabled = false;
		m_NightmareFilter.enabled = false;
		m_SFilterLevel1.enabled = false;
		m_SFilterLevel2.enabled = false;
		m_SFilterLevel3.enabled =false;

		m_HUDRoot.SetActive(false);
		m_HUD_Blue.SetActive(true);
		m_HUD_Red.SetActive(false);
	}

	private void ChangeHUD()
	{
		if (isHUD_Blue == true)
		{
			m_HUD_Blue.SetActive(true);
			m_HUD_Red.SetActive(false);
			isHUD_Blue = false;
		}
		else
		{
			m_HUD_Blue.SetActive(false);
			m_HUD_Red.SetActive(true);
			isHUD_Blue = true;
		}
	}

	public void EnableSpectaclesView()
	{
		DisableAllFilters();

		int level = PlayerPrefs.GetInt("SpectaclesLevel", 1);

		MissionData missionData = MissionProgress.GetCurMissionData();

		if (missionData.Index >= 6 && missionData.Index < 11)
		{
			if (missionData.Index != 8)
			{
				if (level == 1)
				{
					m_SFilterLevel1.enabled = true;
					spectaclesType = SpectaclesType.LEVEL1;
					m_HUDRoot.SetActive(false);

					InvokeRepeating("DischargeBattery", 1, 1);
				}
				else if (level == 2)
				{
					if (spectaclesType != SpectaclesType.LEVEL2)
					{
						m_SFilterLevel2.enabled = true;
						spectaclesType = SpectaclesType.LEVEL2;
					}
					else
					{
						m_SFilterLevel1.enabled = true;
						spectaclesType = SpectaclesType.LEVEL1;
					}

					m_HUDRoot.SetActive(false);

					InvokeRepeating("DischargeBattery", 1, 1);
				}
				else if (level == 3)
				{
					if (spectaclesType != SpectaclesType.LEVEL1 && spectaclesType != SpectaclesType.LEVEL3)
					{
						m_SFilterLevel3.enabled = true;
						spectaclesType = SpectaclesType.LEVEL3;
						m_HUDRoot.SetActive(true);
					}
					else if (spectaclesType != SpectaclesType.LEVEL1)
					{
						m_SFilterLevel1.enabled = true;
						spectaclesType = SpectaclesType.LEVEL1;
						m_HUDRoot.SetActive(false);
					}
					else
					{
						m_SFilterLevel2.enabled = true;
						spectaclesType = SpectaclesType.LEVEL2;
						m_HUDRoot.SetActive(false);
					}

					InvokeRepeating("DischargeBattery", 1, 1);
				}
				else
				{
					m_BlurFilter.enabled = true;
					spectaclesType = SpectaclesType.NONE;
					m_HUDRoot.SetActive(false);
					CancelInvoke("DischargeBattery");
				}
			}
			else
			{
				m_NightmareFilter.enabled = true;
				spectaclesType = SpectaclesType.NONE;
				m_HUDRoot.SetActive(false);
				CancelInvoke("DischargeBattery");
			}
		}
		else if (missionData.Index >= 11 && missionData.Index < 14)
		{
			if (level == 2)
			{
				if (spectaclesType != SpectaclesType.LEVEL2)
				{
					m_SFilterLevel2.enabled = true;
					spectaclesType = SpectaclesType.LEVEL2;
				}
				else
				{
					m_SFilterLevel1.enabled = true;
					spectaclesType = SpectaclesType.LEVEL1;
				}

				m_HUDRoot.SetActive(false);

				InvokeRepeating("DischargeBattery", 1, 1);
			}
			else if (level == 3)
			{
				if (spectaclesType != SpectaclesType.LEVEL1 && spectaclesType != SpectaclesType.LEVEL3)
				{
					m_SFilterLevel3.enabled = true;
					spectaclesType = SpectaclesType.LEVEL3;
					m_HUDRoot.SetActive(true);
				}
				else if (spectaclesType != SpectaclesType.LEVEL1)
				{
					m_SFilterLevel1.enabled = true;
					spectaclesType = SpectaclesType.LEVEL1;
					m_HUDRoot.SetActive(false);
				}
				else
				{
					m_SFilterLevel2.enabled = true;
					spectaclesType = SpectaclesType.LEVEL2;
					m_HUDRoot.SetActive(false);
				}

				InvokeRepeating("DischargeBattery", 1, 1);
			}
			else
			{
				m_FogFilter.enabled = true;
				spectaclesType = SpectaclesType.NONE;
				m_HUDRoot.SetActive(false);
				CancelInvoke("DischargeBattery");
			}
		}
		else if (missionData.Index >= 14)
		{
			if (missionData.Index != 14 && missionData.Index != 20)
			{
				if (level == 3)
				{
					if (spectaclesType != SpectaclesType.LEVEL1 && spectaclesType != SpectaclesType.LEVEL3)
					{
						m_SFilterLevel3.enabled = true;
						spectaclesType = SpectaclesType.LEVEL3;
						m_HUDRoot.SetActive(true);
					}
					else if (spectaclesType != SpectaclesType.LEVEL1)
					{
						m_SFilterLevel1.enabled = true;
						spectaclesType = SpectaclesType.LEVEL1;
						m_HUDRoot.SetActive(false);
					}
					else
					{
						m_SFilterLevel2.enabled = true;
						spectaclesType = SpectaclesType.LEVEL2;
						m_HUDRoot.SetActive(false);
					}

					InvokeRepeating("DischargeBattery", 1, 1);
				}
				else
				{
					m_FogFilter.enabled = true;
					m_BlurFilter.enabled = true;
					spectaclesType = SpectaclesType.NONE;
					m_HUDRoot.SetActive(false);
					CancelInvoke("DischargeBattery");
				}
			}
			else
			{
				m_NightmareFilter.enabled = true;
				spectaclesType = SpectaclesType.NONE;
				m_HUDRoot.SetActive(false);
				CancelInvoke("DischargeBattery");
			}
		}
		else if (missionData.Index == 4)
		{
			m_NightmareFilter.enabled = true;
			spectaclesType = SpectaclesType.NONE;
			m_HUDRoot.SetActive(false);
			CancelInvoke("DischargeBattery");
		}
	}

	public void DisableSpectaclesView()
	{
		DisableAllFilters();

		CancelInvoke("DischargeBattery");

		if (missionData.Index >= 6 && missionData.Index < 11 && missionData.Index != 8)
		{
			m_BlurFilter.enabled = true;
		}
		if (missionData.Index >= 11 && missionData.Index < 14)
		{
			m_FogFilter.enabled = true;
		}
		else if (missionData.Index > 14 && missionData.Index != 20)
		{
			m_FogFilter.enabled = true;
			m_BlurFilter.enabled = true;
		}
		else if (missionData.Index == 4 
			|| missionData.Index == 8 
			|| missionData.Index == 14 
			|| missionData.Index == 20)
		{
			m_NightmareFilter.enabled = true;
		}

		m_HUDRoot.SetActive(false);
	}

	public void OnSpectaclesToggleChanged(bool isOn)
	{
		if (isOn == true)
		{
			EnableSpectaclesView();
		}
		else
		{
			DisableSpectaclesView();
		}
	}
}
