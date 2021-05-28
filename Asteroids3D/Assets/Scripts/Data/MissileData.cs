using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MissileData : IComponentData
{
    public float lifeSpan;
    public float currentLifeSpan;
    public float3 initialVector;
    public bool doubleShot;
}