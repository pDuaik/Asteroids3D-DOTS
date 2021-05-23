using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct AsteroidData : IComponentData
{
    public float3 asteroidAxisRotation;
    public float asteroidSpeedRotation;
    public float3 asteroidDirection;
    public float asteroidSpeed;
    public bool isActive;
}