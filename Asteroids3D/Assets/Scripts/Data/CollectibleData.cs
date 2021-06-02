using Unity.Entities;

[GenerateAuthoringComponent]
public struct CollectibleData : IComponentData
{
    public bool shield;
    public bool power;
}
