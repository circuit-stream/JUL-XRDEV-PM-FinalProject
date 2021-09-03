using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Ship ship;
    public GameObject planetRig;
    public GameObject shipRig;
    public Transform shipExitPoint;

    void Start()
    {
        // Start inside the ship
        EnterShip();
    }

    void Update()
    {
        // If the E key or button are pressed
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("XRI_Right_PrimaryButton"))
        {
            // If the player is on the ship
            if(ship.playerOnboard)
            {
                // Leave the ship
                LeaveShip();
            }
            // If the player is on the planet
            else
            {
                // Enter the ship
                EnterShip();
            }
        }
    }

    private void LeaveShip()
    {
        // Place the planet XR rig at the exit point of the ship
        planetRig.transform.position = shipExitPoint.position;
        planetRig.transform.rotation = shipExitPoint.rotation;

        // Disable the ship XR rig
        shipRig.SetActive(false);

        // Enable the planet XR rig
        planetRig.SetActive(true);

        // Let the ship know the player is no longer onboard
        ship.playerOnboard = false;
    }

    private void EnterShip()
    {
        // Disable the planet XR rig
        planetRig.SetActive(false);

        // Enable the ship XR rig
        shipRig.SetActive(true);

        // The player is now on the ship
        ship.playerOnboard = true;
    }
}
