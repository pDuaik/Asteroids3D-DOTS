using Unity.Entities;

[GenerateAuthoringComponent]
public struct CollisionEntityBufferData : IBufferElementData
{
    public Entity entity;
}