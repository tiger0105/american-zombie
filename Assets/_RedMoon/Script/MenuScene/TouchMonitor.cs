using UnityEngine;
using System.Collections;

public class TouchMonitor : MonoBehaviour {
	public MenuSceneControl menuSenceControl;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) ) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(!Physics.Raycast(ray, out hit) ) {
				Debug.Log("Raycast fail!");
				return;
			} 
			menuSenceControl.TriggerState (hit.collider.gameObject);
		}
	}
}
