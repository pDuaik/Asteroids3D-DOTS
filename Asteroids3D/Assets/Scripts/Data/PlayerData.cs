using Unity.Entities;
using Unity.Mathematics;
using System;

[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    // Missile entity
    public Entity missile;

    // Gameplay variables
    public float rotationSpeed;
    public float acceleration;
    //public bool doubleShot;
    //public float3 powerUpPosition;
    //public float powerUpRadius;

    // Missile shooting cooldown
    public float shootingCooldownTime;

    // Trackers:
    [NonSerialized]
    public float currentShootingCooldownTime;
    [NonSerialized]
    public float3 currentThrust;
    [NonSerialized]
    public float3 currentVelocity;
}
