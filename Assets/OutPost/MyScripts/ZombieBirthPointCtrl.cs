using UnityEngine;
using System.Collections;

public class ZombieBirthPointCtrl : MonoBehaviour {
	
	public Rigidbody prefabZombie;
	Rigidbody clone;
	public bool myZombieIsDead = true;

	
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ((Vector3.Distance(GameObject.FindWithTag("Player").transform.position ,transform.position) > 40) && (myZombieIsDead))
		{
			clone = Instantiate(prefabZombie,transform.position,transform.rotation) as Rigidbody;
			ZombieCtrl cloneScript = clone.GetComponent<ZombieCtrl>();			
			cloneScript.createPoint = transform; //Assign mySelf as the "birth point" to the Zombie
			myZombieIsDead = false;
		}	
	}
}
