using System;
using System.Collections.Generic;
using UnityEngine;

public enum ShipQuadrant
{
    FrontLeft,
    FrontRight,
    BackLeft,
    BackRight
}

public class Ship : MonoBehaviour
{
    public float turningForce = 1;
    public float engineForce = 1;
    public LandingThruster frontLeftLanderthruster;
    public LandingThruster frontRightLanderthruster;
    public LandingThruster backLeftLanderthruster;
    public LandingThruster backRightLanderthruster;
    public float nonDirectHitDamageMultiplier = 0.25f;

    private Rigidbody rigidBody;
    internal bool playerOnboard;
    
    public List<ShipSystem> shipSystems = new List<ShipSystem>();

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    internal void UpdateTurningControls(float pitch, float yaw)
    {
        // TODO: Replace this with rotation thrusters?
        rigidBody.AddRelativeTorque(pitch * turningForce, yaw * turningForce, 0);
    }

    internal void UpdateLateralControls(Vector3 lateralDirection)
    {
        // Calculate the direction of the force in world space
        var forceDirection = transform.TransformDirection(lateralDirection);

        // Apply a force to the ship to move it laterally
        rigidBody.AddForce(forceDirection * engineForce);
    }

    internal void ApplyForceAtPosition(Vector3 force, Vector3 position)
    {
        rigidBody.AddForceAtPosition(force, position);
    }

    internal void UpdateLandingThrusterControls(float percentThrust, ShipQuadrant quadrant)
    {
        // Get the requested landing thruster
        var thruster = GetLandingThruster(quadrant);

        // Tell the thruster to apply the percentage of thrust
        thruster.UpdateThrustPercent(percentThrust);
    }

    private LandingThruster GetLandingThruster(ShipQuadrant quadrant)
    {
        switch (quadrant)
        {
            case ShipQuadrant.FrontLeft:
                return frontLeftLanderthruster;
            case ShipQuadrant.FrontRight:
                return frontRightLanderthruster;
            case ShipQuadrant.BackLeft:
                return backLeftLanderthruster;
            case ShipQuadrant.BackRight:
                return backRightLanderthruster;
            default:
                throw new ArgumentOutOfRangeException(nameof(quadrant), quadrant, null);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Find out what part of the ship was hit
        var colliderHit = collision.contacts[0].thisCollider;

        // If the collider on the ship that was hit was system assosiated with it
        var shipSystem = colliderHit.GetComponent<ShipSystem>();
        if(shipSystem != null)
        {
            // Let that system know it was hit and how hard (== how fast)
            shipSystem.OnImpact(collision.relativeVelocity.magnitude);
        }
        // If no particular system was hit
        else
        {
            // Get a random ship system
            var randomShipSystem = shipSystems[UnityEngine.Random.Range(0, shipSystems.Count)];

            // Impact that system but with less force than a direct hit
            randomShipSystem.OnImpact(collision.relativeVelocity.magnitude * nonDirectHitDamageMultiplier);
        }
    }
}
