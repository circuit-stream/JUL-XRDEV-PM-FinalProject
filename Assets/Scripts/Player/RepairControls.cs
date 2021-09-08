using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepairControls : MonoBehaviour
{
    public float repairRange = 50f;
    public float percentDistanceToDamageReport = 0.75f;
    public TMP_Text damageDisplay;
    public Transform hoverOverUI;
    public Vector3 damageDisplayOffset;

    private Player player;
    private ShipSystem targetedShipSystem;
    private Vector3 damageReportPosition;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        // If the player is looking at something
        if (Physics.Raycast(transform.position, transform.forward, out var hit, repairRange))
        {
            // If the player is looking at a specific ship system
            var shipSystem = hit.collider.GetComponent<ShipSystem>();
            if (shipSystem != null)
            {
                // Store the targeted system
                targetedShipSystem = shipSystem;

                // Calculate and store the position of the damage report
                damageReportPosition = transform.position + (targetedShipSystem.transform.position - transform.position) * percentDistanceToDamageReport;
            }
            // If the player isn't not looking at a ship system
            else
            {
                // No longer targeting anything
                targetedShipSystem = null;
            }
        }
        // Not looking at anything
        else
        {
            // No longer targeting anything
            targetedShipSystem = null;
        }

        // If we're targeting a ship system
        if(targetedShipSystem)
        {
            // Set the damage report text
            damageDisplay.text = $"<b>{targetedShipSystem.name}</b>\n{targetedShipSystem.integrity / targetedShipSystem.maxIntegrity * 100f:F0}%\nPress B to repair";

            // Set the damage report's position
            hoverOverUI.transform.position = damageReportPosition + Camera.main.transform.TransformVector(damageDisplayOffset);

            // Show the damage report
            hoverOverUI.gameObject.SetActive(true);

            // If the player is pressing B
            if (Input.GetKey(KeyCode.R) || Input.GetButton("XRI_Right_SecondaryButton"))
            {
                // Tell the player to repair the targeted ship system
                player.RepairShipSystem(targetedShipSystem);
            }
        }
        // If we're not targeting a ship system
        else
        {
            // Hide the damage report
            hoverOverUI.gameObject.SetActive(false);
        }
    }
}
