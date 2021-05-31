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

    // Missile shooting cooldown
    public float shootingCooldownTime;

    // Power-Up
    public bool powerUp;

    // Trackers:
    [NonSerialized]
    public float currentShootingCooldownTime;
    [NonSerialized]
    public float3 currentThrust;
    [NonSerialized]
    public float3 currentVelocity;
}
