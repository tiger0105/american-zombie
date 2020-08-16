using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Polaris.GameData
{
	public class PurchaseData : GameData<PurchaseData>
	{
		public int Id{ get; protected set;} 
		//associated with UltimateMobileSettings class
		public string ProductId{ get; protected set;} 
		public string DisplayName{ get; protected set;}
		public int ProductType{ get; protected set;}
		public int PriceTier{ get; protected set;}
		public string Image{ get; protected set;}
		public string Description{ get; protected set;}
		public string AndroidId{ get; protected set;}
		public string IOSId{ get; protected set;}
		public string WP8Id{ get; protected set;}
		//associated with game logic
		public int Coin{get; protected set;}
		public PurchaseData(){

		} 
		static public readonly string fileName = "xml/PurchaseData";
	}
}