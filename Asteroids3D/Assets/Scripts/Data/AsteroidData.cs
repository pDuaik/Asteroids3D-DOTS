using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct AsteroidData : IComponentData
{
    // ECS Start populates this variables
    public float3 asteroidAxisRotation;
    public float asteroidSpeedRotation;
    public float3 asteroidDirection;
    public float asteroidSpeed;
}