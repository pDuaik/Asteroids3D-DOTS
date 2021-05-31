using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;

[GenerateAuthoringComponent]
public struct MissileData : IComponentData
{
    // Time it takes to destroy the missile
    public float lifeSpan;
    public float currentLifeSpan;

    // Inital velocity to pair with player
    public float3 initialVector;

    // Missile Hit
    public Entity hit;
}