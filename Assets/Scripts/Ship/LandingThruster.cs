using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingThruster : ShipSystem
{
    public ParticleSystem particles;
    public ParticleSystem damageParticles;
    public float minParticleSpeed = 0;
    public float maxParticleSpeed = 50;
    public float thrustForce = 5;
    public float maxDamageParticlesEmission = 250f;

    private SoundCrossFade sounds;

    protected override void Start()
    {
        base.Start();

        sounds = GetComponent<SoundCrossFade>();
    }

    internal void UpdateThrustPercent(float percentThrust)
    {
        // Reduce the possible thrust percent by the integrity percent
        percentThrust *= integrity / maxIntegrity;

        // Set the speed of the exhaust particles
        var main = particles.main;
        main.startSpeedMultiplier = Mathf.Lerp(minParticleSpeed, maxParticleSpeed, percentThrust);

        // Apply the thrust force to the ship at the position of this thruster
        ship.ApplyForceAtPosition(transform.up * thrustForce * percentThrust, transform.position);

        // Update the thruster sounds
        sounds.SetVolume(percentThrust);
        sounds.SetCrossFade(percentThrust);
    }

    public override void UpdateDamage()
    {
        base.UpdateDamage();

        // Calculate the precentage of integrity left
        var integrityPercent = integrity / maxIntegrity;

        // Update the fire particles based on the integrity
        var emission = damageParticles.emission;
        // TODO: Fix the error here
        //emission.rateOverTimeMultiplier = Mathf.Lerp(0, maxDamageParticlesEmission, 1f - integrityPercent);

        // TODO: Set the volume of the damage sound
    }
}
