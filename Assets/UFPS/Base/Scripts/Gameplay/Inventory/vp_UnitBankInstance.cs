/////////////////////////////////////////////////////////////////////////////////
//
//	vp_UnitBankInstance.cs
//	© Opsive. All Rights Reserved.
//	https://twitter.com/Opsive
//	http://www.opsive.com
//
//	description:	this internal class is used to represent a unitbank item record
//					inside the inventory. NOTE: it is not to be confused with the
//					concept of an in-world item (pickup) gameobject instance (!)
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Polaris.GameData;

[System.Serializable]
public class vp_UnitBankInstance : vp_ItemInstance
{

	private const int UNLIMITED = -1;

	[SerializeField]
	public vp_UnitType UnitType;

	[SerializeField]
	public int Count = 0;
	[SerializeField]
	protected int m_Capacity = UNLIMITED;

	[SerializeField]
	protected vp_Inventory m_Inventory;

	protected bool m_Result;
	protected int m_PrevCount = 0;


	/// <summary>
	/// returns the capacity of this unit bank. a value of -1 means
	/// the capacity is unlimited. a value of zero means the unit
	/// bank is locked / disabled
	/// </summary>
	public int Capacity
	{
		get
		{
            if (Type != null)
            {
                vp_UnitBankType itemType = (vp_UnitBankType)Type;
                if (itemType.name == "Pistol")
                {
                    int inventory_id = WeaponInventory.GetInventoryID(0);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    int added = newLevel * WeaponData.dataMap[1].GetAmmoCapacity_LevelUp();
                    int capacity = WeaponData.dataMap[1].GetAmmoCapacity() + added;
                    m_Capacity = capacity;
                    //Debug.Log(itemType.name + " " + m_Capacity + " " + capacity);
                }
                else if (itemType.name == "Shotgun")
                {
                    int inventory_id = WeaponInventory.GetInventoryID(1);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    int added = newLevel * WeaponData.dataMap[2].GetAmmoCapacity_LevelUp();
                    int capacity = WeaponData.dataMap[2].GetAmmoCapacity() + added;
                    m_Capacity = capacity;
                    //Debug.Log(itemType.name + " " + m_Capacity + " " + capacity);
                }
                else if (itemType.name == "Autorifle")
                {
                    int inventory_id = WeaponInventory.GetInventoryID(2);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    int added = newLevel * WeaponData.dataMap[3].GetAmmoCapacity_LevelUp();
                    int capacity = WeaponData.dataMap[3].GetAmmoCapacity() + added;
                    m_Capacity = capacity;
                    //Debug.Log(itemType.name + " " + m_Capacity + " " + capacity);
                }
                else if (itemType.name == "Grenadegun")
                {
                    int inventory_id = WeaponInventory.GetInventoryID(3);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    int added = newLevel * WeaponData.dataMap[4].GetAmmoCapacity_LevelUp();
                    int capacity = WeaponData.dataMap[4].GetAmmoCapacity() + added;
                    m_Capacity = capacity;
                    //Debug.Log(itemType.name + " " + m_Capacity + " " + capacity);
                }
                else if (itemType.name == "Superrifle")
                {
                    int inventory_id = WeaponInventory.GetInventoryID(4);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    int added = newLevel * WeaponData.dataMap[5].GetAmmoCapacity_LevelUp();
                    int capacity = WeaponData.dataMap[5].GetAmmoCapacity() + added;
                    m_Capacity = capacity;
                    //Debug.Log(itemType.name + " " + m_Capacity + " " + capacity);
                }
                else if (itemType.name == "Crossbow")
                {
                    int inventory_id = WeaponInventory.GetInventoryID(5);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    int added = newLevel * WeaponData.dataMap[6].GetAmmoCapacity_LevelUp();
                    int capacity = WeaponData.dataMap[6].GetAmmoCapacity() + added;
                    m_Capacity = capacity;
                    //Debug.Log(itemType.name + " " + m_Capacity + " " + capacity);
                }
                else if (itemType.name == "Blaster")
                {
                    int inventory_id = WeaponInventory.GetInventoryID(6);
                    int newLevel = WeaponInventory.levelList[inventory_id] - 1;
                    int added = newLevel * WeaponData.dataMap[7].GetAmmoCapacity_LevelUp();
                    int capacity = WeaponData.dataMap[7].GetAmmoCapacity() + added;
                    m_Capacity = capacity;
                    //Debug.Log(itemType.name + " " + m_Capacity + " " + capacity);
                }
				else if (itemType.name == "FlameThrower")
				{
					int inventory_id = WeaponInventory.GetInventoryID(7);
					int newLevel = WeaponInventory.levelList[inventory_id] - 1;
					int added = newLevel * WeaponData.dataMap[8].GetAmmoCapacity_LevelUp();
					int capacity = WeaponData.dataMap[8].GetAmmoCapacity() + added;
					m_Capacity = capacity;
					//Debug.Log(itemType.name + " " + m_Capacity + " " + capacity);
				}
				else
                    m_Capacity = (itemType).Capacity;
            }
			return m_Capacity;
		}
		set
		{
			m_Capacity = Mathf.Max(UNLIMITED, value);
		}
	}


