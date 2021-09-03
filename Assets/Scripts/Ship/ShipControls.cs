using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ShipControls : MonoBehaviour
{
    public float sensistivity = 1f;

    private bool usingVR;
    private Ship ship;

    void Start()
    {
        // Grab the ship component
        ship = GetComponent<Ship>();

        // Are we using VR?
        usingVR = Util.IsVRPresent();
    }

    void Update()
    {
        // Don't control the ship when the player's not on it
        if (!ship.playerOnboard)
            return;

        // We'll need horizontal and vertical inputs for turning
        float horizontal;
        float vertical;

        // We'll need a movement direction for lateral motion
        var lateralDirection = new Vector3();

        // We'll need thrust amount for the landing thrusters
        float frontLeftThrust;
        float frontRightThrust;
        float backLeftThrust;
        float backRightThrust;

        // VR
        if (usingVR)
        {
            // Turn using the right thumbstick
            horizontal = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal") * sensistivity;
            vertical = -Input.GetAxis("XRI_Right_Primary2DAxis_Vertical") * sensistivity;

            // Move using the left thumbstick
            lateralDirection = new Vector3(Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal"), 0, -Input.GetAxis("XRI_Left_Primary2DAxis_Vertical"));

            // Control the landing thrusters with the triggers and grips
            frontLeftThrust = Input.GetAxis("XRI_Left_Trigger");
            frontRightThrust = Input.GetAxis("XRI_Right_Trigger");
            backLeftThrust = Input.GetAxis("XRI_Left_Grip");
            backRightThrust = Input.GetAxis("XRI_Right_Grip");
        }
        // Non-VR
        else
        {
            // Turn using the mouse
            horizontal = Input.GetAxis("Mouse X") * sensistivity;
            vertical = -Input.GetAxis("Mouse Y") * sensistivity;

            // Move using WASD
            if (Input.GetKey(KeyCode.W))
                lateralDirection.z = 1;
            if (Input.GetKey(KeyCode.S))
                lateralDirection.z = -1;
            if (Input.GetKey(KeyCode.A))
                lateralDirection.x = -1;
            if (Input.GetKey(KeyCode.D))
                lateralDirection.x = 1;

            // Control the landing thrusters with the space bar
            if (Input.GetKey(KeyCode.Space))
                frontLeftThrust = frontRightThrust = backLeftThrust = backRightThrust = 1f;
            else
                frontLeftThrust = frontRightThrust = backLeftThrust = backRightThrust = 0f;
        }

        // Tell the ship to turn using the inputs
        ship.UpdateTurningControls(vertical, horizontal);

        // Tell the ship to move laterally using the inputs
        ship.UpdateLateralControls(lateralDirection);

        // Tell the ship to use the landing thrusters using the input
        ship.UpdateLandingThrusterControls(frontLeftThrust, ShipQuadrant.FrontLeft);
        ship.UpdateLandingThrusterControls(frontRightThrust, ShipQuadrant.FrontRight);
        ship.UpdateLandingThrusterControls(backLeftThrust, ShipQuadrant.BackLeft);
        ship.UpdateLandingThrusterControls(backRightThrust, ShipQuadrant.BackRight);
    }
}
