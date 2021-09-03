using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingThruster : MonoBehaviour
{
    public ParticleSystem particles;
    public float minParticleSpeed = 0;
    public float maxParticleSpeed = 50;
    public float thrustForce = 5;

    private Ship ship;
    private SoundCrossFade sounds;

    void Start()
    {
        ship = GetComponentInParent<Ship>();
        sounds = GetComponent<SoundCrossFade>();
    }

    internal void UpdateThrustPercent(float percentThrust)
    {
        // Set the speed of the exhaust particles
        var main = particles.main;
        main.startSpeedMultiplier = Mathf.Lerp(minParticleSpeed, maxParticleSpeed, percentThrust);

        // Apply the thrust force to the ship at the position of this thruster
        ship.ApplyForceAtPosition(transform.up * thrustForce * percentThrust, transform.position);

        // Update the thruster sounds
        sounds.SetVolume(percentThrust);
        sounds.SetCrossFade(percentThrust);
    }
}
