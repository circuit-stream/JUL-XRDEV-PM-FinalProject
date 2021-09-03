using System;
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

    private Rigidbody rigidBody;
    internal bool playerOnboard;

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
}
