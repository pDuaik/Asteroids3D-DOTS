using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct AsteroidData : IComponentData
{
    public float3 asteroidAxisRotation { get; set; }
    public float asteroidSpeedRotation { get; set; }
}