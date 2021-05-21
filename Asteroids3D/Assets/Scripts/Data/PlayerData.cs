using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    // Gameplay variables
    public float rotationSpeed;
    public float acceleration;

    public float3 currentVelocity { get; set; }
    public Entity Missile { get; set; }
}
