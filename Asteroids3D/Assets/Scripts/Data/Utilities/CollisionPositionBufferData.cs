using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct CollisionPositionBufferData : IBufferElementData
{
    public float3 position;
}