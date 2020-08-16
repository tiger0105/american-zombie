using UnityEngine;
using System.Collections;

public class Target1 : MonoBehaviour {
	public GameObject particle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision collision){
		Debug.Log (collision.gameObject.tag);
		if (collision.gameObject.CompareTag("Poke"))
		{
			GameObject obj = GameObject.Instantiate(particle, collision.transform.position, Quaternion.identity) as GameObject;
			Destroy(obj, 3.0f);
		}
	}
}