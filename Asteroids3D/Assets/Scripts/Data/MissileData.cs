using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MissileData : IComponentData
{
    // Time it takes to destroy the missile
    public float lifeSpan;
    public float currentLifeSpan;

    // Inital velocity to pair with player
    public float3 initialVector;

    // If it is a double shot
    public bool doubleShot;
}