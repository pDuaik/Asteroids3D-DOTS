using Unity.Entities;

[GenerateAuthoringComponent]
public struct WarpingData : IComponentData
{
    public bool isPlayer;
    public float canvasHalfSize;
    public bool update;
}
