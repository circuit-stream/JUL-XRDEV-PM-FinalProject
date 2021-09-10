using System;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _speedDisplay;
    [SerializeField] private TMP_Text _altitudeDisplay;
    [SerializeField] private TMP_Text _landedDisplay;
    [SerializeField] private Transform _velocityDirectionIndicator;

    private Ship _ship;
    private Rigidbody _shipRigidBody;

    private void Awake()
    {
        _ship = FindObjectOfType<Ship>();
        _shipRigidBody = _ship.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Update the ship's speed and altitude displays
        _speedDisplay.text = $"Speed: {_shipRigidBody.velocity.magnitude * Ship.unitsToMeters:F1}m/s";
        _altitudeDisplay.text = $"Altitude: {_ship._altitude * Ship.unitsToMeters:F1}m";
        _landedDisplay.text = $"Status: {_ship._status}";
        
        // Update the ship's velocity direction indicator
        _velocityDirectionIndicator.up = _shipRigidBody.velocity.normalized;
    }
}
