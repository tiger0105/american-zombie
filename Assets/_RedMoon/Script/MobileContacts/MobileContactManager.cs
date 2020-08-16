//using UnityEngine;
//using System.Collections;
//
//public class MobileContactManager {
////	public static MobileContactManager Instance
////	{
////		get
////		{
////			if (instance == null)
////			{
////				instance = (MobileContactManager) FindObjectOfType (typeof (MobileContactManager));
////			}
////			return instance;
////		}
////	}
//	public int GetContactCount(){
//		if (Contacts.ContactsList != null) {
//			return Contacts.ContactsList.Count;
//		}else {
//			return -1;  
//		}
//	}
//	public Contact GetContactInfo(int id){
//		Contact contact;
//		contact = Contacts.ContactsList[id];
//		return contact;
//	}
//	public bool IsReady{
//		get{ return isReady;}
//	}
//	static MobileContactManager instance;
//	string failString;
//	int contactCount;
//	bool isReady;
//	void Awake ()
//	{
//		if (Instance != this)
//		{
//			Destroy (gameObject);
//			Debug.Log ("DestroyedObjectPersist");
//		}
//		else
//		{
//			DontDestroyOnLoad (gameObject);
//		}
//	}	
//	IEnumerator Start(){
//		failString = string.Empty;
//		while (failString == string.Empty) {
//			yield return new WaitForSeconds (.5f);
//		}
//		Contacts.LoadContactList( onDone, onLoadFailed );
//	}
//	void onLoadFailed( string reason )
//	{
//		failString = reason;
//		isReady = false;
//	}
//
//	void onDone()
//	{
//		isReady = true;
//		failString = "success";
//		contactCount = Contacts.ContactsList.Count;
//	}
//
//}
