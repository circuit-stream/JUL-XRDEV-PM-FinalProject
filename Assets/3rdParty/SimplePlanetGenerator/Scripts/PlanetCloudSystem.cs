using UnityEngine;
using System.Collections;

public class PlanetCloudSystem : MonoBehaviour {

	public float spawnAltitude = 10f;
	public int cloudDensity = 20;
	public GameObject cloudPrefab;

	void Start(){
		if (cloudPrefab != null) {
			for (int i = 0; i < cloudDensity; i++) {
				Vector3 randomPointOnSurface = transform.position + (Random.onUnitSphere * spawnAltitude);

				GameObject currentCloud = (GameObject) Instantiate (cloudPrefab, randomPointOnSurface, new Quaternion ());

				currentCloud.transform.parent = transform;
			}
		}
	}
}
