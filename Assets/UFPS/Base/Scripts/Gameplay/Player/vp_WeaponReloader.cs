﻿/////////////////////////////////////////////////////////////////////////////////
//
//	vp_WeaponReloader.cs
//	© Opsive. All Rights Reserved.
//	https://twitter.com/Opsive
//	http://www.opsive.com
//
//	description:	this component adds firearm reload logic, sound and reload
//					duration to an FPWeapon. it doesn't handle ammo max caps
//					or levels. instead this should be governed by an inventory
//					system via the event handler
//
/////////////////////////////////////////////////////////////////////////////////

using Polaris.GameData;
using UnityEngine;



public class vp_WeaponReloader : MonoBehaviour
{

	protected vp_Weapon m_Weapon = null;
	protected vp_PlayerEventHandler m_Player = null;

	protected AudioSource m_Audio = null;
	public AudioClip SoundReload = null;

	public float ReloadDuration = 1.0f;


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Awake()
	{

		m_Audio = GetComponent<AudioSource>();

		// store the first player event handler found in the top of our transform hierarchy
		m_Player = (vp_PlayerEventHandler)transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));

	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Start()
	{
		
		// store a reference to the FPSWeapon
		m_Weapon = transform.GetComponent<vp_Weapon>();
		
	}


	/// <summary>
	/// registers this component with the event handler (if any)
	/// </summary>
	protected virtual void OnEnable()
	{

		// allow this monobehaviour to talk to the player event handler
		if (m_Player != null)
			m_Player.Register(this);

	}


	/// <summary>
	/// unregisters this component from the event handler (if any)
	/// </summary>
	protected virtual void OnDisable()
	{

		// unregister this monobehaviour from the player event handler
		if (m_Player != null)
			m_Player.Unregister(this);

	}


	/// <summary>
	/// adds a condition (a rule set) that must be met for the
	/// event handler 'Reload' activity to successfully activate.
	/// NOTE: other scripts may have added conditions to this
	/// activity aswell
	/// </summary>
	protected virtual bool CanStart_Reload()
	{

		// can't reload if current weapon isn't fully wielded
		if (m_Player.CurrentWeaponWielded.Get() == false)
			return false;

		// can't reload if weapon is full
		if (m_Player.CurrentWeaponMaxAmmoCount.Get() != 0 &&	// only check if max capacity is reported
			(m_Player.CurrentWeaponAmmoCount.Get() == m_Player.CurrentWeaponMaxAmmoCount.Get()))
			return false;

		// can't reload if the inventory has no additional ammo for this weapon
		if (m_Player.CurrentWeaponClipCount.Get() < 1)
		{
			return false;
		}

		return true;

	}


	/// <summary>
	/// this callback is triggered right after the 'Reload' activity
	/// has been approved for activation
	/// </summary>
	protected virtual void OnStart_Reload()
	{

        // end the Reload activity in 'ReloadDuration' seconds
        switch (m_Weapon.name)
        {
            case "1Pistol":
                {
                    int inventory_id = WeaponInventory.GetInventoryID(0);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    float reduced = newLevel * WeaponData.dataMap[1].GetReloadSpeed_LevelUp();
                    float speed = WeaponData.dataMap[1].GetReloadSpeed() - reduced;
                    m_Player.Reload.AutoDuration = speed;
                }
                break;
            case "2Shotgun":
                {
                    int inventory_id = WeaponInventory.GetInventoryID(1);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    float reduced = newLevel * WeaponData.dataMap[2].GetReloadSpeed_LevelUp();
                    float speed = WeaponData.dataMap[2].GetReloadSpeed() - reduced;
                    m_Player.Reload.AutoDuration = speed;
                }
                break;
            case "3Autorifle":
                {
                    int inventory_id = WeaponInventory.GetInventoryID(2);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    float reduced = newLevel * WeaponData.dataMap[3].GetReloadSpeed_LevelUp();
                    float speed = WeaponData.dataMap[3].GetReloadSpeed() - reduced;
                    m_Player.Reload.AutoDuration = speed;
                }
                break;
            case "4Grenadegun":
                {
                    int inventory_id = WeaponInventory.GetInventoryID(3);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    float reduced = newLevel * WeaponData.dataMap[4].GetReloadSpeed_LevelUp();
                    float speed = WeaponData.dataMap[4].GetReloadSpeed() - reduced;
                    m_Player.Reload.AutoDuration = speed;
                }
                break;
            case "5Superrifle":
                {
                    int inventory_id = WeaponInventory.GetInventoryID(4);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    float reduced = newLevel * WeaponData.dataMap[5].GetReloadSpeed_LevelUp();
                    float speed = WeaponData.dataMap[5].GetReloadSpeed() - reduced;
                    m_Player.Reload.AutoDuration = speed;
                }
                break;
            case "6Crossbow":
                {
                    int inventory_id = WeaponInventory.GetInventoryID(5);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    float reduced = newLevel * WeaponData.dataMap[6].GetReloadSpeed_LevelUp();
                    float speed = WeaponData.dataMap[6].GetReloadSpeed() - reduced;
                    m_Player.Reload.AutoDuration = speed;
                }
                break;
            case "7SniperBlaster":
                {
                    int inventory_id = WeaponInventory.GetInventoryID(6);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    float reduced = newLevel * WeaponData.dataMap[7].GetReloadSpeed_LevelUp();
                    float speed = WeaponData.dataMap[7].GetReloadSpeed() - reduced;
                    m_Player.Reload.AutoDuration = speed;
                }
                break;
			case "8FlameThrower":
				{
					int inventory_id = WeaponInventory.GetInventoryID(7);
					int newLevel = WeaponInventory.levelList[inventory_id] - 1;
					float reduced = newLevel * WeaponData.dataMap[8].GetReloadSpeed_LevelUp();
					float speed = WeaponData.dataMap[8].GetReloadSpeed() - reduced;
					m_Player.Reload.AutoDuration = speed;
				}
				break;
			default:
                m_Player.Reload.AutoDuration = m_Player.CurrentWeaponReloadDuration.Get();
                break;
        }
		
		if (m_Audio != null && GameSetting.soundOn == true)
		{
			m_Audio.pitch = Time.timeScale;
			m_Audio.PlayOneShot(SoundReload);
		}

	}


	/// <summary>
	/// this callback is triggered when the 'Reload' activity
	/// deactivates
	/// </summary>
	protected virtual void OnStop_Reload()
	{

		m_Player.RefillCurrentWeapon.Try();

	}


	/// <summary>
	/// returns the reload duration of the current weapon
	/// </summary>
	protected virtual float OnValue_CurrentWeaponReloadDuration
	{
		get
		{
			return ReloadDuration;
		}
	}
}

