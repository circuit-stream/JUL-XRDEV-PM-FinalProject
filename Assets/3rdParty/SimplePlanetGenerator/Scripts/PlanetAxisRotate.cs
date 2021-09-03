using UnityEngine;
using System.Collections;

public class PlanetAxisRotate : MonoBehaviour {

	public float rotationSpeed = 10f;
	public int selectedAxis = 0;
	
	// Update is called once per frame
	void FixedUpdate () {
		RotateOnSelectedAxs ();
	}

	void RotateOnSelectedAxs(){
		switch (selectedAxis) {
		case 0:
			transform.RotateAround (transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
			break;
		case 1:
			transform.RotateAround (transform.position, Vector3.right, rotationSpeed * Time.deltaTime);
			break;
		case 2: 
			transform.RotateAround (transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
			break;
		}
	}
}
