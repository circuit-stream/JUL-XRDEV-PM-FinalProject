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
    public float shipBoardingDistance = 10;
    public float scrapCollected;
    public float scrapPerIntegrity = 0.1f;
    
    private Game _game;
    private Sounds _sounds;

    void Start()
    {
        _game = FindObjectOfType<Game>();
        _sounds = FindObjectOfType<Sounds>();
        
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
            // If the player is on the planet (and nearby the ship)
            else if(Vector3.Distance(ship.transform.position, planetRig.transform.position) <= shipBoardingDistance / Ship.unitsToMeters)
            {
                // Enter the ship
                EnterShip();
            }
        }
        
        // TESTING: Cycle through the planets
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _game.MoveToNextPlanet();
        }
        
        // TESTING: Cheat for lots of scrap
        if (Input.GetKeyDown(KeyCode.F2))
        {
            scrapCollected += 10f;
        }
    }

    internal void OnScrapCollected(Scrap scrap)
    {
        // Play the scrap pickup sound
        _sounds.PlayAtSource(Sounds.Type.Pickup, transform);

        // Add the scrap to the inventory
        scrapCollected += 1;
        Debug.Log($"{scrapCollected} scrap collected");

        // Remove the scrap
        Destroy(scrap.gameObject);
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

    internal void RepairShipSystem(ShipSystem shipSystem)
    {
        // Calculate the integrity there is to repair on the ship system
        var integrityToRepair = shipSystem.maxIntegrity - shipSystem.integrity;

        // Calculate how much scrap it will take to repair it
        var scrapToRepair = integrityToRepair * scrapPerIntegrity;

        // Calculate the maximum amount of scrap the player can afford to repair the system
        var scrapAvailableToRepair = Mathf.Min(scrapCollected, scrapToRepair);

        // Play the repair sound (using the same volume as the damaged system's damage sound)
        _sounds.PlayAtSource(Sounds.Type.ExtinguishFire, transform, shipSystem._damageSound.volume);
        
        // Repair the ship system as much as possible
        shipSystem.integrity += scrapAvailableToRepair / scrapPerIntegrity;
        shipSystem.UpdateDamage();

        // Spend the scrap
        scrapCollected -= scrapAvailableToRepair;
    }
}
