using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetObjectSpawner : MonoBehaviour
{
    public float minNumber;
    public float maxNumber;
    public float spawnDistance;
    public Rigidbody objectPrefab;
    public float distanceToFall = 1f;
    public Transform parent;

    void Start()
    {
        // Determine how many of the object to spawn
        var number = Random.Range(minNumber, maxNumber);

        // For each object to spawn
        for (int i = 0; i < number; i++)
        {
            // Spawn the object in orbit somewhere random
            var newObject = Instantiate(objectPrefab, transform.position + Random.onUnitSphere * spawnDistance, Random.rotation);

            // Calculate the direction from the object to the planet
            var planetDirection = (transform.position - newObject.transform.position).normalized;

            // TODO: Keep an eye on this if things start getting slowly
            if(newObject.SweepTest(planetDirection, out var hit))
            {
                // Move the object near to the surface of the planet
                newObject.transform.position = hit.point - planetDirection * distanceToFall;
            }
            else
            {
                Debug.LogWarning($"Trying to place {newObject.name} on the planet surface failed");
                Destroy(newObject.gameObject);
            }

            // Set the object's parent
            newObject.transform.SetParent(parent, true);
        }
    }
}
