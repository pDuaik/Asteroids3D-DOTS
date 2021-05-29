using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct QuadrantData : IComponentData
{
    public int3 quadrant;
}
