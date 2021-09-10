using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public enum ShipQuadrant
{
    FrontLeft,
    FrontRight,
    BackLeft,
    BackRight
}

public enum ShipStatus
{
    Orbit,
    Approach,
    Landed
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

    [SerializeField] private float _landedSpeedThreshold = 0.1f;
    [SerializeField] private float _landedAltitudeThreshold = 0.1f;
    [SerializeField] private float _warpCoreChargePerSecond = 1f;
    [SerializeField] private float _orbitalDistanceThreshold = 750f;
    [SerializeField] private TMP_Text _HUDMessage;
    [SerializeField] private AudioSource _engineSound;
    [SerializeField] private Image _whiteFadeImage;
    [SerializeField] private float _surfaceLateralControlMultiplier = 0.1f;

    public List<ShipSystem> shipSystems = new List<ShipSystem>();
    public float _altitude;
    public bool playerOnboard;

    public static float unitsToMeters = 0.2f;

    private Rigidbody rigidBody;

    public float _warpCoreCharge;
    public float _maxWarpCoreCharge = 100f;
    public ShipStatus _status;
    private Game _game;
    private bool _warping;
    private Music _music;
    private Sounds _sounds;

    void Start()
    {
        _music = FindObjectOfType<Music>();
        _sounds = FindObjectOfType<Sounds>();
        _game = FindObjectOfType<Game>();
        rigidBody = GetComponent<Rigidbody>();
        
        // Start the warp core as empty
        _warpCoreCharge = 0f;
    }

    internal void UpdateTurningControls(float pitch, float yaw)
    {
        // TODO: Replace this with rotation thrusters?
        rigidBody.AddRelativeTorque(pitch * turningForce, yaw * turningForce, 0);
    }

    internal void UpdateLateralControls(Vector3 lateralDirection)
    {
        // If the ship is near the planet surface
        if (_status == ShipStatus.Approach)
            // Reduce the input controls significantly
            lateralDirection *= _surfaceLateralControlMultiplier;
        
        // Calculate the direction of the force in world space
        var forceDirection = transform.TransformDirection(lateralDirection);

        // Apply a force to the ship to move it laterally
        rigidBody.AddForce(forceDirection * engineForce);
        
        // Update the engine sound
        _engineSound.volume = lateralDirection.magnitude;
    }

    internal void ApplyForceAtPosition(Vector3 force, Vector3 position)
    {
        rigidBody.AddForceAtPosition(force, position);
    }

