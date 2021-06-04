using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct WarpingData : IComponentData
{
    public bool isPlayer;
    public float canvasHalfSize;
    public float3 warpingValue;
}