	/// <summary>
	/// constructor for unit banks that represent the unit capacity of
	/// loadable items such as firearms
	/// </summary>
	[SerializeField]
	public vp_UnitBankInstance(vp_UnitBankType unitBankType, int id, vp_Inventory inventory)
		: base(unitBankType, id)
	{
		UnitType = unitBankType.Unit;
		m_Inventory = inventory;
	}


	/// <summary>
	/// constructor for unit banks that represent a player's capacity
	/// to carry a certain type of unit
	/// </summary>
	[SerializeField]
	public vp_UnitBankInstance(vp_UnitType unitType, vp_Inventory inventory)
		: base(null, 0)
	{
		UnitType = unitType;
		m_Inventory = inventory;
	}


	/// <summary>
	/// tries to remove 'amount' units from the bank, and returns
	/// the amount of units eventually removed
	/// </summary>
	public virtual bool TryRemoveUnits(int amount)
	{

		// if the bank holds zero units, abort
		if (Count <= 0)
			return false;

		// make sure never to remove with a negative value
		amount = Mathf.Max(0, amount);

		// removing zero units will count as failure
		if (amount == 0)
			return false;

		// remove the units
		Count = Mathf.Max(0, (Count - amount));

		return true;

	}


	/// <summary>
	/// tries to add 'amount' units to the bank, taking item
	/// limit rules into account
	/// </summary>
	public virtual bool TryGiveUnits(int amount)
	{

		if ((Type != null) && ((vp_UnitBankType)Type).Reloadable == false)
			return false;

		// if capacity has already been met, abort
		if ((Capacity != UNLIMITED) && (Count >= Capacity))
			return false;

		// make sure never to add with a negative value
		amount = Mathf.Max(0, amount);

		// adding zero units amounts to failure
		if (amount == 0)
			return false;

		// add the units (preliminary)
		Count += amount;

		// if capacity was not exceeded
		if (Count <= Capacity)
			return true;

		// if capacity is unlimited
		if (Capacity == UNLIMITED)
			return true;

		// if capacity is limited and was exceeded, clamp to capacity
		// int result = Capacity - (Count - amount);		// this is how to calculate amount added
		Count = Capacity;

		return true;

	}


	/// <summary>
	/// 
	/// </summary>
	public virtual bool IsInternal
	{
		get
		{
			return Type == null;
		}
	}


	/// <summary>
	/// 
	/// </summary>
	public virtual bool DoAddUnits(int amount)
	{
		// TODO: 'do' shouldn't run 'try'. it should be the other way around
		m_PrevCount = Count;
		m_Result = TryGiveUnits(amount);
		if (m_Inventory.SpaceEnabled && (m_Result == true) && ((!IsInternal) && m_Inventory.SpaceMode == vp_Inventory.Mode.Weight))
			m_Inventory.UsedSpace += ((Count - m_PrevCount) * UnitType.Space);
		m_Inventory.SetDirty();
		return m_Result;
	}


	/// <summary>
	/// 
	/// </summary>
	public virtual bool DoRemoveUnits(int amount)
	{
		// TODO: 'do' shouldn't run 'try'. it should be the other way around
		m_PrevCount = Count;
		m_Result = TryRemoveUnits(amount);
		if (m_Inventory.SpaceEnabled && (m_Result == true) && ((!IsInternal) && m_Inventory.SpaceMode == vp_Inventory.Mode.Weight))
			m_Inventory.UsedSpace = Mathf.Max(0, (m_Inventory.UsedSpace - ((m_PrevCount - Count) * UnitType.Space)));
		m_Inventory.SetDirty();
		return m_Result;
	}


	/// <summary>
	/// clamps the unit count between zero and its capacity and returns
	/// the amount of units shaved off. can be used for cleanup: let's
	/// say the unit count was temporarily boosted above capacity (due
	/// to a spell or powerup) but we have now loaded a new level and
	/// wish to start over fresh
	/// NOTE: does not take inventory 'Space' into consideration!
	/// TODO: expose
	/// </summary>
	public virtual int ClampToCapacity()
	{

		int prevCount = Count;

		if (Capacity != UNLIMITED)
			Count = Mathf.Clamp(Count, 0, Capacity);

		Count = Mathf.Max(0, Count);

		return prevCount - Count;

	}


}

