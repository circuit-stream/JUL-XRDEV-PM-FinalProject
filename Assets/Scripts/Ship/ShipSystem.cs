using System;
using UnityEngine;

public class ShipSystem : MonoBehaviour
{
    public float impactSpeedDamageMultiplier = 0.25f;
    public float maxIntegrity = 100f;
    [SerializeField] private float _maxImpactSpeed;
    [SerializeField] private float _impactAlarmSpeedThreshold = 50f;
    [SerializeField] public AudioSource _damageSound;

    protected Ship ship;
    public float integrity;
    private Sounds _sounds;

    protected virtual void Start()
    {
        ship = GetComponentInParent<Ship>();
        ship.shipSystems.Add(this);

        _sounds = FindObjectOfType<Sounds>();

        // Start at full integrity
        integrity = maxIntegrity;
    }

    internal void OnImpact(float speed)
    {
        // Calculate the damage to be done
        var damage = speed * impactSpeedDamageMultiplier;

        // Reduce the integrity of the system (making sure it doesn't go below zero)
        integrity = Mathf.Clamp(integrity - damage, 0, maxIntegrity);
        Debug.Log($"{name} took {damage} damage because of impact with speed {speed}, integrity: {integrity}");
        
        // Play an impact sound with the volume based on impact speed
        _sounds.PlayAtSource(Sounds.Type.Impact, transform, Mathf.Clamp(speed / _maxImpactSpeed, 0f, 1f));
        
        // If the impact was hard enough
        if(speed >= _impactAlarmSpeedThreshold)
            _sounds.PlayAtSource(Sounds.Type.Alarm, transform);

        // Update the system's damage
        UpdateDamage();
    }

    public virtual void UpdateDamage()
    {
        // Calculate the precentage of integrity left
        var integrityPercent = integrity / maxIntegrity;
        
        // Set the volume of the damage sounds
        _damageSound.volume = 1f - integrityPercent;
    }
}