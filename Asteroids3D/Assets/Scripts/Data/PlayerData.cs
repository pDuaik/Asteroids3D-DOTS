using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    // Missile Variables
    public float missileLifeSpan;
    public float missileSpeed;

    // Gameplay variables
    public float rotationSpeed;
    public float acceleration;

    // Missile shooting cooldown
    public float shootingCooldownTime;

    // Power-Up
    public bool powerUp;
    public bool shield;

    // Trackers:
    public float currentShootingCooldownTime;
    public float3 currentThrust;
    public float3 currentVelocity;

    // When a collision happens
    public Entity hit;
}
