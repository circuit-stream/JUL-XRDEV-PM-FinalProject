using UnityEngine;
using System.Collections;

public class PlanetGravity : MonoBehaviour {

	public float radius = 20f;
	public float strength = 10f;
	public bool faceSurface = false;

	void Update () {

		Collider[] objects = Physics.OverlapSphere (transform.position, radius);
		foreach (Collider col in objects) {
			if (col.GetComponent<Rigidbody> ()) { //Must be rigidbody
				applyGravity (col);
			}
		}
			
	}

	void applyGravity(Collider col){
		float dynamicDistance = Mathf.Abs( (Vector3.Distance (transform.position, col.transform.position) ) - (radius) );
		float multiplier = dynamicDistance / radius;

		col.GetComponent<Rigidbody> ().AddForce ( (strength * (transform.position - col.transform.position).normalized) * multiplier, ForceMode.Force);

		if (faceSurface) {
			col.transform.LookAt (transform.position, col.transform.up);
		}
	}
}
