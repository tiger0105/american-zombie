using UnityEngine;
using System.Collections;

public class TouchSensor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) ) {
			if (Camera.main == null)
				return;

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (!Physics.Raycast (ray, out hit)) {
				Debug.Log ("Raycast fail!");
				return;
			} 
			if(hit.transform.gameObject.Equals(gameObject) && CheckPickable())
				CombatObserver.Instance.PickupItem (gameObject);
		}
	}

	bool CheckPickable() {
		foreach (Item item in TargetManager.Instance.itemList) {
			if (item.playObject.Equals (gameObject)) {
				float distance = Vector2.Distance (new Vector2 (item.pixelX, item.pixelY), new Vector2 (TargetManager.Instance.player.pixelX, TargetManager.Instance.player.pixelY));
				return (distance < 3);
			}
		}

		return false;
	}
}