    private void Update()
    {
        // Get the current planet
        var planet = GameObject.FindWithTag("Planet").transform;
        
        // Calculate the direction from the ship to the planet
        var planetDirection = (planet.position - transform.position).normalized;
        
        // Sweep the ship's rigid body toward the planet to find the closest point
        if (rigidBody.SweepTest(planetDirection, out var sweepHit))
        {
            _altitude = sweepHit.distance;
            // Debug.Log($"RB Sweep Altitude: {_altitude}");
        }

        // Determine if the ship is in orbit
        if (_altitude >= _orbitalDistanceThreshold)
        {
            // If the ship was just near the surface
            if (_status == ShipStatus.Approach)
            {
                // Fade out the planet surface music
                _music.FadeOutSurfaceStems(3f);
            }

            // The ship is now in orbit
            _status = ShipStatus.Orbit;
            
            // If the warp core is charged (and we're not already warping)
            if (_warpCoreCharge >= _maxWarpCoreCharge && !_warping)
            {
                DisplayHUDMessage("press B to activate\nwarp drive");
                
                // If B is pressed
                if (Input.GetKey(KeyCode.R) || Input.GetButton("XRI_Right_SecondaryButton"))
                {
                    // Start the warp sequence
                    StartCoroutine(StartWarpSequence());
                }
            }
        }
        // Determine if the ship is landed
        else if (rigidBody.velocity.magnitude < _landedSpeedThreshold / unitsToMeters && _altitude < _landedAltitudeThreshold / unitsToMeters)
        {
            _status = ShipStatus.Landed;
            
            // If the warp core is charged
            if (_warpCoreCharge >= _maxWarpCoreCharge)
            {
                // Tell the player to go into orbit
                DisplayHUDMessage("Return to orbit\nto warp to next planet");
            }
        }
        // Otherwise, the ship is near the surface
        else
        {
            // If the ship was just in orbit
            if (_status == ShipStatus.Orbit)
            {
                // Fade in the planet surface music
                _music.FadeInSurfaceStems(3f);
            }
            
            _status = ShipStatus.Approach;
            
            // Hide the HUD message
            HideHUDMessage();
        }

        // If the ship is landed
        if (_status == ShipStatus.Landed)
        {
            // Recharge the warp core slowly
            _warpCoreCharge = Mathf.Clamp(_warpCoreCharge + _warpCoreChargePerSecond * Time.deltaTime, 0, _maxWarpCoreCharge);
        }
        
        // Find the closest point on the planet from the ship
        // TODO: This isn't a great solution because the pivot of the ship is between the legs and rayscasts keep missing
        // if (Physics.Raycast(transform.position, planetDirection, out var planetHit, float.PositiveInfinity, _planetRaycastMask))
        // {
        //     // Debug.DrawLine(transform.position, planetHit.point, Color.red);
        //     
        //     // Raycast back from the closest point on the planet towards the ship
        //     Debug.DrawLine(planetHit.point, planetHit.point - planetDirection * 200f, Color.green);
        //     if (Physics.Raycast(planetHit.point, -planetDirection, out var shipHit))
        //     {
        //         Debug.DrawLine(planetHit.point, shipHit.point, Color.green);
        //
        //         // Calculate the altitude from the current
        //         _altitude = shipHit.distance;
        //         Debug.Log($"Two-Raycast Altitude: {_altitude}");
        //     }
        // }
        
        // TESTING: Cheat for warping to next planet
        if (Input.GetKeyDown(KeyCode.F5))
        {
            StartCoroutine(StartWarpSequence());
        }
    }

    private void DisplayHUDMessage(string message)
    {
        // Show the message
        _HUDMessage.text = message;
        _HUDMessage.gameObject.SetActive(true);
    }

    private void HideHUDMessage()
    {
        // Hide the HUD message
        _HUDMessage.gameObject.SetActive(false);
    }

    private IEnumerator StartWarpSequence()
    {
        // Start warping
        _warping = true;
        
        // Hide the HUD message
        HideHUDMessage();
        
        // Disable the ship controls
        GetComponent<ShipControls>().enabled = false;

        // Fade out the current music
        _music.FadeOutTrack(3f);

        // Get the length of the warp windup sound
        var warpWindupLength = _sounds.GetSoundLength(Sounds.Type.WarpOut);
        
        // Fade the player's view to white over the windup sequence
        _whiteFadeImage.DOFade(1f, warpWindupLength);
        
        // Play the warp windup sound
        _sounds.PlayAtSource(Sounds.Type.WarpOut, transform);
        yield return new WaitForSeconds(warpWindupLength);
        
        // Move to the next planet
        _game.MoveToNextPlanet();
        
        // Get the length of the warp arrival sound 
        var warpArrivalLength = _sounds.GetSoundLength(Sounds.Type.WarpIn);

        // Play the warp arrival sound
        _sounds.PlayAtSource(Sounds.Type.WarpIn, transform);
        
        // Fade the player's view back in over the length of the sound
        _whiteFadeImage.DOFade(0f, warpArrivalLength);
        
        // Kill the ships movement
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        // Drain the warp core
        _warpCoreCharge = 0;
        
        // Re-enable the ship controls
        GetComponent<ShipControls>().enabled = true;
        
        // Stop warping
        _warping = false;
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