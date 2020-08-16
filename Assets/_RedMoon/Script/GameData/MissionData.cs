using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Polaris.GameData
{
	public class MissionData : GameData<MissionData>
	{
		public int Id{ get; protected set;} 
		//public data map elements
		public string Type {get; protected set;}
		public int Index{ get; protected set;}
		public string Name{ get; protected set;}
		public string Image{ get; protected set;}
		public string Icon{ get; protected set;}
		public string Description{ get; protected set;}
		public int Coin{ get; protected set;}
		public float performTime{ get; protected set;}
		public List<string> MonsterIdList{ get; protected set;}
		public List<int> MonsterCountList{ get; protected set;}
		public List<string> WeaponIdList{ get; protected set;}
		public List<int> WeaponCountList{ get; protected set;}
		public List<string> ItemIdList{ get; protected set;}
		public List<int> ItemCountList{ get; protected set;}
		//private 
		private List<int> allMonsterCountList;
		private List<int> allWeaponCountList;
		private List<int> allItemCountList;

		public MissionData(){

		}
//		public int GetMonsterCount(MonsterTypes mon_type){
//			for(int i = 0; i<MonsterIdList.Count; i++){
//				if(MonsterIdList[i].Equals(mon_type.ToString())){
//					return MonsterCountList [i];
//				}
//			}
//			return 0;
//		}
//		public List<int> GetMonsterCountList(){
//			if (allMonsterCountList == (null)) {
//				allMonsterCountList = new List<int>();
//				for (int i = 0; i < (int)MonsterTypes.COUNT; i++) {
//					int count = GetMonsterCount ((MonsterTypes)i);
//					allMonsterCountList.Add (count);
//				}
//			} 
//			return allMonsterCountList;
//		}
//		public int GetWeaponCount(WeaponTypes wea_type){
//			for(int i = 0; i<WeaponIdList.Count; i++){
//				if(MonsterIdList[i].Equals(wea_type.ToString())){
//					return WeaponCountList [i];
//				}
//			}
//			return 0;
//		}
//		public List<int> GetWeaponCountList(){
//			if (allWeaponCountList== null) {
//				allWeaponCountList = new List<int>();
//				for (int i = 0; i < (int)WeaponTypes.COUNT; i++) {
//					int count = GetWeaponCount ((WeaponTypes)i);
//					allWeaponCountList.Add (count);
//				}
//			} 
//			return allWeaponCountList;
//		}
//		public int GetItemCount(ItemTypes ite_type){
//			for(int i = 0; i<ItemIdList.Count; i++){
//				if(ItemIdList[i].Equals(ite_type.ToString())){
//					return ItemCountList [i];
//				}
//			}
//			return 0;
//		}
//		public List<int> GetItemCountList(){
//			if (allItemCountList== null) {
//				allItemCountList = new List<int>();
//				for (int i = 0; i < (int)ItemTypes.COUNT; i++) {
//					int count = GetItemCount ((ItemTypes)i);
//					allItemCountList.Add (count);
//				}
//			} 
//			return allItemCountList;
//
//		}
		public int GetMissionKills(){
			int kills = 0;
			for (int i = 0; i < MonsterCountList.Count; i++) {
				kills += MonsterCountList [i];
			}
			return kills;
		}
		public int GetMissionPickups(){
			int pickups = 0;
			for (int i = 0; i < WeaponCountList.Count; i++) {
				pickups += WeaponCountList [i];
			}
			for (int i = 0; i < ItemCountList.Count; i++) {
				pickups += ItemCountList [i];
			}
			return pickups;
		}
		public float GetMissionHP(){
			return GetMissionDifficulty() * 100f;
		}
		public int GetMissionHeadShots(){
			return (int)(GetMissionDifficulty()*GetMissionKills());
		}
		public float GetMissionTime(){
			return GameInfo.maxMissionTime / GetMissionDifficulty();
		}
		public float GetMissionDifficulty(){
			float difficulty = GameInfo.minDifficulty + (GameInfo.maxDifficulty - GameInfo.minDifficulty) / (GameInfo.maxMission - GameInfo.minMission) * (MissionProgress.GetMissionCurIndex()-GameInfo.minMission);
			return difficulty;
		}
		static public readonly string fileName = "xml/MissionData";
		private static List<string> missionTypeList;
		private static List<int> missionCountList;
		/// <summary>
		/// return the list of mission type ,ie. {"Main","Supply","Boss"}
		/// </summary>
		/// <returns>The mission type list.</returns>
		public static List<string> GetMissionTypeList(){
			if(MissionData.dataMap.Count.Equals(0)){
				return null;
			}
			else if(missionTypeList == null){
				missionTypeList = new List<string> ();
				for (int i = 0; i < MissionData.dataMap.Count; i++) {
					MissionData md = MissionData.dataMap [i];
					if(missionTypeList.Contains(md.Type).Equals(false)){
						missionTypeList.Add (md.Type);
					}
				}
			}
			return missionTypeList;
		}
		/// <summary>
		/// return the list of mision count i.e {3,4,1}
		/// </summary>
		/// <returns>The mission count list.</returns>
		public static List<int> GetMissionCountList(){
			if(MissionData.dataMap.Count.Equals(0)){
				return null;
			}else if(missionCountList == null){
				missionCountList = new List<int> ();

				List<string> stringlist = GetMissionTypeList ();
				for(int i = 0; i< stringlist.Count; i++){
					missionCountList.Add (0);  
					for (int j = 0; j < MissionData.dataMap.Count; j++) {
						MissionData md = MissionData.dataMap [j];
						if(md.Type.Equals(stringlist[i])){
							missionCountList [i]++;
						}
					}
				}
			}
			return missionCountList;
		}
		public static int GetMissionTypeCount(){
			return GetMissionTypeList ().Count; 
		}
		public static MissionData GetMissionData (int mission_type, int mission_index){
			for (int i = 0; i < MissionData.dataMap.Count; i++) {
				MissionData md = MissionData.dataMap [i];
				string str = missionTypeList [mission_type];
				if(md.Type.Equals(str) && md.Index.Equals(mission_index)){
					return md;
				}
			}
			return null;
		}
		public static int GetMissionCount(int mission_type){
			return GetMissionCountList()[mission_type];
		}
	}
}