using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {
	public float power = 1000.0f;
	public GameObject PokeBall;
	Vector3 startPos;
	// Use this for initialization

	void Start(){
		GetComponent<Rigidbody> ().useGravity = false;
	}

	void OnMouseDown () {
		Physics.gravity = new Vector3 (0, -20, 0);
		startPos = Input.mousePosition;
		startPos.z = transform.position.z - Camera.main.transform.position.z;
		startPos = Camera.main.ScreenToWorldPoint (startPos);
	}
	
	void OnMouseUp(){
		Vector3 endPos = Input.mousePosition;
		endPos.z = transform.position.z - Camera.main.transform.position.z;;
		endPos = Camera.main.ScreenToWorldPoint (endPos);
		Vector3 force = endPos - startPos;
		force.z = force.magnitude;
		force.Normalize();
		GetComponent<Rigidbody> ().AddForce (force * power);
		GetComponent<Rigidbody> ().useGravity = true;
		Invoke ("CreatePokeBall", 3.0f);
	}

	void CreatePokeBall(){
		GameObject.Instantiate (PokeBall, new Vector3(0.1f, -2.3f, -36.78f), Quaternion.identity);
		Destroy (gameObject);
	}
}
