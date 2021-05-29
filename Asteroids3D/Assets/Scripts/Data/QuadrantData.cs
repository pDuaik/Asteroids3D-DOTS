using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct QuadrantData : IComponentData
{
    public float3 quadrant;
}
