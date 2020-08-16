using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Polaris.GameData
{
	public class DailyBonusData : GameData<DailyBonusData>
	{
		public int Id { get; protected set; }
		public int Coin { get; protected set; }
		public int ItemKind { get; protected set; }
		public int ItemID { get; protected set; }

		public DailyBonusData(){

		}

		static public readonly string fileName = "xml/DailyBonusData";
	}
}