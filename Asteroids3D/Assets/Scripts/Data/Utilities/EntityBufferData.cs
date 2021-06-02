using Unity.Entities;

[GenerateAuthoringComponent]
public struct EntityBufferData : IBufferElementData
{
    public Entity entity;
}