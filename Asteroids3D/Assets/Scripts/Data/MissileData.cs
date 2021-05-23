using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MissileData : IComponentData
{
    public bool isActive;
    public bool awake;
    public float lifeSpan;
    public float currentLifeSpan;
    public float3 initialVector;
}