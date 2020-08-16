//#define EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if(EDITOR)
    using System.Xml;
#endif

namespace Polaris.GameData
{
    public class GameProperty: GameData<GameProperty>
    {
        public string Name { get; set; }
        public string DispName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public GameProperty() {}
        static public readonly string fileName = "xml/GamePropertyData";
    }

    public class WeaponData : GameData<WeaponData>
    {
        public string Name { get; set; }
        public int Price { get; set; } 
        public string ShopModel { get; set; }
		public string PickUpModel{ get; set;}
        public string Image { get; set; }
        public string Icon { get; set; }
        public int    Kind { get; set; }
        public string Description { get; set; }
        public List<string> PropertyIdList { get;  set; }
        public List<string> PropertyValList { get; set; }

        public WeaponData()
        {
            PropertyIdList = new List<string>();
            PropertyValList = new List<string>();
        }

		public string GetProperty(string prop_str) {
			for (int i = 0; i < PropertyValList.Count; i++)
				if (PropertyIdList [i] == prop_str)
					return PropertyValList [i];

			return PropertyValList [0];
		}

		public int GetMaxPower() {
			int min_power = GetMinPower ();
			int max_power = (int)(GameInfo.maxPowerRate * min_power);
			return max_power;
		}

		public int GetMinPower() {
			float fire_rate = float.Parse(GetProperty ("fire_rate"));
			float damage = float.Parse (GetProperty ("damage"));
			int min_power = (int)(damage / fire_rate * GameInfo.powerRate);
			return min_power;
		}

        public float GetFireRate()
        {
            return float.Parse(GetProperty("fire_rate"));
        }

        public int GetDamage()
        {
            return int.Parse(GetProperty("damage"));
        }

        public int GetRange()
        {
            return int.Parse(GetProperty("range"));
        }

        public float GetReloadSpeed()
        {
            return float.Parse(GetProperty("reload_speed"));
        }

        public int GetAmmoCapacity()
        {
            return int.Parse(GetProperty("ammo_capacity"));
        }

        public float GetFireRate_LevelUp()
        {
            return float.Parse(GetProperty("fire_rate_level"));
        }

        public int GetDamage_LevelUp()
        {
            return int.Parse(GetProperty("damage_level"));
        }

        public int GetRange_LevelUp()
        {
            return int.Parse(GetProperty("range_level"));
        }

        public float GetReloadSpeed_LevelUp()
        {
            return float.Parse(GetProperty("reload_speed_level"));
        }

        public int GetAmmoCapacity_LevelUp()
        {
            return int.Parse(GetProperty("ammo_capacity_level"));
        }

        static public readonly string fileName = "xml/WeaponData";

		public static WeaponData GetWeaponData(string name) {
			foreach (WeaponData wd in WeaponData.dataMap.Values)
				if(wd.Name.Equals(name))
					return wd;
				
			return null;
		}
    }

    public class ItemData : GameData<ItemData>
    {
        public string 	Name 		{ get; set; }
        public int 		Kind 		{ get; set; }
        public float	Price 		{ get; set; }
        public string 	ShopModel 	{ get; set; }
		public string 	PickUpModel { get; set;}
        public string 	Image 		{ get; set; }
        public string 	Icon 		{ get; set; }
        public string 	Description { get; set; }

        public List<string> PropertyIdList 	{ get; set; }
        public List<string> PropertyValList { get; set; }

        public ItemData()
        {
            PropertyIdList = new List<string>();
            PropertyValList = new List<string>();
        }

		public string GetProperty(string prop_str){
			for (int i = 0; i < PropertyValList.Count; i++) {
				if (PropertyIdList [i] == prop_str) {
					return PropertyValList [i];
				} 
			}
			return PropertyValList [0];
		}
        static public readonly string fileName = "xml/ItemData";
		public static ItemData GetDataFrom(string name){
			int count = ItemData.dataMap.Count;
			for(int i = 0; i<ItemData.dataMap.Count; i++){
				ItemData data = ItemData.dataMap [i+1];
				if(data.Name.Equals(name)){
					return data;
				}
			}
			return null;
		}
    }
}