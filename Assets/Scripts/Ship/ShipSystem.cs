using System;
using UnityEngine;

public class ShipSystem : MonoBehaviour
{
    public float impactSpeedDamageMultiplier = 0.25f;
    public float maxIntegrity = 100f;

    protected Ship ship;
    public float integrity;

    protected virtual void Start()
    {
        ship = GetComponentInParent<Ship>();
        ship.shipSystems.Add(this);

        // Start at full integrity
        integrity = maxIntegrity;
    }

    internal void OnImpact(float speed)
    {
        // Calculate the damage to be done
        var damage = speed * impactSpeedDamageMultiplier;

        // Reduce the integrity of the system (making sure it doesn't go below zero)
        integrity = Mathf.Clamp(integrity - damage, 0, maxIntegrity);
        Debug.Log($"{name} took {damage} damage, integrity: {integrity}");

        // Update the system's damage
        UpdateDamage();
    }

    public virtual void UpdateDamage()
    {
    }
}