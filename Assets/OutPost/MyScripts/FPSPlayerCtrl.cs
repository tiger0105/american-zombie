using UnityEngine;
using System.Collections;

public class FPSPlayerCtrl : MonoBehaviour {

    public float speed = 3.0F;
	Vector3 moveVector;
	
    void Update() 
	{
        CharacterController controller = GetComponent<CharacterController>();

        moveVector = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
        moveVector = transform.TransformDirection(moveVector);
        moveVector *= speed ;
		
        controller.SimpleMove(moveVector);
    }

}
