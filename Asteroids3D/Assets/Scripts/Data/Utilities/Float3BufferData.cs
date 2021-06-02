using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Float3BufferData : IBufferElementData
{
    public float3 Value;
}
