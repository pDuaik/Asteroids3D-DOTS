using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    // Gameplay variables
    public float rotationSpeed;
    public float acceleration;
    public bool doubleShot;

    public float3 currentThrust { get; set; }
    public float3 currentVelocity { get; set; }
}
