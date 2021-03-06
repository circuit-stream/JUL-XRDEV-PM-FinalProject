using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSurfaceGravity : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Get the current planet
        var planet = GameObject.FindWithTag("Planet").transform;
        
        // Calculate the up direction from where this object is on the planet
        var planetUp = (transform.position - planet.position).normalized;

        // Calculate the rotation needed for this object's up to match the planet's up
        var targetRotation = Quaternion.FromToRotation(transform.up, planetUp) * transform.rotation;

        // Blend to that rotation, this way this object will always stand upright on the planet
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);

        // Could have also just done this:
        //transform.rotation = targetRotation;
    }
}
