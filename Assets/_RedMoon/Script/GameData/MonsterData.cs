using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Polaris.GameData
{
	public class MonsterData : GameData<MonsterData>
	{
		public string Type{ get; protected set;}
		public string BattleModel{ get; protected set;}
		public string StreetModel{ get; protected set;}
		public string Image{ get; protected set;}
		public string Icon{ get; protected set;}
		public string Description{ get; protected set;}
		public int Coin{ get; protected set;}
		public float Speed{ get; protected set;}
		public float Attack{ get; protected set;}
		public float Hp { get; protected set; }

		public MonsterData(){

		}

		static public readonly string fileName = "xml/MonsterData";

		public static MonsterData GetMonsterData(string type) {
			foreach(MonsterData md in MonsterData.dataMap.Values)
				if (md.Type.Equals (type))
					return md;

			return null;
		}
	}
}