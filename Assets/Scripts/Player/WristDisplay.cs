using TMPro;
using UnityEngine;

public class WristDisplay : MonoBehaviour
{
    public TMP_Text scrapNumber;
    public Transform shipDirectionIndicator;
    public TMP_Text shipDistanceDisplay;

    private Player player;
    private Ship ship;

    void Start()
    {
        player = FindObjectOfType<Player>();
        ship = FindObjectOfType<Ship>();
    }

    void Update()
    {
        // Display the amount of scrap the player has collected
        scrapNumber.text = $"Scrap: {player.scrapCollected:F1}";

        // Calculate the distance and direction to the ship
        var shipDelta = ship.transform.position - transform.position;
        var shipDirection = shipDelta.normalized;
        var shipDistance = shipDelta.magnitude * Ship.unitsToMeters;

        // Rotate the ship direction indicator to point towards the ship
        shipDirectionIndicator.up = shipDirection;

        // Update the distance to the ship indicator
        shipDistanceDisplay.text = $"Ship: {shipDistance:F1}m";
    }
}